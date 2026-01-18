using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour {

    [SerializeField] private Button tolkButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private GameObject nameWindow;
    [SerializeField] private GameObject mainWindow;

    private Talks talks;
    private int episode = 0;
    private int readingEpisode = 0;
    private bool isTalking = false; //話の途中か
    private bool isTalked = false; //一度話をしたか

    private OutGameSaveData saveData;

    public void Awake() {
        nameWindow.SetActive(false);
        mainWindow.SetActive(false);
        talks = new Talks();
        talks.InitTalks(nameText, mainText);
        saveData = SaveSystem.LoadOutGame();
        talks.ReadEnd += Afterreading;
        tolkButton.onClick.AddListener(() => Talk());
    }

    /// <summary>
    /// 会話開始
    /// </summary>
    private void Talk() {
        if (isTalking) {
            return;
        }

        isTalking = true;
        PopUp(nameWindow);
        PopUp(mainWindow);
        if (!isTalked) {
            episode = FlagSearch();
        }
        StoryCaseSearch(episode);
    }

    /// <summary>
    /// セーブデータから読んでいないストーリーのフラグを探して返す
    /// </summary>
    /// <returns>ストーリー</returns>
    private int FlagSearch() {
        for (int i = 0; i < saveData.flags.Length; i++) {
            if (saveData.flags[i] == false) {
                isTalked = true;
                return i;
            }
        }

        return 7; //読了後
    }

    /// <summary>
    /// 受け取ったフラグに応じて話を開始する
    /// </summary>
    /// <param name="episode">フラグ</param>
    private void StoryCaseSearch(int episode) {
        switch (episode) {
            case 0: StartCoroutine(talks.StartStory()); break;
            case 1: StartCoroutine(talks.Story1()); break;
            case 2: StartCoroutine(talks.Story2()); break;
            case 3: StartCoroutine(talks.Story3()); break;
            case 4: StartCoroutine(talks.Story4()); break;
            case 5: StartCoroutine(talks.Story5()); break;
            case 6: StartCoroutine(talks.Story6()); break;
            case 7: StartCoroutine(talks.OverStory7()); break; //蛇足(OverStory)はクリア回数によって変えたい
        }

        readingEpisode = episode;
    }

    private void SaveReadedEpisode(int episode) {
        if (episode < 0 || episode > 6) {
            return;
        }
        saveData.flags[episode] = true;
        SaveSystem.SaveOutGame(saveData);
    }

    private void PopUp(GameObject window) {
        window.SetActive(true);
        window.transform.localScale = Vector3.zero;
        window.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 読了後の処理
    /// </summary>
    private void Afterreading() {
        PopDown(nameWindow);
        PopDown(mainWindow);
        SaveReadedEpisode(readingEpisode);
        isTalking = false;
    }

    private void PopDown(GameObject window) {
        window.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => window.SetActive(false));
    }
}
