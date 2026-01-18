using EnumUtilities;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkillData", menuName = "ScriptableObjects/AttackSkillData")]
public class AttackSkillData : SkillData {
    [Header("attack専用")]
    [SerializeField] private DamageCalculateType damageCalcType;

    public DamageCalculateType DamageCalcType => damageCalcType;

    public override SkillType SkillType => SkillType.Attack;
    public override string Description => $"{Power}の威力で最大{ActionRangeTable.GetMaxHitCount(RangeType)}体に攻撃する。{DamageCalculateTextHelper.DescriptionTextDictionary[DamageCalcType]}";

    public override void ExecuteSkill(StageCharacter attacker, Direction direction) {
        var targetList = BattleHelper.GetTargetList(attacker.StagePosition, RangeType, direction);
        BattleHelper.ApplyDamage(attacker, targetList, this);
    }
}
