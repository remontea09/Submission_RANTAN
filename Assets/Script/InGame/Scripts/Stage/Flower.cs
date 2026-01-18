
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;

public class Flower : StageEntity {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void InitFlower() {
        var stagePosition = FindFlowerPosition();
        base.InitStageEntity(EntityType.Flower, stagePosition);
        _spriteRenderer.transform.DOLocalMoveY(_spriteRenderer.transform.localPosition.y + 0.2f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        _spriteRenderer.sprite = Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId].FlowerSprite;
    }

    public void SetBoss() {
        var enemyMainDataArray = Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId].EnemyMainDataArray;
        var enemyMainData = enemyMainDataArray[enemyMainDataArray.Length - 1];
        var enemy = StageService.Instance.SpawnEnemy(enemyMainData, StagePosition);
        _spriteRenderer.enabled = false;
        transform.SetParent(enemy.transform);
        enemy.OnChangePosition += ((pos) => StagePosition = pos);
        enemy.OnDeathAction += (() => {
            transform.SetParent(null);
            transform.position = CoordinateConvertUtil.StageToWorldPosition(enemy.StagePosition);
            _spriteRenderer.enabled = true;
        });

        // ボスにバフ追加
        var buff = StatusBuffFactory.Create(StatusBuffType.NormalMultiplicative, StatusType.MagicPower, duration: null, 300);
        enemy.AddStatusBuff(buff);
        buff = StatusBuffFactory.Create(StatusBuffType.NormalMultiplicative, StatusType.Defense, duration: null, 300);
        enemy.AddStatusBuff(buff);
        buff = StatusBuffFactory.Create(StatusBuffType.NormalMultiplicative, StatusType.Speed, duration: null, 200);
        enemy.AddStatusBuff(buff);
    }



    private static Index2D FindFlowerPosition() {
        var height = StageService.Instance.StageHeight;
        var width = StageService.Instance.StageWidth;
        int[,] distance = new int[height, width];
        for (int v = 0; v < height; v++) {
            for (int h = 0; h < width; h++) {
                if (StageService.Instance.GetTile(v, h) != TileType.Floor) {
                    distance[v, h] = -1;
                }
            }
        }

        var campPosition = StageService.Instance.CampPosition;
        Queue<Index2D> searchPositionQueue = new();
        searchPositionQueue.Enqueue(campPosition);
        distance[campPosition.v, campPosition.h] = 1;
        int maxDistance = 0;
        List<Index2D> maxDistancePositionList = new();

        while (searchPositionQueue.Count > 0) {
            var nowPosition = searchPositionQueue.Dequeue();
            var newDistance = distance[nowPosition.v, nowPosition.h] + 1;
            foreach (var direction in Direction.FourDirections) {
                var nextPosition = nowPosition + direction;
                bool isInHeight = 0 <= nextPosition.h && nextPosition.h < height;
                bool isInWidth = 0 <= nextPosition.v && nextPosition.v < width;
                if (isInHeight && isInWidth && distance[nextPosition.v, nextPosition.h] == 0) {
                    distance[nextPosition.v, nextPosition.h] = newDistance;
                    if (newDistance >= maxDistance) {
                        if (newDistance > maxDistance) {
                            maxDistancePositionList.Clear();
                            maxDistance = newDistance;
                        }
                        maxDistancePositionList.Add(nextPosition);
                    }
                    searchPositionQueue.Enqueue(nextPosition);
                }
            }
        }

        var random = new System.Random(GameSessionService.Instance.DungeonSeed);
        var index = random.Next(0, maxDistancePositionList.Count);
        var flowerPosition = maxDistancePositionList[index];

        int GetWallSqrtDistance(Index2D basePosition, Direction direction) {
            int distance = 0;
            var searchPosition = basePosition + direction;
            while (StageService.Instance.GetTile(searchPosition) == TileType.Floor) {
                searchPosition += direction;
                distance++;
            }
            return (int)Math.Sqrt(distance);
        }

        int verticalShift = GetWallSqrtDistance(flowerPosition, Direction.Down) - GetWallSqrtDistance(flowerPosition, Direction.Up);
        int horizontalShift = GetWallSqrtDistance(flowerPosition, Direction.Right) - GetWallSqrtDistance(flowerPosition, Direction.Left);
        // シフト量が大きいほうを優先(同じ場合は横優先)
        if (verticalShift > horizontalShift) {
            flowerPosition.v += verticalShift;
            horizontalShift = GetWallSqrtDistance(flowerPosition, Direction.Right) - GetWallSqrtDistance(flowerPosition, Direction.Left);
            flowerPosition.h += horizontalShift;
        }
        else {
            flowerPosition.h += horizontalShift;
            verticalShift = GetWallSqrtDistance(flowerPosition, Direction.Down) - GetWallSqrtDistance(flowerPosition, Direction.Up);
            flowerPosition.v += verticalShift;
        }

        return flowerPosition;
    }
}
