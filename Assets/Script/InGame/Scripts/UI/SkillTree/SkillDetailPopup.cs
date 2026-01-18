using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailPopup : MonoBehaviour {
    [SerializeField] private Image _frameImage;
    [SerializeField] private TMP_Text _skillNameText;
    [SerializeField] private TMP_Text _skillDescriptionText;
    [SerializeField] private Button _setButton;
    [SerializeField] private TMP_Text _setText;
    [Header("エネルギー")]
    [SerializeField] private TMP_Text _redEnergyText;
    [SerializeField] private TMP_Text _greenEnergyText;
    [SerializeField] private TMP_Text _blueEnergyText;
    [Header("レベルアップ")]
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private TMP_Text _levelUpText;
    [SerializeField] private SkillLevelUpPopup _levelupPopup;
    [Header("Unlock")]
    [SerializeField] private GameObject _unlockGroup;
    [SerializeField] private TMP_Text _unlockCostText;
    [SerializeField] private TMP_Text _skillPointText;
    [SerializeField] private Button _unlockButton;

    private SkillNode _skillNode;
    private SkillData _skillData => _skillNode.SkillData;

    private void Start() {
        _setButton.onClick.AddListener(SetSkill);
        _levelUpButton.onClick.AddListener(OpenLevelUpPopup);
        _unlockButton.onClick.AddListener(UnlockSkill);
    }

    public void InitSkillDetailPopup(SkillNode skillNode, bool isConnected) {
        _skillNode = skillNode;

        _frameImage.color = skillNode.NodeColor;

        UpdateLockState(isConnected);
        UpdatePopup();
    }

    private void UpdatePopup() {
        bool isUnlocked = GameSessionService.Instance.UnlockedSkillIdSet.Contains(_skillData.Id);
        _unlockGroup.SetActive(!isUnlocked);
        _setButton.gameObject.SetActive(isUnlocked);
        _levelUpButton.gameObject.SetActive(isUnlocked);

        _skillNameText.text = $"-{_skillData.Name}-";
        _skillDescriptionText.text = _skillData.Description;
        _unlockCostText.text = _skillData.UnlockCost.ToString();
        _skillPointText.text = GameSessionService.Instance.SkillPoint.ToString();

        var requiredEnergy = _skillData.RequiredEnergy;
        _redEnergyText.text = requiredEnergy.red.ToString();
        _greenEnergyText.text = requiredEnergy.green.ToString();
        _blueEnergyText.text = requiredEnergy.blue.ToString();

        UpdateSetState();
        UpdateLevelState();
    }

    private void UnlockSkill() {
        GameSessionService.Instance.UnlockSkill(_skillData);
        _skillNode.Unlock(true);
        _frameImage.color = _skillNode.NodeColor;
        UpdatePopup();
    }

    private void SetSkill() {
        GameSessionService.Instance.ChangePlayerSkill(_skillData);
        UpdatePopup();
    }

    private void UpdateLockState(bool isConnected) {
        bool isEnough = GameSessionService.Instance.SkillPoint >= _skillData.UnlockCost;
        bool isUnlockable = isConnected & isEnough;

        _unlockButton.interactable = isUnlockable;
        _skillPointText.color = isEnough ? Color.black : Color.red;
    }

    public void UpdateSetState() {
        bool isSet = _skillData == GameSessionService.Instance.PlayerEnableAttackSkill
            || _skillData == GameSessionService.Instance.PlayerEnableSupportSkill;
        bool isSupport = _skillData.SkillType == SkillType.Support;

        bool interactable = isSupport ? true : !isSet;
        Color color = isSupport ? (isSet ? new Color(0.82f, 0.82f, 0.82f) : Color.white) : Color.white;
        string text = isSupport ? (isSet ? "外す" : "セット") : (isSet ? "セット中" : "セット");

        _setButton.interactable = interactable;
        _setButton.image.color = color;
        _setText.text = text;
    }

    private void UpdateLevelState() {
        bool hasNextLevel = _skillData.NextSkillData is not null;
        _levelUpButton.interactable = hasNextLevel;
        _levelUpText.text = hasNextLevel ? "レベルアップ" : "最大レベル";
    }

    private void OpenLevelUpPopup() {
        Action<SkillData> callback = (skillData) => {
            _skillNode.InitSkillNode(skillData);
            UpdatePopup();
        };
        _levelupPopup.InitSkillLevelUpPopup(_skillData, callback);
    }
}
