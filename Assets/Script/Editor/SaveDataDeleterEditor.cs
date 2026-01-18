using System.IO;
using UnityEditor;
using UnityEngine;

public static class SaveDataDeleterEditor {
    [MenuItem("Tools/Save Data/Delete InGame Save File")]
    public static void DeleteSaveDataInGame() => SaveConfirm(SaveSystem.InGameSavePath);

    [MenuItem("Tools/Save Data/Delete OutGame Save File")]
    public static void DeleteSaveDataOutGame() => SaveConfirm(SaveSystem.OutGameSavePath);

    private static void SaveConfirm(string savePath) {
        if (!File.Exists(savePath)) {
            EditorUtility.DisplayDialog(
                "セーブデータ削除",
                "セーブデータが存在しません。\n\nパス:\n" + savePath,
                "OK"
            );
            return;
        }

        bool confirm = EditorUtility.DisplayDialog(
            "セーブデータ削除確認",
            "本当にセーブデータを削除しますか？\n\nパス:\n" + savePath,
            "はい",
            "キャンセル"
        );

        if (confirm) {
            try {
                FileUtil.DeleteFileOrDirectory(savePath);
                EditorUtility.DisplayDialog(
                    "削除完了",
                    "セーブデータを削除しました。\n\nパス:\n" + savePath,
                    "OK"
                );
                Debug.Log("[SaveDataDeleter] セーブデータを削除しました: " + savePath);
            }
            catch (System.Exception e) {
                EditorUtility.DisplayDialog(
                    "削除失敗",
                    "削除に失敗しました。\n" + e.Message,
                    "OK"
                );
                Debug.LogError("[SaveDataDeleter] 削除失敗: " + e);
            }
        }
    }
}
