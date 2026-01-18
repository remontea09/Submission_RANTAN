using UnityEngine;

public abstract class OnTurnEndBuff : CharacterBuff {
    public abstract TriggeredBuffType BuffType { get; }
    protected OnTurnEndBuff(int? duration, Sprite icon) : base(duration, icon) { }

    public abstract void ExecuteBuff(StageCharacter character);
}
