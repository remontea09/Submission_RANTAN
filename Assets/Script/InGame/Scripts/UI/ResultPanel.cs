using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour {
    [SerializeField] private GameObject _clearGroup;
    [SerializeField] private GameObject _failGroup;
    [SerializeField] private TMP_Text _clearFloorConunt;
    [SerializeField] private TMP_Text _releaseSkillCount;
    [SerializeField] private TMP_Text _reachLevelCount;
    [SerializeField] private TMP_Text _changeCount;
    [SerializeField] private TMP_Text _totalTakeDamageCount;
    [SerializeField] private TMP_Text _totalHitDamageCount;
    [SerializeField] private TMP_Text _killedEnemyCount;
    [SerializeField] private TMP_Text _heroinText;
    [SerializeField] private Image _heroineImage;
    [SerializeField] private Sprite _heroineClear;
    [SerializeField] private Sprite _heroineLose;
    [SerializeField] private Button _homeButton;

    private void Awake() {
        gameObject.SetActive(false);
    }

    private void Start() {
        _homeButton.onClick.AddListener(() => SceneManager.LoadScene("home"));
    }

    public void InitResultPanel() {
        Dao.StageInstance.Player.OnDeathAction += OpenFailurePanel;
        GameSessionService.Instance.OnClearWorld += OpenClearPanel;
    }

    private void OnDestroy() {
        if (Dao.StageInstance.Player is not null) {
            Dao.StageInstance.Player.OnDeathAction -= OpenFailurePanel;
        }
        GameSessionService.Instance.OnClearWorld -= OpenClearPanel;
    }

    private void OpenClearPanel() => OpenResultPanel(true);
    private void OpenFailurePanel() => OpenResultPanel(false);

    private void OpenResultPanel(bool isClear) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        SetUpCount();

        _heroineImage.sprite = isClear ? _heroineClear : _heroineLose;
        _homeButton.image.color = isClear ? new Color32(127, 241, 146, 255) : new Color32(241, 127, 136, 255);
        _heroinText.text = isClear ? "いい冒険だった\r\nまた共に行こう" : "そんなときもある\r\nまた冒険しよう";
        _clearGroup.SetActive(isClear);
        _failGroup.SetActive(!isClear);
        if (isClear) {
            AudioService.Instance.PlayOnClearSE();
        }

        SaveSystem.DeleteInGameSaveData();
    }

    private void SetUpCount() {
        _clearFloorConunt.text = GameSessionService.Instance.TotalFloorCount.ToString();
        _releaseSkillCount.text = GameSessionService.Instance.UnlockedSkillIdSet.Count.ToString();
        _reachLevelCount.text = GameSessionService.Instance.PlayerLevel.ToString();
        _changeCount.text = GameSessionService.Instance.Achievement.ChangeCount.ToString();
        _totalTakeDamageCount.text = GameSessionService.Instance.Achievement.TotalTakeDamage.ToString();
        _totalHitDamageCount.text = GameSessionService.Instance.Achievement.TotalHitDamage.ToString();
        _killedEnemyCount.text = GameSessionService.Instance.Achievement.killedEnemyCount.ToString();
    }
}
