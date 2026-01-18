using System;

namespace Damage.Caluculate {
    // 相手の最大HPが小さいほど効果量UP
    public class IncreaseByTargetHp : DamageCalculateBase {
        public override int Calculate(SkillContext attackContext, StageCharacter defender) {
            var baseDamage = base.Calculate(attackContext, defender);

            float ratio = (float)attackContext.attacker.MaxHp / defender.MaxHp;
            var correctionFactor = Math.Clamp(ratio, 0.5f, 2f);

            int resultDamage = (int)(baseDamage * correctionFactor);
            return resultDamage;
        }
    }
}
