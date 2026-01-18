using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Dao {
    public static class Master {
        public static IReadOnlyDictionary<string, SkillData> SkillMaster { get; private set; }
        public static IReadOnlyDictionary<string, DungeonData> DungeonMaster { get; private set; }
        public static IReadOnlyDictionary<string, PlayerMainData> PlayerMainMaster { get; private set; }

        public static void InitMaster() {
            if (SkillMaster != null) {
                Debug.LogWarning("マスターは既にロード済みです");
                return;
            }

            Dictionary<string, SkillData> tmpSkillMaster = new Dictionary<string, SkillData>();

            var attackSkillDataArray = Resources.LoadAll<AttackSkillData>("SkillData/AttackSkillData");
            foreach (var skillData in attackSkillDataArray) tmpSkillMaster[skillData.Id] = skillData;
            var statusBuffSkillDataArray = Resources.LoadAll<StatusBuffSkillData>("SkillData/SupportSkillData/StatusBuffSkillData");
            foreach (var skillData in statusBuffSkillDataArray) tmpSkillMaster[skillData.Id] = skillData;
            var onTurnEndBuffSkillDataArray = Resources.LoadAll<OnTurnEndBuffSkillData>("SkillData/SupportSkillData/OnTurnEndBuffSkillData");
            foreach (var skillData in onTurnEndBuffSkillDataArray) tmpSkillMaster[skillData.Id] = skillData;
            var onHitBuffSkillDataArray = Resources.LoadAll<OnHitBuffSkillData>("SkillData/SupportSkillData/OnHitBuffSkillData");
            foreach (var skillData in onHitBuffSkillDataArray) tmpSkillMaster[skillData.Id] = skillData;
            var instantSkillDataArray = Resources.LoadAll<InstantSkillData>("SkillData/SupportSkillData/InstantSkillData");
            foreach (var skillData in instantSkillDataArray) tmpSkillMaster[skillData.Id] = skillData;

            Master.SkillMaster = tmpSkillMaster;

            var dungeonDataArray = Resources.LoadAll<DungeonData>("DungeonData");
            Master.DungeonMaster = dungeonDataArray.ToDictionary(dungeonData => dungeonData.Id);
            var playerMainDataArray = Resources.LoadAll<PlayerMainData>("PlayerMainData");
            Master.PlayerMainMaster = playerMainDataArray.ToDictionary(playerMainData => playerMainData.Id);
        }
    }

    public static class GameSettings {
        public static GameType GameType;
    }

    public static class StageInstance {
        public static IReadOnlyPlayer Player;
        public static IExecutablePlayer ExecutablePlayer;
    }
}
