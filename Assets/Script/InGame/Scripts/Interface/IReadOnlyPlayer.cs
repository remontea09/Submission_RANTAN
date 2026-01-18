using System;
using System.Collections.Generic;

public interface IReadOnlyPlayer {
    public Index2D StagePosition { get; }
    public EntityType TargetType { get; }
    public int MaxHp { get; }
    public int CurrentHp { get; }
    public int MagicPower { get; }
    public int Defense { get; }
    public int Speed { get; }

    public IReadOnlyDictionary<StatusType, SortedDictionary<BuffCalculationType, List<CharacterStatusBuff>>> StatusBuffs { get; }
    public IReadOnlyList<OnTurnEndBuff> TurnEndBuffs { get; }

    public event Action OnMove;
    public event Action OnChangeBuff;
    public event Action<Index2D> OnChangePosition;
    public event Action OnDeathAction;

    public event Action<int, int> OnChangeHp;
    public event Action<int, int> OnChangePassiveHp;
    public event Action OnDecideAction;
}
