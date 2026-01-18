using System;
using Common.Const;
using UnityEngine;

public enum PlayerType {
    Hero = 0,
    Heroine = 1
}

public class PlayerController : StageCharacter, IReadOnlyPlayer, IExecutablePlayer {
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private GameObject _flowerEffect;

    public override int Level {
        get {
            return GameSessionService.Instance.PlayerLevel;
        }
    }

    public event Action<int, int> OnChangePassiveHp;
    public event Action OnDecideAction;
    public Action turnAction = null;

    private int _passiveHp;

    public void InitPlayerController(Index2D playerStagePosition) {
        var master = GetCurrentPlayerMaster(true);
        _playerSprite.sprite = master.PlayerSprite;
        base.InitStageCharacter(EntityType.Player, playerStagePosition, EntityType.Enemy, master, 1);
        base.ChangeName(master.Name);

        _flowerEffect.SetActive(false);
        GameSessionService.Instance.OnAcquireFlower += OnAcquireFlower;

        var passiveMaster = GetCurrentPlayerMaster(false);
        var passiveMaxHp = GetStatusValue(passiveMaster.MaxHp, StatusType.MaxHp);
        _passiveHp = passiveMaxHp;

        InvokeChangeHp(MaxHp, CurrentHp);
        OnChangePassiveHp?.Invoke(passiveMaxHp, _passiveHp);
        AddStatusBuff(StatusBuffFactory.Create(StatusBuffType.EffectedEnergy, StatusType.MagicPower, duration: null));
        AddStatusBuff(StatusBuffFactory.Create(StatusBuffType.EffectedEnergy, StatusType.Defense, duration: null));
        AddStatusBuff(StatusBuffFactory.Create(StatusBuffType.EffectedEnergy, StatusType.Speed, duration: null));

        EventService.Instance.AddListner(EventType.TurnEnd, AutoHealHp);

        StageService.Instance.SetStageCharacter(this);
        Dao.StageInstance.Player = this;
        Dao.StageInstance.ExecutablePlayer = this;
    }

    private void ChangePlayer() {
        GameSessionService.Instance.ChangePlayer();

        var master = GetCurrentPlayerMaster(true);
        var passiveMaster = GetCurrentPlayerMaster(false);
        var passiveMaxHp = GetStatusValue(passiveMaster.MaxHp, StatusType.MaxHp);

        base._master = master;
        base.ChangeName(master.Name);

        (CurrentHp, _passiveHp) = (_passiveHp, CurrentHp);
        CurrentHp = Math.Min(CurrentHp, MaxHp);

        InvokeChangeHp(MaxHp, CurrentHp);
        OnChangePassiveHp?.Invoke(passiveMaxHp, _passiveHp);

        AnimationService.Instance.AddChangePlayerAnimation(transform, () => _playerSprite.sprite = master.PlayerSprite);
    }

    public override void DecideAndMove() {
        return;
    }

    public override void ExecuteTurn() {
        turnAction?.Invoke();
        turnAction = null;
    }

    public void ExecutePlayerMove(Direction direction) {
        IsInCombatOrChase = false;
        ExecuteMove(direction);
        OnDecideAction?.Invoke();
    }

    public void ExecutePlayerSkill(Direction direction) {
        IsInCombatOrChase = true;
        SkillData skill = null;
        if (direction == Direction.None) {
            skill = GameSessionService.Instance.PlayerEnableSupportSkill;
            if (GameSessionService.Instance.PlayerType != PlayerType.Heroine) ChangePlayer();
        }
        else {
            skill = GameSessionService.Instance.PlayerEnableAttackSkill;
            if (GameSessionService.Instance.PlayerType != PlayerType.Hero) ChangePlayer();
        }

        turnAction = (() => ExecuteSkill(skill, direction));
        OnDecideAction?.Invoke();
    }

    public override void HealHp(int point) {
        base.HealHp(point);
        _passiveHp += point;

        var passiveMainHp = GetCurrentPlayerMaster(false).MaxHp;
        AnimationService.Instance.AddActionToAnimation(startAction: () => {
            NumberEffectManager.PlayEffect(transform.position, point);
            }, completeAction: () => {
            InvokeChangeHp(MaxHp, CurrentHp);
            int passiveMaxHp = GetStatusValue(passiveMainHp, StatusType.MaxHp);
            OnChangePassiveHp?.Invoke(passiveMaxHp, _passiveHp);
            LogService.Instance.WriteLog($"[{GameSessionService.Instance.TurnCount}] 味方全体の体力が<color=#8BFF44>{point}</color>回復した。");
        });
    }

    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        GameSessionService.Instance.Achievement.TotalHitDamage += damage;
    }

    protected override void OnDeath() {
        AnimationService.Instance.AddDeathAnimation(transform, () => {
            base.OnDeath();
            AudioService.Instance.PlaySE(AudioType.EnemyDeathSE);
            gameObject.SetActive(false);
        });
    }

    private PlayerMainData GetCurrentPlayerMaster(bool isActive) {
        var type = isActive ? PlayerType.Hero : PlayerType.Heroine;
        var masterId = GameSessionService.Instance.PlayerType == type
            ? GameSessionService.Instance.HeroMainDataId
            : GameSessionService.Instance.HeroineMainDataId;
        var master = Dao.Master.PlayerMainMaster[masterId];
        return master;
    }

    private void AutoHealHp() {
        int healPoint = (int)(MaxHp * PlayerConst.AUTO_HEAL_RATE);
        base.HealHp(healPoint);
        _passiveHp += healPoint;
        var passiveMainHp = GetCurrentPlayerMaster(false).MaxHp;
        AnimationService.Instance.AddActionToAnimation(completeAction: () => {
            InvokeChangeHp(MaxHp, CurrentHp);
            int passiveMaxHp = GetStatusValue(passiveMainHp, StatusType.MaxHp);
            OnChangePassiveHp?.Invoke(passiveMaxHp, _passiveHp);
        });
    }

    private void OnAcquireFlower() {
        _flowerEffect.SetActive(true);
        var flowerBuff = StatusBuffFactory.Create(StatusBuffType.EffectedEnergy, StatusType.Speed, null, icon: Dao.Master.DungeonMaster[GameSessionService.Instance.DungeonDataId].FlowerSprite);
        AddStatusBuff(flowerBuff);
    }

    private void OnDestroy() {
        Dao.StageInstance.Player = null;
        Dao.StageInstance.ExecutablePlayer = null;
        EventService.Instance.RemoveListner(EventType.TurnEnd, AutoHealHp);
    }
}
