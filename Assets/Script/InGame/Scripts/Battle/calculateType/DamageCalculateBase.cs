
namespace Damage.Caluculate {
    public abstract class DamageCalculateBase {
        public virtual int Calculate(SkillContext attackContext, StageCharacter defender) {
            var magicPower = attackContext.attacker.MagicPower;
            var skillPower = attackContext.skillData.Power;
            var defence = defender.Defense;

            var baseDamage = (magicPower * skillPower) / (magicPower + defence);

            var resultDamage = baseDamage + CalculateAddDamage(attackContext, defender);
            return resultDamage;
        }

        protected virtual int CalculateAddDamage(SkillContext attackContext, StageCharacter defender) {
            return 0;
        }
    }
}
