using UnityEngine;

public abstract class CharacterStatusBuff : CharacterBuff {
    public StatusType Type { get; }
    public abstract BuffCalculationType CalculationType { get; }
    public abstract StatusBuffType EffectValueFormula { get; }
    public int Amount { get; private set; }

    protected CharacterStatusBuff(StatusType type, int? duration, int amount, Sprite icon) : base(duration, icon) {
        Type = type;
        DurationTurns = duration;
        Amount = amount;
    }

    public abstract float CalculateBuffValue(StageCharacter character);
}



public abstract class AdditiveBuff : CharacterStatusBuff {
    public override BuffCalculationType CalculationType => BuffCalculationType.Additive;
    protected AdditiveBuff(StatusType buffType, int? duration, int amount, Sprite icon) : base(buffType, duration, amount, icon) { }
}

public abstract class MultiplicativeBuff : CharacterStatusBuff {
    public override BuffCalculationType CalculationType => BuffCalculationType.Multiplicative;
    protected MultiplicativeBuff(StatusType buffType, int? duration, int amount, Sprite icon) : base(buffType, duration, amount, icon) { }
}

public abstract class AddRateBuff : CharacterStatusBuff {
    public override BuffCalculationType CalculationType => BuffCalculationType.AddRate;
    protected AddRateBuff(StatusType buffType, int? duration, int amount, Sprite icon) : base(buffType, duration, amount, icon) { }
}
