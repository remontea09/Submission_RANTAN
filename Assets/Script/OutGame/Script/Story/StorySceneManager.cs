using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class StorySceneManager : MonoBehaviour
{

    [SerializeField] private RectTransform selects;
    [SerializeField] private GameObject backButton;
    [SerializeField] private Button goHomeButton;

    //tensi
    [SerializeField] private Button tenshiButton;
    [SerializeField] private RectTransform tenshiObjs;
    [SerializeField] private TenshiScenarioManager tenshiScenarioManager;

    private Vector2 defaultPos = new Vector2(0, 0);
    private float fadeSecond = 0.5f;

    private Tween inTween;
    private Tween outTween;

    private OutGameSaveData outGameSaveData;

    /// <summary>
    /// Storyシーンの初期化処理
    /// </summary>
    private void Awake() {
        AudioService.Instance.InitHomeAudio();
        outGameSaveData = SaveSystem.LoadOutGame();
        if(!outGameSaveData.opneStroy) StartPrologue();
        backButton.SetActive(false);
        tenshiObjs.gameObject.SetActive(false);
        tenshiScenarioManager.Back += BackSelects;
        tenshiButton.onClick.AddListener(() => SelectCharactor(tenshiObjs));
        goHomeButton.onClick.AddListener(() => SceneManager.LoadScene("Home"));
        
    }

    private void StartPrologue() {
        outGameSaveData.opneStroy = true;
        SaveSystem.SaveOutGame(outGameSaveData);
        StoryStorage.id = "0";
        StoryStorage.catalog = StoryStorage.GetScenarioCatalog(ScenarioType.prologue);
        SceneManager.LoadScene("RunStory");
    }

    private void SelectCharactor(RectTransform obj) {
        SlideOut(selects);
        SlideIn(obj);
        StartCoroutine(TrueBackButton());
    }

    private void BackSelects(RectTransform obj) {
        SlideOut(obj);
        SlideIn(selects);
        backButton.gameObject.SetActive(false);
    }

    private void SlideIn(RectTransform obj) {
        inTween?.Kill();
        obj.gameObject.SetActive(true);
        Vector2 nowPos = defaultPos;
        Vector2 movePos = nowPos + new Vector2(1100f,0f);
        obj.anchoredPosition = movePos;
        inTween = obj.DOAnchorPos(nowPos, fadeSecond);
    }

    private void SlideOut(RectTransform obj) {
        outTween?.Kill();
        Vector2 nowPos = obj.anchoredPosition;
        Vector2 movePos = nowPos + new Vector2(1100f, 0f);
        outTween = obj.DOAnchorPos(movePos, fadeSecond).OnComplete(() => obj.gameObject.SetActive(false));
    }

    private IEnumerator TrueBackButton() {
        yield return new WaitForSeconds(fadeSecond);
        backButton.gameObject.SetActive(true);
    }

}
