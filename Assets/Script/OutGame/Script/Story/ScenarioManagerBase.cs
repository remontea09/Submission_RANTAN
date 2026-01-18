using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ScenarioManagerBase : MonoBehaviour
{
    [SerializeField] private RectTransform rect;

    [SerializeField] public Button story1Button;
    [SerializeField] public Button story2Button;
    [SerializeField] public Button story3Button;
    [SerializeField] public Button story4Button;
    [SerializeField] public Button story5Button;
    [SerializeField] public Button storyEXButton;
    [SerializeField] public Button backButton;

    public event Action<RectTransform> Back;

    public OutGameSaveData outGameSaveData;

    private void Awake() {
        backButton.onClick.AddListener(() => Back?.Invoke(rect));
        this.gameObject.SetActive(false);
        outGameSaveData = SaveSystem.LoadOutGame();
        Initialize();
    }

    private void Initialize() {
        SetEnabledFalse();
        SetEnabled();
        SetOnClick();
    }

    /// <summary>
    /// 全てのボタンを非アクティブにする
    /// </summary>
    private void SetEnabledFalse() {
        story1Button.interactable = false;
        story2Button.interactable = false;
        story3Button.interactable = false;
        story4Button.interactable = false;
        story5Button.interactable = false;
        storyEXButton.interactable = false;
    }

    /// <summary>
    /// 好感度に応じてボタンを押せるようにする
    /// </summary>
    public abstract void SetEnabled();

    /// <summary>
    /// ボタンに対しての設定処理
    /// </summary>
    public abstract void SetOnClick();

    /// <summary>
    /// 実際のストーリーシーンに遷移する
    /// </summary>
    /// <param name="number">ストーリーの何話か</param>
    /// <param name="type">誰のシナリオか</param>
    public void GoRun(string number, ScenarioType type) {
        StoryStorage.id = "Story" + number;
        StoryStorage.catalog = StoryStorage.GetScenarioCatalog(type);
        SceneManager.LoadScene("RunStory");
    }
}
