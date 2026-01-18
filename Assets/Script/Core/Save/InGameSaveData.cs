using Common;
using Common.Const;

[System.Serializable]
public class InGameSaveData {
    public int DungeonSeed = RandomHelper.GenerateNewSeed();
    public int DungeonSize = DungeonConst.DUNGEON_DEFAULT_SIZE;
    public int EnemyCount = DungeonConst.ENEMY_DEFAULT_COUNT;
    public int EnemyBaseLevel = DungeonConst.ENEMY_DEFAULT_LEVEL;
    public string DungeonDataId = "000001";
    public string HeroMainDataId = "000001";
    public string HeroineMainDataId = "000002";
    public PlayerType PlayerType = PlayerType.Hero;
    public int TotalFloorCount = 0;
    public Energy Energy = new Energy(PlayerConst.ENERGY_DEFAULT_VALUE);
    public int PlayerXp = 0;
    public SerializableHashSet<string> UnlockedSkillIdSet = new SerializableHashSet<string> { "000101" };
    public string EnableAttackSkillId = "000101"; //選択中のキャラの初期技
    public string EnableSupportSkillId = string.Empty;
    public int SkillPoint = 0;
    public Achievement Achievement = new();
}

[System.Serializable]
public class Achievement {
    public int ChangeCount = 0;
    public int TotalTakeDamage = 0;
    public int TotalHitDamage = 0;
    public int killedEnemyCount = 0;
}
