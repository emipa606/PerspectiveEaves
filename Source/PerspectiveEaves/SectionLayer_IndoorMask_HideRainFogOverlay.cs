using HarmonyLib;
using Verse;

namespace PerspectiveEaves;

[HarmonyPatch(typeof(SectionLayer_IndoorMask), nameof(SectionLayer_IndoorMask.HideRainFogOverlay))]
public class SectionLayer_IndoorMask_HideRainFogOverlay
{
    private static bool Prefix(SectionLayer_IndoorMask __instance, IntVec3 c, ref bool __result)
    {
        var def = __instance?.Map?.roofGrid?.RoofAt(c);
        if (def == null)
        {
            return true;
        }

        if (def.isNatural || !(c.GetRoom(__instance.Map)?.UsesOutdoorTemperature ?? false))
        {
            return true;
        }

        __result = false;
        return false;
    }
}