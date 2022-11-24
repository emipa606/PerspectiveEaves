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
            new Harmony("owlchemist.perspectiveeaves").PatchAll();
        }
    }

    [HarmonyPatch(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.HideRainPrimary))]
	public class Patch_NoSunlight
    {
        static bool Prefix(SectionLayer_IndoorMask __instance, IntVec3 c, ref bool __result)
        {   
            RoofDef def = __instance.Map.roofGrid.RoofAt(c);
            if (def?.isNatural == false && (c.GetRoom(__instance.Map)?.UsesOutdoorTemperature ?? false)) {
                __result = false;
                return false;
            }
            return true;
        }
    }
        
    [HarmonyPatch(typeof(SectionLayer_SunShadows), nameof(SectionLayer_SunShadows.Regenerate))]
	public class Patch_SectionLayer_SunShadows
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Property(typeof(EdificeGrid), nameof(EdificeGrid.InnerArray)).GetGetMethod(),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.GetAdjustedList)));
        }
    }

    [HarmonyPatch(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.Regenerate))]
	public class Patch_SectionLayer_IndoorMask
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), new System.Type[] { typeof(IntVec3), typeof(Map) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.OutdoorRoofed)));
        }
    }

    [HarmonyPatch(typeof(RoofGrid), nameof(RoofGrid.SetRoof))]
	public class Patch_RoofGrid
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(MapDrawer), nameof(MapDrawer.MapMeshDirty), new System.Type[] { typeof(IntVec3), typeof(MapMeshFlag) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.MapMeshDirty)));
        }
    }

    [HarmonyPatch(typeof(Building), nameof(Building.SpawnSetup))]
	public class Patch_SpawnSetup
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(AccessTools.Method(typeof(MapDrawer), nameof(MapDrawer.MapMeshDirty), new System.Type[] { typeof(IntVec3), typeof(MapMeshFlag) }),
				AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.MapMeshDirty)));
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

        //Swaps out map mesh updates from building only to building AND roofs
        public static void MapMeshDirty(MapDrawer mapDrawer, IntVec3 c, MapMeshFlag dirtyFlags)
        {
            mapDrawer.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Buildings | MapMeshFlag.Roofs);
        }
    }
}