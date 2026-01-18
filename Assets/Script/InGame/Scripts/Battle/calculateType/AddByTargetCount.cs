

namespace Damage.Caluculate {
    // 当たる敵の数が多いほどダメージUP
    public class AddByTargetCount : DamageCalculateBase {
        protected override int CalculateAddDamage(SkillContext attackContext, StageCharacter defender) {
            int addDamage = 10 * (attackContext.defenderList.Count - 1);
            return addDamage;
        }
    }
}
