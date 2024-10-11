using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace PerspectiveEaves;

[HarmonyPatch(typeof(RoofGrid), nameof(RoofGrid.SetRoof))]
public class RoofGrid_SetRoof
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Method(typeof(MapDrawer), nameof(MapDrawer.MapMeshDirty), [typeof(IntVec3), typeof(ulong)]),
            AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.MapMeshDirty)));
    }
}