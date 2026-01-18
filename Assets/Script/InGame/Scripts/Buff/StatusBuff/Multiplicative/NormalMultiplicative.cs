using UnityEngine;

public class NormalMultiplicative : MultiplicativeBuff {
    public override StatusBuffType EffectValueFormula => StatusBuffType.NormalMultiplicative;
    public override string Description => $"{StatusTypeTexts.StatusNameDictionary[Type]}に{Amount}%の乗算補正";

    public NormalMultiplicative(StatusType type, int? duration, int amount, Sprite icon) : base(type, duration, amount, icon) { }

    public override float CalculateBuffValue(StageCharacter character) {
        return Amount * 0.01f;
    }
}
