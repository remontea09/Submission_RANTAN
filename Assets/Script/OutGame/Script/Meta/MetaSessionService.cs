using UnityEngine;

public class MetaSessionService : Base.SingletonMonoBehaviour<MetaSessionService> {
    private OutGameSaveData saveData;
    public void Load() {
        saveData = SaveSystem.LoadOutGame();
    }
    public void Save() {
        SaveSystem.SaveOutGame(saveData);
    }

    public void ClearFloor() => saveData.FloorClearCount++;
    public void ClearStage() => saveData.StageClearCount++;
    public void ClearWorld() => saveData.WorldClearCount++;
}
