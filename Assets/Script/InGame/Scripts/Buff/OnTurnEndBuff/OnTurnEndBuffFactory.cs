using System;
using UnityEngine;

public static class OnTurnEOnndBuffFactory {
    public static OnTurnEndBuff Create(TriggeredBuffType buffType, int? duration, int amount = 0, Sprite icon = null) {
        return buffType switch {
            TriggeredBuffType.OnTurnEnd_Heal => new OnTurnEnd_Heal(duration, amount, icon),
            TriggeredBuffType.OnTurnEnd_EnergyHeal => new OnTurnEnd_EnergyHeal(duration, amount, icon),
            _ => throw new ArgumentException("引数が不正です"),
        };
    }
}

