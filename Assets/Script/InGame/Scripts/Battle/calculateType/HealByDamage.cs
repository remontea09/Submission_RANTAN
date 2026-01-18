
namespace Damage.Caluculate {
    public class HealByDamage : DamageCalculateBase {
        public override int Calculate(SkillContext attackContext, StageCharacter defender) {
            var baseDamage = base.Calculate(attackContext, defender);
            attackContext.attacker.HealHp((int)(baseDamage * 0.2f));
            return baseDamage;
        }
    }
}
