using UnityEngine;
public class BasedOnCurrentHPPercent : AdditiveBuff {
    public override StatusBuffType EffectValueFormula => StatusBuffType.BasedOnCurrentHPPercent;
    public override string Description => $"現在HPの{Amount}%の値を{StatusTypeTexts.StatusNameDictionary[Type]}に加算";

    public BasedOnCurrentHPPercent(StatusType type, int? duration, int amount, Sprite icon) : base(type, duration, amount, icon) { }

    public override float CalculateBuffValue(StageCharacter character) {
        float effectValue = character.CurrentHp * (Amount * 0.01f);
        return effectValue;
    }
}
