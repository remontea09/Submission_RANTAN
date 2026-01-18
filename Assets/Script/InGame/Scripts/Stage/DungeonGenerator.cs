using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class DungeonGenerator {
    private readonly static int MIN_SIDE = 2;
    private readonly static int MAX_SIDE = 18;
    private readonly static int DIV_LEN = 10;
    private readonly static float SPLIT_RATIO_MIN = 0.4f;
    private readonly static float SPLIT_RATIO_MAX = 0.6f;
    private readonly static float MIN_ROOM_SCALE_RATIO = 0.75f;
    private readonly static int CAMP_SIDE = 5;

    private System.Random random;
    private int idCount = 0;

    private class Node {
        public int id;
        public int x;
        public int y;
        public int height;
        public int width;
        public Index2D AnchorPoint { get; private set; } // 通路生成の基準点
        public Node(ref int id, int x, int y, int height, int width) {
            this.id = id++;
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
        }
        public bool IsLeaf() {
            return (height >= DIV_LEN && width >= DIV_LEN) || (height > MAX_SIDE || width > MAX_SIDE);
        }
        public void ConvertRoom(System.Random random) {
            // 空間内の上下左右を1マスずつパディングする
            x++;
            y++;
            height -= 2;
            width -= 2;

            // 頂点位置、縦横の長さを乱数で変更
            var addx = random.Next(0, height / 4);
            var addy = random.Next(0, width / 4);
            x += addx;
            height -= addx;
            y += addy;
            width -= addy;
            int minHeight = (int)(height * MIN_ROOM_SCALE_RATIO);
            if (minHeight >= MIN_SIDE) {
                height = random.Next(minHeight, height + 1);
            }
            int minWidth = (int)(width * MIN_ROOM_SCALE_RATIO);
            if (minWidth >= MIN_SIDE) {
                width = random.Next(minWidth, width + 1);
            }
            SetAnchorPoint(random);
        }
        private void SetAnchorPoint(System.Random random) {
            var virtical = random.Next(height);
            var horizontal = random.Next(width);
            AnchorPoint = new Index2D(x + virtical, y + horizontal);
        }
    }

    public TileType[,] GenerateDungeon() {
        int height = GameSessionService.Instance.DungeonSize;
        int width = GameSessionService.Instance.DungeonSize;
        random = new System.Random(GameSessionService.Instance.DungeonSeed);
        var terrainMap = GenerateTerrainMap(height, width);

        return terrainMap;
    }

    public Index2D GenerateCamp(ref TileType[,] terrainMap) {
        int mapHeight = terrainMap.GetLength(0);
        int mapWidth = terrainMap.GetLength(1);
        Index2D campOrigin = new(random.Next(mapHeight - CAMP_SIDE), random.Next(mapWidth - CAMP_SIDE));
        for (int i = 0; i < CAMP_SIDE; ++i) {
            for (int j = 0; j < CAMP_SIDE; ++j) {
                int x = campOrigin.v + i;
                int y = campOrigin.h + j;
                terrainMap[x, y] = TileType.Floor;
            }
        }
        int center = CAMP_SIDE / 2;
        Index2D campPosition = campOrigin + new Index2D(center, center);

        // 元マップも更新
        terrainMap = AdjustRoomDistance(terrainMap, ref campPosition);
        return campPosition;
    }

    private TileType[,] GenerateTerrainMap(int height, int width) {
        var roomList = GenerateRoomList(height, width);
        if (GameSessionService.Instance.CurrentStageCount > 1) {
            roomList.AddRange(GenerateRoomList(height, width));
        }

        TileType[,] terrainMap = new TileType[height, width];
        foreach (var room in roomList) {
            var x = room.x;
            var y = room.y;
            for (var i = 0; i < room.height; i++) {
                for (var j = 0; j < room.width; j++) {
                    terrainMap[x + i, y + j] = TileType.Floor;
                }
            }
        }
        GeneratePassageList(terrainMap);

        var randomIdx = random.Next(0, Direction.FourDirections.Length);
        ArrayUtil.Skew45(terrainMap, Direction.FourDirections[randomIdx]);

        return terrainMap;
    }

    private List<Node> GenerateRoomList(int height, int width) {
        // 空間を分割
        Node topNode = new(ref idCount, 0, 0, height, width);
        List<Node> nodeList = new() { topNode };
        Queue<Node> leafQueue = new();
        leafQueue.Enqueue(topNode);
        while (leafQueue.Count > 0) {
            var leaf = leafQueue.Dequeue();
            bool isVertical = leaf.height > leaf.width;
            int length = isVertical ? leaf.height : leaf.width;
            int minLength = Mathf.FloorToInt(length * SPLIT_RATIO_MIN);
            int maxLength = Mathf.FloorToInt(length * SPLIT_RATIO_MAX);
            int newLength = random.Next(minLength, maxLength);

            Node newNode = null;
            if (isVertical) {
                newNode = new Node(ref idCount, leaf.x + newLength, leaf.y, leaf.height - newLength, leaf.width);
                leaf.height = newLength;
            }
            else {
                newNode = new Node(ref idCount, leaf.x, leaf.y + newLength, leaf.height, leaf.width - newLength);
                leaf.width = newLength;
            }

            nodeList.Add(newNode);
            if (newNode.IsLeaf()) {
                leafQueue.Enqueue(newNode);
            }
            if (leaf.IsLeaf()) {
                leafQueue.Enqueue(leaf);
            }
        }

        // 部屋作成
        foreach (var node in nodeList) {
            node.ConvertRoom(random);
        }

        return nodeList;
    }

    private void GeneratePassageList(TileType[,] terrainMap) {
        // 通路生成
        int height = terrainMap.GetLength(0);
        int width = terrainMap.GetLength(1);

        while (true) {
            // マッピング
            int[,] roomIdMap = new int[height, width];
            int now = 1;
            bool[,] visited = new bool[height, width];

            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    if (terrainMap[i, j] == TileType.Floor && !visited[i, j]) {
                        Queue<Index2D> queue = new();
                        queue.Enqueue(new(i, j));
                        visited[i, j] = true;
                        roomIdMap[i, j] = now;

                        while (queue.Count > 0) {
                            var pos = queue.Dequeue();
                            foreach (var dir in Direction.FourDirections) {
                                int nv = pos.v + dir.v;
                                int nh = pos.h + dir.h;

                                if (nv >= 0 && nv < height && nh >= 0 && nh < width &&
                                    terrainMap[nv, nh] == TileType.Floor && !visited[nv, nh]) {
                                    visited[nv, nh] = true;
                                    roomIdMap[nv, nh] = now;
                                    queue.Enqueue(new Index2D(nv, nh));
                                }
                            }
                        }
                        now++;
                    }
                }
            }

            // 全部屋統合完了
            if (now <= 2) {
                break;
            }

            // リスト化
            List<Index2D>[] floorIndexListArray = new List<Index2D>[now - 1];
            for (int i = 0; i < floorIndexListArray.Length; ++i) {
                floorIndexListArray[i] = new();
            }
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    if (roomIdMap[i, j] != 0) {
                        floorIndexListArray[roomIdMap[i, j] - 1].Add(new(i, j));
                    }
                }
            }

            // ランダムに二つ選択して部屋を結合
            int firstIdx = random.Next(floorIndexListArray.Length);
            int secondIdx = random.Next(floorIndexListArray.Length - 1);
            secondIdx += secondIdx >= firstIdx ? 1 : 0;

            int firstListIdx = random.Next(floorIndexListArray[firstIdx].Count);
            int secondListIdx = random.Next(floorIndexListArray[secondIdx].Count);
            var firstAnchor = floorIndexListArray[firstIdx][firstListIdx];
            var secondAnchor = floorIndexListArray[secondIdx][secondListIdx];

            int virticalDist = firstAnchor.v - secondAnchor.v;
            int horizontalDist = firstAnchor.h - secondAnchor.h;
            int signX = -Math.Sign(virticalDist);
            int signY = -Math.Sign(horizontalDist);

            int x = firstAnchor.v;
            int y = firstAnchor.h;
            // そろえる順番はどっちが先でもいい
            for (int i = 0; i < Mathf.Abs(virticalDist); i++) {
                x += signX;
                terrainMap[x, y] = TileType.Floor;
            }
            for (int i = 0; i < Mathf.Abs(horizontalDist) - 1; i++) {
                y += signY;
                terrainMap[x, y] = TileType.Floor;
            }
        }
    }

    private TileType[,] AdjustRoomDistance(TileType[,] terrainMap, ref Index2D campPosition) {
        Index2D campOrigin = campPosition;
        int height = terrainMap.GetLength(0);
        int width = terrainMap.GetLength(1);
        int[] copyRows = new int[height + 1];
        int[] copyCols = new int[width + 1];
        int rowCount = height;
        int colCount = width;
        for (int i = 1; i < height - 1; ++i) {
            for (int j = 1; j < width - 1; ++j) {
                if (terrainMap[i, j] == TileType.Wall) {
                    if (copyRows[i] == 0 && terrainMap[i - 1, j] == TileType.Floor && terrainMap[i + 1, j] == TileType.Floor) {
                        copyRows[i]++;
                        rowCount++;
                        if (campOrigin.v > i) {
                            campPosition.v++;
                        }
                    }
                    if (copyCols[j] == 0 && terrainMap[i, j - 1] == TileType.Floor && terrainMap[i, j + 1] == TileType.Floor) {
                        copyCols[j]++;
                        colCount++;
                        if (campOrigin.h > j) {
                            campPosition.h++;
                        }
                    }
                }
            }
        }

        TileType[,] newTerrainMap = new TileType[rowCount, colCount];
        int row = 0;
        for (int i = 0; i < rowCount; ++i) {
            int col = 0;
            int copyCount = copyCols[col];
            for (int j = 0; j < colCount; ++j) {
                newTerrainMap[i, j] = terrainMap[row, col];
                if (copyCount > 0) {
                    copyCount--;
                }
                else {
                    col++;
                    copyCount = copyCols[col];
                }
            }
            if (copyRows[row] > 0) {
                copyRows[row]--;
            }
            else {
                row++;
            }
        }
        return newTerrainMap;
    }
}
