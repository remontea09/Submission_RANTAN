using System;

namespace Damage.Caluculate {
    public class IncreaceByOwnHp : DamageCalculateBase {
        public override int Calculate(SkillContext attackContext, StageCharacter defender) {
            var baseDamage = base.Calculate(attackContext, defender);

            float ratio = (float)attackContext.attacker.CurrentHp / attackContext.attacker.MaxHp;
            var correctionFactor = Math.Clamp(ratio, 0.5f, 2f);

            int resultDamage = (int)(baseDamage * correctionFactor);

            return baseDamage;
        }
    }
}
