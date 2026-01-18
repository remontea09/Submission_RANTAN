using UnityEngine;

public class FirstLaunchChecker : MonoBehaviour
{

    [SerializeField] private RecommendTutorialPanel recommendTutorialPanel;

    private const string FirstLaunchKey = "HasLaunchedOnce";
    public static bool IsFirstLaunch { get; private set; }

    /// <summary>
    /// ゲームが初めての起動だったらレコメンドパネルを表示する
    /// </summary>
    private void Awake() {
        if (!PlayerPrefs.HasKey(FirstLaunchKey)) {
            IsFirstLaunch = true;

            PlayerPrefs.SetInt(FirstLaunchKey, 1);
            PlayerPrefs.Save();

            recommendTutorialPanel.PopUp();
        }
        else {
            IsFirstLaunch = false;
        }
    }
}
