using System;
using UnityEngine;

public static class StatusBuffFactory {
    public static CharacterStatusBuff Create(StatusBuffType formula, StatusType type, int? duration, int amount = 0, Sprite icon = null) {
        return formula switch {
            StatusBuffType.BasedOnCurrentHPPercent => new BasedOnCurrentHPPercent(type, duration, amount, icon),
            StatusBuffType.NormalMultiplicative => new NormalMultiplicative(type, duration, amount, icon),
            StatusBuffType.EffectedEnergy => new EffectedEnergy(type, duration, icon),
            StatusBuffType.NormalAddRate => new NormalAddRate(type, duration, amount, icon),
            _ => throw new ArgumentException("引数が不正です"),
        };
    }
}
