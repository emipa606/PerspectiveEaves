using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace PerspectiveEaves;

[HarmonyPatch(typeof(SectionLayer_SunShadows), nameof(SectionLayer_SunShadows.Regenerate))]
public class SectionLayer_SunShadows_Regenerate
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Property(typeof(EdificeGrid), nameof(EdificeGrid.InnerArray)).GetGetMethod(),
            AccessTools.Method(typeof(RoofShadows), nameof(RoofShadows.GetAdjustedList)));
    }
}