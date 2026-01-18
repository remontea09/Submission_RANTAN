
namespace Damage.Caluculate {
    public class IncreaceByWall : DamageCalculateBase {
        public override int Calculate(SkillContext attackContext, StageCharacter defender) {
            float baseDamage = base.Calculate(attackContext, defender);
            var attackerPos = attackContext.attacker.StagePosition;
            Index2D offset = new Index2D(2, 2);
            for (int i = 0; i < 5; ++i) {
                for (int j = 0; j < 5; ++j) {
                    Index2D pos = new Index2D(i, j) - offset + attackerPos;
                    if (StageService.Instance.GetTile(pos) == TileType.Wall) {
                        baseDamage += 1.2f;
                    }
                }
            }
            return (int)baseDamage;
        }
    }
}
