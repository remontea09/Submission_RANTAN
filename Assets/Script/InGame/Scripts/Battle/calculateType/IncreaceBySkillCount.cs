
namespace Damage.Caluculate {
    public class IncreaceBySkillCount : DamageCalculateBase {
        public override int Calculate(SkillContext attackContext, StageCharacter defender) {
            float baseDamage = base.Calculate(attackContext, defender);
            for (var i = 0; i < GameSessionService.Instance.UnlockedSkillIdSet.Count; i++) {
                baseDamage *= 1.1f;
            }
            return (int)baseDamage;
        }
    }
}
