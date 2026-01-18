using UnityEngine;

public class NormalAddRate : AddRateBuff {
    public override StatusBuffType EffectValueFormula => StatusBuffType.NormalAddRate;
    public override string Description => $"{StatusTypeTexts.StatusNameDictionary[Type]}に{Amount}%の加算補正";

    public NormalAddRate(StatusType type, int? duration, int amount, Sprite icon) : base(type, duration, amount, icon) { }

    public override float CalculateBuffValue(StageCharacter character) {
        return Amount * 0.01f;
    }
}

