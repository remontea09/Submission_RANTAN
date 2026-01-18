using System;
using System.Collections.Generic;
using System.Linq;
using Common.Const;
using DG.Tweening;
using EnumUtilities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    [SerializeField] private PlayerBuffIconManager _buffIcons;
    [SerializeField] private EnergyUI _energyUI;
    [SerializeField] private PlayerLevelUI _playerLevelUI;
    [Header("スキルボタン")]
    [SerializeField] private PlayerSkillButton _playerSkillButton;
    [SerializeField] private SkillTreePanel skillTreePanel;
    [Header("方向ボタン")]
    [SerializeField] private PlayerDirectionButton[] directionButtons;
    [Header("hpゲージ")]
    [SerializeField] private HpGauge _activeHpGauge;
    [SerializeField] private HpGauge _passiveHpGauge;
    [Header("パネル")]
    [SerializeField] private ResultPanel _resultPanel;
    [Header("背景")]
    [SerializeField] private Image _backGroundImage;

    private static readonly Color HERO_COLOR = new Color32(0xEC, 0xB4, 0xB4, 0xFF);
    private static readonly Color HEROINE_COLOR = new Color32(0xB4, 0xE4, 0xEC, 0xFF);

    private bool _hasConfirmPopup = false;
    public Dictionary<Direction, DirectionInfo> _directionInfos { get; private set; } = new();

    public Action<Direction> OnClickDirection;

    private void Awake() {
        skillTreePanel.gameObject.SetActive(false);
    }

    public void InitPlayerUI() {
        _playerSkillButton.OnClick += OnClickPlayerSkillButton;

        for (int i = 0; i < Direction.AllDirections.Length; i++) {
            Direction direction = Direction.AllDirections[i];
            directionButtons[i].OnClickStay += (() => OnClickDirectionButton(direction));
        }

        GameSessionService.Instance.OnPlayerChanged += SwitchUI;
        EventService.Instance.AddListner(EventType.TurnEnd, UpdateDirectionInfos);
        Dao.StageInstance.Player.OnChangeHp += UpdateHp;
        Dao.StageInstance.Player.OnChangePassiveHp += UpdatePassiveHp;
        GameSessionService.Instance.OnPlayerSkillChanged += UpdateSkill;
        UpdateSkill();
        SetBackGroundColor(0);

        _buffIcons.InitPlayerBuffIcons();
        _energyUI.InitEnergyUI();
        _resultPanel.InitResultPanel();
        _playerLevelUI.InitPlyaerLevelUI();
    }

    private void OnDestroy() {
        EventService.Instance.RemoveListner(EventType.TurnEnd, UpdateDirectionInfos);
        GameSessionService.Instance.OnPlayerSkillChanged -= UpdateSkill;
        if (Dao.StageInstance.Player is not null) {
            Dao.StageInstance.Player.OnChangeHp -= UpdateHp;
            Dao.StageInstance.Player.OnChangePassiveHp -= UpdatePassiveHp;
        }
    }

    private void OnClickPlayerSkillButton(SkillType type) {
        skillTreePanel.gameObject.SetActive(true);
        skillTreePanel.OpenSkillTreeTab(type);
    }
    private void OnClickDirectionButton(Direction direction) {
        if (AnimationService.Instance.IsPlaying) return;

        var player = Dao.StageInstance.ExecutablePlayer;
        switch (_directionInfos[direction]) {
            case DirectionInfo.Camp:
                OpenNextDungeonConfirmPopup();
                break;
            case DirectionInfo.Attack:
            case DirectionInfo.Support:
                player.ExecutePlayerSkill(direction);
                break;
            case DirectionInfo.Move:
                player.ExecutePlayerMove(direction);
                break;
        }
    }

    private void OpenNextDungeonConfirmPopup() {
        if (_hasConfirmPopup) return;

        bool hasKey = GameSessionService.Instance.HasFlower;
        Action<bool> resultAction = (isOk) => {
            if (isOk && hasKey) {
                Invoke(nameof(NextStage), 0f);
            }
            _hasConfirmPopup = false;
        };
        string bodyText = hasKey ? "次のフロアに進みますか?" : "ランタンの花を持っていません";
        string okText = "はい";
        string cancelText = hasKey ? "いいえ" : null;

        ConfirmPopup popup = PopupManager.CreateConfirmPopupFunc.Invoke();
        popup.InitConfirmPopup(bodyText, okText, cancelText, resultAction);
        _hasConfirmPopup = true;
    }

    private void NextStage() {
        GameSessionService.Instance.ClearDungeon();
    }

    private void SetBackGroundColor(float duration) {
        Color end = GameSessionService.Instance.PlayerType == PlayerType.Hero ? HERO_COLOR : HEROINE_COLOR;
        _backGroundImage.DOColor(end, duration);
    }

    private void SwitchUI() {
        _playerSkillButton.ChangePlayerSelected();
        var duration = AnimationConst.CHANGE_PLAYER_DURATION;
        (_activeHpGauge, _passiveHpGauge) = (_passiveHpGauge, _activeHpGauge);
        _activeHpGauge.PlayChangeAnimation(true, duration);
        _passiveHpGauge.PlayChangeAnimation(false, duration);
        _activeHpGauge.transform.SetAsLastSibling();
        SetBackGroundColor(duration);
    }

    private void UpdateSkill() {
        _playerSkillButton.UpdateSkillData();
        UpdateDirectionInfos();
    }
    private void UpdateHp(int maxHp, int currentHp) {
        _activeHpGauge.SetFill((float)currentHp / maxHp);
    }
    private void UpdatePassiveHp(int maxHp, int currentHp) {
        _passiveHpGauge.SetFill((float)currentHp / maxHp);
    }

    private void UpdateDirectionInfos() {
        _directionInfos.Clear();
        _directionInfos[Direction.None] = DirectionInfo.Move;

        var player = Dao.StageInstance.Player;
        var supportSkill = GameSessionService.Instance.PlayerEnableSupportSkill;
        if (supportSkill is not null && supportSkill.RangeType == ActionRange.Mine) {
            var sideLength = PlayerConst.SUPPORT_AREA_SIDE;
            Index2D offset = new Index2D(sideLength / 2, sideLength / 2);
            for (int i = 0; i < sideLength; ++i) {
                for (int j = 0; j < sideLength; ++j) {
                    var pos = player.StagePosition + new Index2D(i, j) - offset;
                    if (StageService.Instance.GetStageCharacter(pos)?.EntityType == player.TargetType) {
                        _directionInfos[Direction.None] = DirectionInfo.Support;
                        i = sideLength;
                        break;
                    }
                }
            }
        }

        var searchRange = ActionRangeTable.SearchRangeDictionary[GameSessionService.Instance.PlayerEnableAttackSkill.RangeType];
        foreach (var direction in Direction.EightDirections) {
            var info = DirectionInfo.None;

            if (StageService.Instance.CanMove(player.StagePosition, direction)) {
                info = DirectionInfo.Move;
            }

            var targetList = BattleHelper.GetTargetList(player.StagePosition, searchRange, direction);
            if (targetList.Any(character => character.EntityType == player.TargetType)) {
                info = DirectionInfo.Attack;
            }

            if (StageService.Instance.IsCamp(player.StagePosition + direction)) {
                if (GameSessionService.Instance.HasFlower) {
                    info = DirectionInfo.Camp;
                }
                else {
                    info = DirectionInfo.None;
                }
            }

            _directionInfos[direction] = info;
        }

        for (int i = 0; i < Direction.AllDirections.Length; i++) {
            Direction direction = Direction.AllDirections[i];
            directionButtons[i].SetButton(_directionInfos[direction]);
        }
    }
}
