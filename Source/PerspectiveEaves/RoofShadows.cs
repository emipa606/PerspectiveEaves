using System.Linq;
using RimWorld;
using Verse;

namespace PerspectiveEaves;

public static class RoofShadows
{
    private static readonly Building dummyBuilding = new Building { def = ThingDefOf.Door };

    public static Building[] GetAdjustedList(EdificeGrid edificeGrid)
    {
        var map = edificeGrid.map;
        var workingList = map.edificeGrid.innerArray.ToArray();
        for (var i = 0; i < map.roofGrid.roofGrid.Length; i++)
        {
            var edifice = workingList[i];
            //If this cell is roofed && is outside
            if (map.roofGrid.roofGrid[i] != null && //Is roofed?
                (map.cellIndices.IndexToCell(i).GetRoom(map)?.UsesOutdoorTemperature ?? false) && //Is outside?
                (edifice == null || edifice.def.staticSunShadowHeight != 1f) //Isn't ontop of another wall?
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
    public static void MapMeshDirty(MapDrawer mapDrawer, IntVec3 c, ulong dirtyFlags)
    {
        mapDrawer.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Roofs);
    }
}