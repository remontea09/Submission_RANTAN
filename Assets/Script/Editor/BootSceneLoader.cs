using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class BootSceneLoader {
    static BootSceneLoader() {
        EditorApplication.playModeStateChanged += ChangeBootScene;
    }

    static void ChangeBootScene(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            var bootScene = EditorBuildSettings.scenes[0];
            EditorSceneManager.OpenScene(bootScene.path);
        }
    }
}
