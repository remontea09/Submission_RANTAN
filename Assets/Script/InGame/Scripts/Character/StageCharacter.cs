using System;
using System.Collections.Generic;
using Common.Const;
using DG.Tweening;
using UnityEngine;

public abstract class StageCharacter : StageEntity {
    protected Tween idleTween;
    public int MaxHp => GetStatusValue(_master.MaxHp, StatusType.MaxHp);
    public int CurrentHp { get; protected set; }
    public int MagicPower => GetStatusValue(_master.MagicPower, StatusType.MagicPower);
    public int Defense => GetStatusValue(_master.Defense, StatusType.Defense);
    public int Speed => GetStatusValue(_master.Speed, StatusType.Speed);

    public virtual int Level { get; protected set; }

    protected IStatus _master;

    public bool IsDeath { get; protected set; }

    public EntityType TargetType { get; protected set; }
    public bool IsInCombatOrChase { get; protected set; }

    public event Action OnMove;
    public event Action OnChangeBuff;
    public event Action<Index2D> OnChangePosition;
    public event Action OnDeathAction;
    public event Action<int, int> OnChangeHp;

    public IReadOnlyDictionary<StatusType, SortedDictionary<BuffCalculationType, List<CharacterStatusBuff>>> StatusBuffs => _statusBuffDictionary;
    public IReadOnlyList<OnTurnEndBuff> TurnEndBuffs => _onTurnEndBuffList;

    private SortedDictionary<StatusType, SortedDictionary<BuffCalculationType, List<CharacterStatusBuff>>> _statusBuffDictionary;
    private List<OnTurnEndBuff> _onTurnEndBuffList;
    protected string _name;

    private int _miasmaDamage;

    protected void InitStageCharacter(EntityType entityType, Index2D stagePosition, EntityType targetType, IStatus master, int level) {
        base.InitStageEntity(entityType, stagePosition);
        this.TargetType = targetType;
        this._master = master;
        IsDeath = false;
        OnChangeBuff = null;
        OnChangePosition = null;
        OnDeathAction = null;
        _miasmaDamage = 0;

        Level = level;
        _statusBuffDictionary = new();
        _onTurnEndBuffList = new();
        EventService.Instance.AddListner(EventType.TurnEndBuff, ExecuteTurnEndBuff);
        CurrentHp = MaxHp;
    }

    protected void ChangeName(string name) => _name = name;

    public abstract void DecideAndMove();

    public abstract void ExecuteTurn();
    protected virtual void ExecuteMove(Direction direction) {
        var oldPosition = StagePosition;
        StagePosition += direction;
        OnMove?.Invoke();
        AnimationService.Instance.AddMoveAnimation(transform, this.EntityType, StagePosition);
        if (oldPosition != StagePosition) {
            StageService.Instance.UpdateStagePosition(oldPosition, StagePosition, this);
            OnChangePosition?.Invoke(StagePosition);
        }
    }

    protected void ExecuteSkill(SkillData skillData, Direction direction) {
        var logColor = EntityType == EntityType.Player ? "#8BFF44" : "#FF0000";
        var skillLog = $"[{GameSessionService.Instance.TurnCount}] <color={logColor}>{_name}</color>の<color={logColor}>{(EntityType == EntityType.Player ? skillData.Name : "攻撃")}</color>";
        Action startAction = () => {
            if (EntityType == EntityType.Player) {
                GameSessionService.Instance.UseEnergy(skillData.RequiredEnergy);
            }
        };
        Action completeAction = () => LogService.Instance.WriteLog(skillLog);
        AnimationService.Instance.AddSkillAnimation(transform, this.EntityType, StagePosition, direction, skillData, startAction, completeAction);
        BattleHelper.Processkill(this, skillData, direction);
    }

    public void AddStatusBuff(CharacterStatusBuff buff) {
        buff.OnEndBuff += RemoveBuff;

        if (!_statusBuffDictionary.ContainsKey(buff.Type)) _statusBuffDictionary.Add(buff.Type, new());
        if (!_statusBuffDictionary[buff.Type].ContainsKey(buff.CalculationType)) _statusBuffDictionary[buff.Type].Add(buff.CalculationType, new());
        _statusBuffDictionary[buff.Type][buff.CalculationType].Add(buff);
        OnChangeBuff?.Invoke();
    }

    public void AddTurnEndBuff(OnTurnEndBuff buff) {
        buff.OnEndBuff += RemoveBuff;
        _onTurnEndBuffList.Add(buff);
        OnChangeBuff?.Invoke();
    }

    public void AddBuffTurn(int turn) {
        foreach (var x in _statusBuffDictionary.Values) {
            foreach (var buffList in x.Values) {
                foreach (var buff in buffList) {
                    buff.AddTurn(turn);
                }
            }
        }
        foreach (var buff in _onTurnEndBuffList) {
            buff.AddTurn(turn);
        }
        OnChangeBuff?.Invoke();
    }

    public virtual void HealHp(int point) {
        CurrentHp = Math.Min(CurrentHp + point, MaxHp);
    }

    public virtual void TakeDamage(int damage) {
        CurrentHp -= damage;

        var logColor = EntityType == EntityType.Player ? "#8BFF44" : "#FF0000";
        var skillLog = $"[{GameSessionService.Instance.TurnCount}] <color={logColor}>{_name}</color>に<color={logColor}>{damage}</color>の<color={logColor}>ダメージ</color>";
        var maxHp = MaxHp;
        var curHp = CurrentHp;
        Action completeAction = () => {
            LogService.Instance.WriteLog(skillLog);
            OnChangeHp?.Invoke(maxHp, curHp);
        };
        AnimationService.Instance.AddHitAnimation(transform, EntityType, damage, completeAction);

        if (CurrentHp <= 0) {
            CurrentHp = 0;
            OnDeath();
        }
    }

    public virtual void TakeMiasmaDamage() {
        _miasmaDamage += 1;
        Action completeAction = () => TakeDamage(PlayerConst.MIASMA_DAMAGE + _miasmaDamage);
        AnimationService.Instance.AddHitAnimation(transform, this.EntityType, _miasmaDamage, completeAction, wait: true);
    }

    protected virtual void OnDeath() {
        OnDeathAction?.Invoke();
    }

    protected void InvokeChangeHp(int maxHp, int currentHp) => OnChangeHp?.Invoke(maxHp, currentHp);

    private void ExecuteTurnEndBuff() {
        foreach (var buff in _onTurnEndBuffList) {
            buff.ExecuteBuff(this);
        }
    }

    private void RemoveBuff(CharacterBuff buff) {
        buff.OnEndBuff -= RemoveBuff;
        if (buff is CharacterStatusBuff statusBuff) {
            _statusBuffDictionary[statusBuff.Type][statusBuff.CalculationType].Remove(statusBuff);
        }
        else if (buff is OnTurnEndBuff turnEndBuff) {
            _onTurnEndBuffList.Remove(turnEndBuff);
        }
        OnChangeBuff?.Invoke();
    }

    protected int GetStatusValue(int defaultValue, StatusType buffType) {
        float baseValue = buffType switch {
            StatusType.MaxHp => defaultValue * LevelUtil.GetLevelCoefficientForHp(Level),
            _ => defaultValue * LevelUtil.GetLevelCoefficient(Level)
        };
        if (!_statusBuffDictionary.ContainsKey(buffType)) return (int)baseValue;

        float addValue = GetAdditiveValue(buffType);
        float multiplicativeValue = GetMultiplicativeValue(buffType);
        float addRateValue = GetAddRateValue(buffType);


        int resultValue = (int)((baseValue + addValue) * (multiplicativeValue + addRateValue));
        return Math.Max(resultValue, 50);
    }

    private float GetAdditiveValue(StatusType buffType) {
        if (!_statusBuffDictionary[buffType].ContainsKey(BuffCalculationType.Additive)) return 0;

        float additiveValue = 0;
        foreach (CharacterStatusBuff buff in _statusBuffDictionary[buffType][BuffCalculationType.Additive]) {
            additiveValue += buff.CalculateBuffValue(this);
        }
        return additiveValue;
    }

    private float GetMultiplicativeValue(StatusType buffType) {
        if (!_statusBuffDictionary[buffType].ContainsKey(BuffCalculationType.Multiplicative)) return 1f;

        float multiplicative = 1f;
        foreach (CharacterStatusBuff buff in _statusBuffDictionary[buffType][BuffCalculationType.Multiplicative]) {
            multiplicative *= buff.CalculateBuffValue(this);
        }
        return multiplicative;
    }

    private float GetAddRateValue(StatusType buffType) {
        if (!_statusBuffDictionary[buffType].ContainsKey(BuffCalculationType.AddRate)) return 0f;

        float addRate = 0;
        foreach (CharacterStatusBuff buff in _statusBuffDictionary[buffType][BuffCalculationType.AddRate]) {
            Debug.Log(buff.ToString());
            addRate += buff.CalculateBuffValue(this);
        }
        return addRate;
    }
}
