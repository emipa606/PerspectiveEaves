using HarmonyLib;
using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace PerspectiveEaves
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("owlchemist.perspectiveeaves");

            harmony.Patch(AccessTools.Method(typeof(SectionLayer_SunShadows), nameof(SectionLayer_SunShadows.Regenerate)),
			transpiler: new(typeof(HarmonyPatches), nameof(Replace_InnerArray)));

            harmony.Patch(AccessTools.Method(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.Regenerate)),
			transpiler: new(typeof(HarmonyPatches), nameof(Replace_InnerArray)));

            harmony.Patch(AccessTools.Method(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.Regenerate)),
			transpiler: new(typeof(HarmonyPatches), nameof(Replace_Roofed)));

            harmony.Patch(AccessTools.Method(typeof(RoofGrid), nameof(RoofGrid.SetRoof)),
			transpiler: new(typeof(HarmonyPatches), nameof(Replace_MapMeshDirty)));

            harmony.Patch(AccessTools.Method(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.HideRainPrimary)),
			prefix: new(typeof(HarmonyPatches), nameof(Patch_HideRainPrimary)));
        }

        static IEnumerable<CodeInstruction> Replace_InnerArray(IEnumerable<CodeInstruction> instructions)
            => instructions.MethodReplacer(AccessTools.Property(typeof(EdificeGrid), nameof(EdificeGrid.InnerArray)).GetGetMethod(),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.GetAdjustedList)));

        static IEnumerable<CodeInstruction> Replace_Roofed(IEnumerable<CodeInstruction> instructions)
            => instructions.MethodReplacer(AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new System.Type[] { typeof(IntVec3), typeof(Map) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.OutdoorRoofed)));

        static IEnumerable<CodeInstruction> Replace_MapDirty(IEnumerable<CodeInstruction> instructions)
            => instructions.MethodReplacer(AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new System.Type[] { typeof(IntVec3), typeof(Map) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.OutdoorRoofed)));

        static IEnumerable<CodeInstruction> Replace_MapMeshDirty(IEnumerable<CodeInstruction> instructions)
            => instructions.MethodReplacer(AccessTools.Method(typeof(MapDrawer), nameof(MapDrawer.MapMeshDirty), new System.Type[] { typeof(IntVec3), typeof(MapMeshFlag) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.MapMeshDirty)));


        static bool Patch_HideRainPrimary(SectionLayer_IndoorMask __instance, IntVec3 c, ref bool __result)
        {   
            if (__instance.Map.roofGrid.Roofed(c) && (c.GetRoom(__instance.Map)?.UsesOutdoorTemperature ?? false)) {
                __result = false;
                return false;
            }
            return true;
        }
    }

    public static class RoofShadows
    {
        static Building dummyBuilding = new Building(){ def = ThingDefOf.Door };
        public static Building[] GetAdjustedList(EdificeGrid edificeGrid)
        {
            Map map = edificeGrid.map;
            Building[] workingList = map.edificeGrid.innerArray.ToArray();
            for (int i = 0; i < map.roofGrid.roofGrid.Length; i++)
            {
                var edifice = workingList[i];
                //If this cell is roofed && is outside
                if (map.roofGrid.roofGrid[i] != null &&  //Is roofed?
                (map.cellIndices.IndexToCell(i).GetRoom(map)?.UsesOutdoorTemperature ?? false) &&  //Is outside?
                (edifice == null || edifice.def.staticSunShadowHeight != 1f) //Is not ontop of another wall?
                )
                {
                    workingList[i] = dummyBuilding;
                }
            }

            return workingList;
        }

        public static bool OutdoorRoofed(this IntVec3 c, Map map)
        {
            return map.roofGrid.Roofed(c) && (!c.GetRoom(map)?.UsesOutdoorTemperature ?? false);
        }

        public static void MapMeshDirty(MapDrawer mapDrawer, IntVec3 c, MapMeshFlag dirtyFlags)
        {
            mapDrawer.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Buildings | MapMeshFlag.Roofs);
        }
    }
}