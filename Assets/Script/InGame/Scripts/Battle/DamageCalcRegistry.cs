using System.Collections.Generic;
using Damage.Caluculate;

public static class DamageCalcRegistry {
    private static readonly Dictionary<DamageCalculateType, DamageCalculateBase> damageCalcDict = new() {
        { DamageCalculateType.Normal, new Normal() },
        { DamageCalculateType.IncreaseByTargetHp, new IncreaseByTargetHp() },
        { DamageCalculateType.AddByTargetCount, new AddByTargetCount() },
        { DamageCalculateType.IncreaseByOwnHp, new IncreaceByOwnHp() },
        { DamageCalculateType.IncreaseByWall, new IncreaceByWall() },
        { DamageCalculateType.IncreaseBySkillCount, new IncreaceBySkillCount() },
        { DamageCalculateType.HealByDamage, new HealByDamage() },
    };

    public static DamageCalculateBase GetCalculator(DamageCalculateType damageCalculateType) {
        return damageCalcDict[damageCalculateType];
    }
}
