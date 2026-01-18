using System;
using Common;
using Common.Const;
using UnityEngine.SceneManagement;

public class GameSessionService : Base.Singleton<GameSessionService> {
    public int DungeonSeed => saveData.DungeonSeed;
    public int DungeonSize => saveData.DungeonSize;
    public int TurnCount { get; set; } = 1;
    public int EnemyCount => saveData.EnemyCount;
    public int EnemyBaseLevel => saveData.EnemyBaseLevel;
    public string DungeonDataId => saveData.DungeonDataId;
    public string HeroMainDataId => saveData.HeroMainDataId;
    public string HeroineMainDataId => saveData.HeroineMainDataId;
    public PlayerType PlayerType => saveData.PlayerType;
    public DungeonType CurrentDungeonType {
        get {
            return (CurrentFloorCount == DungeonConst.STAGE_PER_CYCLE) ? DungeonType.Boss : DungeonType.Normal;
        }
    }
    public int CurrentStageCount {
        get {
            return saveData.TotalFloorCount / DungeonConst.STAGE_PER_CYCLE + 1;
        }
    }
    public int CurrentFloorCount {
        get {
            return saveData.TotalFloorCount % DungeonConst.STAGE_PER_CYCLE + 1;
        }
    }
    public int TotalFloorCount => saveData.TotalFloorCount;
    public Energy Energy => saveData.Energy;

    public int PlayerXp => saveData.PlayerXp;
    public int PlayerLevel {
        get {
            return LevelUtil.GetLevelFromXp(PlayerXp);
        }
    }
    public int SkillPoint => saveData.SkillPoint;
    public SkillData PlayerEnableAttackSkill => Dao.Master.SkillMaster[saveData.EnableAttackSkillId];
    public SkillData PlayerEnableSupportSkill => Dao.Master.SkillMaster.TryGetValue(saveData.EnableSupportSkillId, out var skill) ? skill : null;

    public IReadOnlySet<string> UnlockedSkillIdSet => saveData.UnlockedSkillIdSet;
    public bool HasFlower { get; private set; } = false;

    public Achievement Achievement => saveData.Achievement;

    public event Action OnPlayerXpChanged;
    public event Action<SkillData> OnPlayerSkillUnlocked;
    public event Action OnPlayerSkillChanged;
    public event Action OnPlayerChanged;
    public event Action<Energy> OnPlayerEnergyChanged;
    public event Action OnClearWorld;
    public event Action OnAcquireFlower;

    private InGameSaveData saveData;

    public void Reset() {
        TurnCount = 0;
        saveData = null;
        HasFlower = false;
        OnPlayerChanged = null;
        OnPlayerEnergyChanged = null;
        OnClearWorld = null;
        OnAcquireFlower = null;
        OnPlayerSkillUnlocked = null;
        OnPlayerSkillChanged = null;
        OnPlayerXpChanged = null;
    }
    public void Load() {
        if (saveData == null) {
            this.saveData = SaveSystem.LoadInGame();
        }
    }

    public void LoadTutorial() {
        this.saveData = new InGameSaveData();
    }

    public void Save() {
        SaveSystem.SaveInGame(saveData);
        MetaSessionService.Instance.Save();
    }

    public void SaveAsync() {
        _ = SaveSystem.SaveInGameAsync(saveData);
    }

    public void AcquireRandomEnergy(int giv = 1) {
        EnergyType energyType = RandomHelper.GetRandomEnumValue<EnergyType>();
        AcquireEnergy(energyType, giv);
    }

    public void AcquireEnergy(EnergyType type, int giv) {
        switch (type) {
            case EnergyType.Red:
                saveData.Energy.red = Math.Min(PlayerConst.ENERGY_MAX_VALUE, Math.Max(PlayerConst.ENERGY_MIN_VALUE, saveData.Energy.red + giv));
                break;
            case EnergyType.Green:
                saveData.Energy.green = Math.Min(PlayerConst.ENERGY_MAX_VALUE, Math.Max(PlayerConst.ENERGY_MIN_VALUE, saveData.Energy.green + giv));
                break;
            case EnergyType.Blue:
                saveData.Energy.blue = Math.Min(PlayerConst.ENERGY_MAX_VALUE, Math.Max(PlayerConst.ENERGY_MIN_VALUE, saveData.Energy.blue + giv));
                break;
        }
        OnPlayerEnergyChanged?.Invoke(Energy);
    }

    public void UseEnergy(Energy useCount) {
        saveData.Energy -= useCount;
        OnPlayerEnergyChanged?.Invoke(Energy);
    }

    public void AcquireXp(int level) {
        var beforeLevel = LevelUtil.GetLevelFromXp(PlayerXp);
        saveData.PlayerXp += level;
        var afterLevel = LevelUtil.GetLevelFromXp(PlayerXp);
        if (beforeLevel != afterLevel) {
            saveData.SkillPoint += afterLevel - beforeLevel;
        }
        OnPlayerXpChanged?.Invoke();
    }

    public void UnlockSkill(SkillData skillData) {
        saveData.UnlockedSkillIdSet.Add(skillData.Id);
        saveData.SkillPoint -= skillData.UnlockCost;
        OnPlayerSkillUnlocked?.Invoke(skillData);
        OnPlayerXpChanged?.Invoke();
    }

    public void ChangePlayerSkill(SkillData skillData) {
        if (skillData.SkillType == SkillType.Attack) {
            saveData.EnableAttackSkillId = skillData.Id;
        }
        else {
            if (skillData.Id == saveData.EnableSupportSkillId) saveData.EnableSupportSkillId = string.Empty;
            else saveData.EnableSupportSkillId = skillData.Id;
        }

        OnPlayerSkillChanged?.Invoke();
    }

    public void ChangePlayer() {
        var newPlayerType = PlayerType == PlayerType.Hero ? PlayerType.Heroine : PlayerType.Hero;
        saveData.PlayerType = newPlayerType;
        Achievement.ChangeCount++;
        OnPlayerChanged?.Invoke();
    }

    public void AcquireFlower() {
        HasFlower = true;
        string logText = $"[{TurnCount}] <color=#FF6741>ランタンの花</color>を手に入れた";
        LogService.Instance.WriteLog(logText);
        OnAcquireFlower?.Invoke();
    }

    public void ClearDungeon() {
        if (TotalFloorCount + 1 >= DungeonConst.WORLD_PER_CYCLE * DungeonConst.STAGE_PER_CYCLE) {
            saveData.TotalFloorCount++;
            OnClearWorld?.Invoke();
            MetaSessionService.Instance.ClearWorld();
            MetaSessionService.Instance.Save();
            return;
        }

        TurnCount = 1;
        HasFlower = false;
        saveData.PlayerType = PlayerType.Hero;
        UpdateDungeon();
        StageService.Instance = null;
        EventService.Instance.RemoveAllListners();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateDungeon() {
        var preStageCount = CurrentStageCount;

        saveData.TotalFloorCount++;
        saveData.DungeonSize = DungeonConst.DUNGEON_DEFAULT_SIZE
            + ((CurrentStageCount - 1) * DungeonConst.DUNGEON_STAGE_INCREMENT)
            + ((CurrentFloorCount - 1) * DungeonConst.ENEMY_COUNT_FLOOR_INCREMENT);
        saveData.EnemyCount = DungeonConst.ENEMY_DEFAULT_COUNT
            + ((CurrentStageCount - 1) * DungeonConst.ENEMY_COUNT_STAGE_INCREMENT)
            + ((CurrentFloorCount - 1) * DungeonConst.ENEMY_COUNT_FLOOR_INCREMENT);
        saveData.EnemyBaseLevel += DungeonConst.ENEMY_DEFAULT_LEVEL
            + ((CurrentStageCount - 1) * DungeonConst.ENEMY_LEVEL_STAGE_INCREMENT)
            + ((CurrentFloorCount - 1) * DungeonConst.ENEMY_LEVEL_FLOOR_INCREMENT);

        MetaSessionService.Instance.ClearFloor();
        if (preStageCount != CurrentStageCount) {
            saveData.DungeonDataId = Dao.Master.DungeonMaster[DungeonDataId].NextDungeonData.Id;
            MetaSessionService.Instance.ClearStage();
        }


        Save();
    }
}
