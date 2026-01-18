using System.Collections.Generic;
using System.Linq;
using Common.Const;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileGenerator {
    private static Dictionary<TileType, TileBase> tileDict = new Dictionary<TileType, TileBase> {
        { TileType.Floor, null },
        { TileType.Wall, null }
    };
    public static void GenerateStageTile(Tilemap tilemap) {
        int margin = GrobalConst.TILEMAP_MARGIN;

        DungeonData dungeonData = Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId];
        tileDict[TileType.Floor] = dungeonData.FloorTile;
        tileDict[TileType.Wall] = dungeonData.WallTile;

        GenerateTile(tilemap, margin);
    }

    public static void GenerateMiasmaTile(Tilemap tilemap, TileBase miasma) {
        int margin = GrobalConst.TILEMAP_MARGIN;

        foreach (var key in tileDict.Keys.ToList()) {
            tileDict[key] = miasma;
        }

        GenerateTile(tilemap, margin);
        HideTile(tilemap);
    }

    public static void GenerateMinimap(Tilemap tilemap, TileBase floor, TileBase wall) {
        int margin = GrobalConst.MINMAP_MARGIN;

        tileDict[TileType.Floor] = floor;
        tileDict[TileType.Wall] = wall;

        GenerateTile(tilemap, margin);
        HideTile(tilemap);
    }

    private static void GenerateTile(Tilemap tilemap, int margin) {

        tilemap.ClearAllTiles();

        int height = StageService.Instance.StageHeight;
        int width = StageService.Instance.StageWidth;

        for (int v = -margin; v < height + margin; v++) {
            for (int h = -margin; h < width + margin; h++) {
                Vector3Int tilePosition = new(h, -v, 0);
                TileType tileType = (v >= 0 && v < height && h >= 0 && h < width) ? StageService.Instance.GetTile(v, h) : TileType.Wall;

                TileBase tileBase = tileDict[tileType];
                if (tileBase) {
                    tilemap.SetTile(tilePosition, tileBase);
                }
            }
        }
    }

    private static void HideTile(Tilemap tilemap) {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            if (tilemap.HasTile(pos)) {
                Color color = new Color(1, 1, 1, 0);
                tilemap.SetColor(pos, color);
            }
        }
    }
}
