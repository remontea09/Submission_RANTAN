using UnityEngine;

public abstract class SkillData : MasterData {
    [Header("共通")]
    [SerializeField] private string skillName;
    [SerializeField] private int power;
    [SerializeField] private Sprite sprite;
    [Header("エネルギー")]
    [SerializeField] private int redEnergy;
    [SerializeField] private int greenEnergy;
    [SerializeField] private int blueEnergy;
    [Header("")]
    [SerializeField] private ActionRange range;
    [SerializeField] private RuntimeAnimatorController effect;
    [SerializeField] private ConstellationCategory category;
    [SerializeField] private int unlockCost;
    [SerializeField] private SkillData nextSkillData;

    public string Name => skillName;
    public int Power => power;
    public Sprite Sprite => sprite;
    public Energy RequiredEnergy => new Energy(redEnergy, greenEnergy, blueEnergy);
    public ActionRange RangeType => range;
    public ConstellationCategory Category => category;
    public RuntimeAnimatorController Effect => effect;
    public int Index => id % 100 - 1;
    public int UnlockCost => unlockCost;
    public SkillData NextSkillData => nextSkillData;
    public int Level => (int)(id / 100) % 10; // 最大レベル桁数1

    public abstract SkillType SkillType { get; }
    public abstract string Description { get; }
    public abstract void ExecuteSkill(StageCharacter attacker, Direction direction);
}

public enum SkillType : short {
    Attack = 1,
    Support = 2,
}
