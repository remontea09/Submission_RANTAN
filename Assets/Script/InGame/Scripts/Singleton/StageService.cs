using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Const;
using UnityEngine;

public class StageService : Base.Singleton<StageService> {
    public IReadOnlyDictionary<Index2D, StageCharacter> StageCharacterMap => stageCharacterMap;
    public List<Index2D> FloorPositionList { get; private set; }
    public Index2D CampPosition { get; private set; }

    private ObjectPool<EnemyController> enemyPool;
    private TileType[,] terrainMap;
    private Dictionary<Index2D, StageCharacter> stageCharacterMap;
    public Flower _flower;

    public int StageHeight => terrainMap.GetLength(0);
    public int StageWidth => terrainMap.GetLength(1);
    public TileType GetTile(int v, int h) {
        if (v < 0 || StageHeight <= v || h < 0 || StageWidth <= h) {
            return TileType.Wall;
        }
        return terrainMap[v, h];
    }
    public TileType GetTile(Index2D pos) => GetTile(pos.v, pos.h);

    public void InitStageServise(TileType[,] terrainMap, EnemyController enemyPrefab, Index2D campPosition, Flower flower) {
        stageCharacterMap = new();
        FloorPositionList = new();
        this.terrainMap = terrainMap;
        this.CampPosition = campPosition;
        _flower = flower;

        enemyPool = new ObjectPool<EnemyController>(
            createFunc: () => {
                return UnityEngine.Object.Instantiate(enemyPrefab);
            },
            actionOnGet: (obj) => {
                obj.transform.localScale = Vector3.one;
                obj.gameObject.SetActive(true);
            }
        );

        for (int i = 0; i < StageHeight; ++i)
            for (int j = 0; j < StageWidth; ++j)
                if (GetTile(i, j) == TileType.Floor)
                    FloorPositionList.Add(new Index2D(i, j));

        _flower.InitFlower();
        if (GameSessionService.Instance.CurrentFloorCount == DungeonConst.STAGE_PER_CYCLE) {
            _flower.SetBoss();
        }
    }

    public void SetStageCharacter(StageCharacter character) {
        var playerPosition = character.StagePosition;
        stageCharacterMap[playerPosition] = character;
    }

    public StageCharacter GetStageCharacter(Index2D position) {
        if (stageCharacterMap.ContainsKey(position))
            return stageCharacterMap[position];
        else return null;
    }

    public bool CanMove(Index2D oldPosition, Direction direction) {
        bool canMove = CanStandOn(oldPosition + direction);
        if (direction != Direction.None) {
            var searchPosition = new[] {
                oldPosition + new Index2D(direction.v, 0),
                oldPosition + new Index2D(0, direction.h)
             };
            foreach (var position in searchPosition) {
                if (GetTile(position) != TileType.Floor) {
                    canMove = false;
                }
            }
        }
        return canMove;
    }
    public bool CanStandOn(params Index2D[] positionArray) {
        return positionArray.All(position =>
            GetTile(position) == TileType.Floor && !stageCharacterMap.ContainsKey(position) && CampPosition != position
        );
    }

    public void UpdateStagePosition(Index2D oldPosition, Index2D newPosition, StageCharacter character) {
        stageCharacterMap.Remove(oldPosition);
        stageCharacterMap[newPosition] = character;

        if (character.EntityType == EntityType.Player && newPosition == _flower?.StagePosition) {
            GameSessionService.Instance.AcquireFlower();
            AudioService.Instance.PlayAcquireFlowerSE();
            UnityEngine.Object.Destroy(_flower.gameObject);
            _flower = null;
        }
    }

    public void RemoveEnemy(EnemyController enemy) {
        enemyPool.Release(enemy);

        stageCharacterMap.Remove(enemy.StagePosition);
    }

    public List<Index2D> TryGetRandomEmptyPositionList() {
        var playerViewArea = new ViewAreaInStage(Dao.StageInstance.Player.StagePosition);
        List<Index2D> emptyPositionList = new();
        foreach (var position in FloorPositionList) {
            bool isInViewArea = playerViewArea.Contains(position);
            if (!stageCharacterMap.ContainsKey(position) && !isInViewArea) {
                emptyPositionList.Add(position);
            }
        }
        if (emptyPositionList.Count == 0) {
            return null;
        }
        return emptyPositionList;
    }

    public Index2D GetRandomFloorPosition() {
        var randomIndex = UnityEngine.Random.Range(0, FloorPositionList.Count);
        return FloorPositionList[randomIndex];
    }

    public void SpawnEnemyRandomPosition(int spawnCount = 1) {
        var stagePositionList = TryGetRandomEmptyPositionList();
        if (stagePositionList.Count < spawnCount) return;
        RandomHelper.Shuffle(stagePositionList);
        spawnCount = Mathf.Min(spawnCount, stagePositionList.Count);
        for (int i = 0; i < spawnCount; ++i) {
            SpawnRandomEnemy(stagePositionList[i]);
        }
    }

    public EnemyController SpawnRandomEnemy(Index2D stagePosition) {
        var enemyMainDataArray = Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId].EnemyMainDataArray;
        int randomIndex = UnityEngine.Random.Range(0, enemyMainDataArray.Length - (DungeonConst.STAGE_PER_CYCLE - GameSessionService.Instance.CurrentFloorCount));
        return SpawnEnemy(enemyMainDataArray[randomIndex], stagePosition);
    }

    public EnemyController SpawnEnemy(EnemyMainData enemyMainData, Index2D stagePosition) {
        if (stageCharacterMap.ContainsKey(stagePosition)) return null;

        EnemyController enemy = enemyPool.Get();
        enemy.InitEnemyController(stagePosition, enemyMainData);
        stageCharacterMap[stagePosition] = enemy;
        return enemy;
    }

    public bool IsCamp(Index2D stagePosition) {
        return stagePosition == CampPosition;
    }
}
