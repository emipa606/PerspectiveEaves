using System.Reflection;
using HarmonyLib;
using Verse;

namespace PerspectiveEaves;

[StaticConstructorOnStartup]
internal static class HarmonyPatches
{
    static HarmonyPatches()
    {
        new Harmony("owlchemist.perspectiveeaves").PatchAll(Assembly.GetExecutingAssembly());
    }
}