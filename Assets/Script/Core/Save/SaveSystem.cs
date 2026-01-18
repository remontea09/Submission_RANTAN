using System.IO;
using System.Threading.Tasks;
using UnityEngine;


public static class SaveSystem {
    public static readonly string InGameSavePath = Application.persistentDataPath + "/InGame/save.json";
    public static readonly string OutGameSavePath = Application.persistentDataPath + "/OutGame/save.json";

    public static void SaveInGame(InGameSaveData data) {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(InGameSavePath, json);
    }

    public static async Task SaveInGameAsync(InGameSaveData data) {
        string json = JsonUtility.ToJson(data);
        await File.WriteAllTextAsync(InGameSavePath, json);
    }

    public static InGameSaveData LoadInGame() {
        if (!File.Exists(InGameSavePath)) return InitInGameSaveData();
        return JsonUtility.FromJson<InGameSaveData>(File.ReadAllText(InGameSavePath));
    }

    private static InGameSaveData InitInGameSaveData() {
        if (!File.Exists(InGameSavePath)) {
            var dir = Path.GetDirectoryName(InGameSavePath);
            Directory.CreateDirectory(dir);
        }
        var newData = new InGameSaveData();
        File.WriteAllText(InGameSavePath, JsonUtility.ToJson(newData));

        return newData;
    }

    public static bool ExistsInGameSaveData() {
        return File.Exists(InGameSavePath);
    }

    public static void DeleteInGameSaveData() {
        File.Delete(InGameSavePath);
    }

    public static void SaveOutGame(OutGameSaveData data) {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(OutGameSavePath, json);
    }

    public static OutGameSaveData LoadOutGame() {
        if (!File.Exists(OutGameSavePath)) return InitOutGameSaveData();
        return JsonUtility.FromJson<OutGameSaveData>(File.ReadAllText(OutGameSavePath));
    }

    private static OutGameSaveData InitOutGameSaveData() {
        if (!File.Exists(OutGameSavePath)) {
            var dir = Path.GetDirectoryName(OutGameSavePath);
            Directory.CreateDirectory(dir);
        }
        var newData = new OutGameSaveData();
        File.WriteAllText(OutGameSavePath, JsonUtility.ToJson(newData));

        return newData;
    }
}
