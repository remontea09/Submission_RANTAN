
using System.Collections.Generic;

public enum DamageCalculateType : short {
    Normal = 0,
    IncreaseByTargetHp = 100,
    AddByTargetCount = 200,
    IncreaseByOwnHp = 201,
    IncreaseByWall = 202,
    IncreaseBySkillCount = 203,
    HealByDamage = 204,
}

public static class DamageCalculateTextHelper {
    public readonly static IReadOnlyDictionary<DamageCalculateType, string> DescriptionTextDictionary = new Dictionary<DamageCalculateType, string>() {
        [DamageCalculateType.Normal] = string.Empty,
        [DamageCalculateType.IncreaseByTargetHp] = "\n敵の最大HPが多いほど効果量上昇",
        [DamageCalculateType.AddByTargetCount] = "\nヒットする敵の数が多いほど効果量上昇",
        [DamageCalculateType.IncreaseByOwnHp] = "\n自分のHPが低いほど効果量上昇",
        [DamageCalculateType.IncreaseByWall] = "\n周辺の障害物が多いほど効果量上昇",
        [DamageCalculateType.IncreaseBySkillCount] = "\n解放したスキルが多いほど効果量上昇",
        [DamageCalculateType.HealByDamage] = "\n与えたダメージの20%回復",
    };
}
