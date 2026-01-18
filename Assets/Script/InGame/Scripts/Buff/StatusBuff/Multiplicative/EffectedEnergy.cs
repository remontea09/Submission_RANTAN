using System;
using UnityEngine;

public class EffectedEnergy : MultiplicativeBuff {
    public override StatusBuffType EffectValueFormula => StatusBuffType.EffectedEnergy;
    public override string Description => $"エネルギーの所持数に応じて{StatusTypeTexts.StatusNameDictionary[Type]}が上昇";

    public EffectedEnergy(StatusType type, int? duration, Sprite icon) : base(type, duration, 0, icon) { }

    public override float CalculateBuffValue(StageCharacter character) {
        var energy = GameSessionService.Instance.Energy;
        var count = Type switch {
            StatusType.MagicPower => energy.red,
            StatusType.Defense => energy.blue,
            StatusType.Speed => energy.green,
            _ => throw new NotImplementedException()
        };
        var amount = (int)Math.Sqrt(count);
        return amount * 0.01f + 1;
    }
}
