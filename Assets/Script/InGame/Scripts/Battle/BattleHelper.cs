
using System;
using System.Collections.Generic;
using EnumUtilities;
using Utilities;

public static class BattleHelper {
    public static void Processkill(StageCharacter attacker, SkillData skill, Direction direction) {
        skill.ExecuteSkill(attacker, direction);
    }

    public static List<StageCharacter> GetTargetList(Index2D attackerPosition, ActionRange rangeType, Direction direction) {
        List<StageCharacter> targetList = new();
        int[,] range = ActionRangeTable.ActionRangeDictionary[rangeType];
        range = ArrayUtil.Skew45(range, direction);

        int height = range.GetLength(0);
        int width = range.GetLength(1);
        Index2D offset = new Index2D(height / 2, width / 2);
        for (int i = 0; i < height; ++i) {
            for (int j = 0; j < width; ++j) {
                if (range[i, j] == 1) {
                    var targetPosition = attackerPosition + new Index2D(i, j) - offset;
                    var target = StageService.Instance.GetStageCharacter(targetPosition);
                    if (target != null) {
                        targetList.Add(target);
                    }
                }
            }
        }
        return targetList;
    }

    public static void ApplyDamage(StageCharacter attacker, List<StageCharacter> defenderList, AttackSkillData skillData) {
        var calculator = DamageCalcRegistry.GetCalculator(skillData.DamageCalcType);
        SkillContext attackContext = new(attacker, defenderList, skillData);

        foreach (var defender in defenderList) {
            var damage = calculator.Calculate(attackContext, defender);
            defender.TakeDamage(damage);
        }
    }

    public static void ApplyStatusBuff(StageCharacter attacker, List<StageCharacter> defenderList, Func<CharacterStatusBuff> createBuffFunc) {
        foreach (var defender in defenderList) {
            defender.AddStatusBuff(createBuffFunc.Invoke());
        }
    }
}
