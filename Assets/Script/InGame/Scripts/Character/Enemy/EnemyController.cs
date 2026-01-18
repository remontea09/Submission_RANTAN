using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

public class EnemyController : StageCharacter {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private EnergyRenderer _energyRenderer;
    
    private SkillData skillData;
    private Index2D? targetPosition;
    private EnergyType _energyType;
    private int viewSide;
    private bool isMoved;

    public void InitEnemyController(Index2D stagePosition, EnemyMainData mainData) {
        base.InitStageCharacter(EntityType.Enemy, stagePosition, EntityType.Player, mainData, GameSessionService.Instance.EnemyBaseLevel);
        base.ChangeName(mainData.Name);

        spriteRenderer.sprite = mainData.Sprite;
        skillData = mainData.SkillData;

        viewSide = mainData.ViewSide;
        isMoved = false;
        ChangeEnergy();
    }

    public override void DecideAndMove() {
        IsInCombatOrChase = false;
        Direction attackDirection = GetBestAttackDirection();
        if (attackDirection == Direction.None) {
            Direction direction = GetMoveDirection();
            ExecuteMove(direction);
            isMoved = true;
        }
        else {
            IsInCombatOrChase = true;
        }
    }

    public override void ExecuteTurn() {
        if (isMoved) {
            isMoved = false;
            return;
        }

        Direction attackDirection = GetBestAttackDirection();
        if (attackDirection != Direction.None) {
            ExecuteSkill(skillData, attackDirection);
        }
    }

    public override void TakeMiasmaDamage() { }

    private Direction GetBestAttackDirection() {
        int maxCount = 0;
        List<Direction> bestDirections = new();
        foreach (Direction direction in Direction.EightDirections) {
            int count = BattleHelper.GetTargetList(this.StagePosition, skillData.RangeType, direction)
                .Where(target => target.EntityType == this.TargetType)
                .Count();
            if (count >= maxCount) {
                if (count > maxCount) {
                    maxCount = count;
                    bestDirections.Clear();
                }
                bestDirections.Add(direction);
            }
        }
        if (maxCount == 0) {
            return Direction.None;
        }
        else {
            return bestDirections[UnityEngine.Random.Range(0, bestDirections.Count)];
        }
    }

    private Direction GetMoveDirection() {
        Direction moveDirection = Direction.None;

        var trackDirection = GetTrackingMove();
        if (trackDirection.HasValue) {
            moveDirection = trackDirection.Value;
        }
        else {
            if (this.targetPosition == null || this.targetPosition == StagePosition) {
                this.targetPosition = StageService.Instance.GetRandomFloorPosition();
            }
            moveDirection = GetBestMoveDirection(this.targetPosition.Value);
            if (moveDirection == Direction.None) {
                this.targetPosition = null;
            }
        }

        return moveDirection;
    }
    private Direction? GetTrackingMove() {
        Index2D offset = new(viewSide / 2, viewSide / 2);
        Queue<Index2D> queue = new();
        queue.Enqueue(offset);

        bool[,] seen = new bool[viewSide, viewSide];
        seen[offset.v, offset.h] = true;

        (int score, Direction firstDir)[,] evalGrid = new (int, Direction)[viewSide, viewSide];
        int INF = int.MaxValue / 2;
        for (int i = 0; i < evalGrid.GetLength(0); ++i) {
            for (int j = 0; j < evalGrid.GetLength(1); ++j) {
                evalGrid[i, j] = (INF, Direction.None);
                if (i == offset.v && j == offset.h) {
                    evalGrid[i, j].score = 0;
                }
            }
        }

        Direction enemyDirection = Direction.None;
        Direction allyDirection = Direction.None;

        while (queue.Count > 0) {
            var basePosition = queue.Dequeue();
            var baseStagePosition = basePosition + StagePosition - offset;
            List<Direction> validDirectionList = new();

            foreach (var direction in Direction.SearchDirections) {
                var searchPosition = basePosition + direction;
                if (searchPosition.v < 0 || searchPosition.h < 0 || searchPosition.v >= viewSide || searchPosition.h >= viewSide || seen[searchPosition.v, searchPosition.h]) continue;
                var searchStagePosition = baseStagePosition + direction;
                var propDir = evalGrid[basePosition.v, basePosition.h].firstDir == Direction.None ? direction : evalGrid[basePosition.v, basePosition.h].firstDir;

                var character = StageService.Instance.GetStageCharacter(searchStagePosition);
                if (character != null) {
                    if (this.TargetType == character.EntityType && enemyDirection == Direction.None) {
                        enemyDirection = propDir;
                    }
                    else if (this.TargetType == character.TargetType && character.IsInCombatOrChase && allyDirection == Direction.None) {
                        allyDirection = propDir;
                    }
                }

                if (!StageService.Instance.CanMove(baseStagePosition, direction)) continue;
                seen[searchPosition.v, searchPosition.h] = true;
                queue.Enqueue(searchPosition);
                validDirectionList.Add(direction);
            }

            foreach (var direction in validDirectionList) {
                var searchPosition = basePosition + direction;
                var nextScore = evalGrid[basePosition.v, basePosition.h].score + 1;
                var propDir = evalGrid[basePosition.v, basePosition.h].firstDir == Direction.None ? direction : evalGrid[basePosition.v, basePosition.h].firstDir;

                if (!StageService.Instance.CanMove(baseStagePosition, direction)) continue;

                if (nextScore < evalGrid[searchPosition.v, searchPosition.h].score) {
                    evalGrid[searchPosition.v, searchPosition.h] = (nextScore, propDir);
                }
            }
        }

        Direction? trackDirection = null;
        if (enemyDirection != Direction.None) {
            trackDirection = enemyDirection;
            IsInCombatOrChase = true;
        }
        else if (allyDirection != Direction.None) {
            trackDirection = allyDirection;
        }

        if (trackDirection.HasValue && !StageService.Instance.CanMove(StagePosition, trackDirection.Value)) {
            trackDirection = Direction.None;
        }

        return trackDirection;
    }

    private Direction GetBestMoveDirection(Index2D targetPositon) {
        var delta = new Direction(targetPositon - StagePosition);
        List<Direction> directionPriorityList = new() { delta };
        if (delta.v == 0) {
            directionPriorityList.Add(new(1, delta.h));
            directionPriorityList.Add(new(-1, delta.h));
        }
        else if (delta.h == 0) {
            directionPriorityList.Add(new(delta.v, 1));
            directionPriorityList.Add(new(delta.v, -1));
        }
        else {
            directionPriorityList.Add(new(delta.v, 0));
            directionPriorityList.Add(new(0, delta.h));
        }

        if (RandomHelper.Chance(2)) {
            (directionPriorityList[1], directionPriorityList[2]) = (directionPriorityList[2], directionPriorityList[1]);
        }
        foreach (var direction in directionPriorityList) {
            if (StageService.Instance.CanMove(StagePosition, direction)) {
                return direction;
            }
        }
        return Direction.None;
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        GameSessionService.Instance.Achievement.TotalTakeDamage += damage;
    }

    protected override void OnDeath() {
        base.OnDeath();
        IsDeath = true;
        AnimationService.Instance.AddDeathAnimation(transform, () => {
            LogService.Instance.WriteLog($"[{GameSessionService.Instance.TurnCount}] <color=#FF0000>{_name}</color>が倒れた");
            AudioService.Instance.PlaySE(AudioType.EnemyDeathSE);
            GameSessionService.Instance.AcquireEnergy(_energyType, (int)Math.Sqrt(Level) + Math.Abs(GameSessionService.Instance.PlayerLevel - Level));
            GameSessionService.Instance.AcquireXp(Level);
            gameObject.SetActive(false);
        });

        GameSessionService.Instance.Achievement.killedEnemyCount++;

        StageService.Instance.RemoveEnemy(this);
    }

    public void ChangeEnergy() {
        _energyType = RandomHelper.GetRandomEnumValue<EnergyType>();
        _energyRenderer.PlayChangeSpriteAnimation(_energyType);
    }
}
