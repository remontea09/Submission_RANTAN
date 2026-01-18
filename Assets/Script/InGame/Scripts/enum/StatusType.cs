using System.Collections.Generic;

public enum StatusType : short {
    MaxHp = 1,
    MagicPower = 2,
    Defense = 3,
    Speed = 4
}

public static class StatusTypeTexts {
    public readonly static IReadOnlyDictionary<StatusType, string> StatusNameDictionary = new Dictionary<StatusType, string>() {
        [StatusType.MagicPower] = "魔力",
        [StatusType.Defense] = "防御力",
        [StatusType.Speed] = "素早さ",
    };
}
