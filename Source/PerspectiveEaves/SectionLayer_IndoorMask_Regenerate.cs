using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace PerspectiveEaves;

[HarmonyPatch(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.Regenerate))]
public class SectionLayer_IndoorMask_Regenerate
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Method(typeof(GridsUtility), nameof(GridsUtility.Roofed), [typeof(IntVec3), typeof(Map)]),
            AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.OutdoorRoofed)));
    }
}