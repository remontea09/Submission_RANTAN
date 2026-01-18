using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour {
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _homeButton;
    [SerializeField] private Image _playerImage;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerMaxHp;
    [SerializeField] private TMP_Text _playerCurrentHp;
    [SerializeField] private TMP_Text _playerMagicPower;
    [SerializeField] private TMP_Text _playerDefence;
    [SerializeField] private TMP_Text _playerSpeed;
    [SerializeField] private TMP_Text _stageCount;
    [SerializeField] private TMP_Text _floorCount;

    private void Awake() {
        _openButton.onClick.AddListener(Open);
        gameObject.SetActive(false);
    }

    private void Start() {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _homeButton.onClick.AddListener(() => SceneManager.LoadScene("home"));
    }

    private void Open() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
            return;
        }
        SetUp();
    }

    private void SetUp() {
        var player = Dao.StageInstance.Player;
        var playerMasterId = GameSessionService.Instance.PlayerType == PlayerType.Hero
            ? GameSessionService.Instance.HeroMainDataId
            : GameSessionService.Instance.HeroineMainDataId;
        var playerMaster = Dao.Master.PlayerMainMaster[playerMasterId];
        _playerImage.sprite = playerMaster.PlayerSprite;
        _playerName.text = playerMaster.Name;
        _playerMaxHp.text = player.MaxHp.ToString();
        _playerCurrentHp.text = player.CurrentHp.ToString();
        _playerMagicPower.text = player.MagicPower.ToString();
        _playerDefence.text = player.Defense.ToString();
        _playerSpeed.text = player.Speed.ToString();
        _stageCount.text = $"第{GameSessionService.Instance.CurrentStageCount.ToString()}ステージ";
        _floorCount.text = "フロア " + GameSessionService.Instance.CurrentFloorCount.ToString();

        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }
}
