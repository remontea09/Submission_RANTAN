using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelUpPopup : MonoBehaviour {
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _skillPointText;
    [SerializeField] private TMP_Text _levelupCostText;
    [Header("Before")]
    [SerializeField] private SkillIcon _beforeSkillIcon;
    [SerializeField] private TMP_Text _beforeSkillName;
    [SerializeField] private TMP_Text _beforeDescription;
    [SerializeField] private TMP_Text _beforeLevel;
    [SerializeField] private TMP_Text _beforeRedEnergy;
    [SerializeField] private TMP_Text _beforeGreenEnergy;
    [SerializeField] private TMP_Text _beforeBlueEnergy;
    [Header("After")]
    [SerializeField] private SkillIcon _afterSkillIcon;
    [SerializeField] private TMP_Text _afterSkillName;
    [SerializeField] private TMP_Text _afterDescription;
    [SerializeField] private TMP_Text _afterLevel;
    [SerializeField] private TMP_Text _afterRedEnergy;
    [SerializeField] private TMP_Text _afterGreenEnergy;
    [SerializeField] private TMP_Text _afterBlueEnergy;

    private SkillData _beforeSkillData;
    private Action<SkillData> _levelUpCallback;

    private void Start() {
        _levelUpButton.onClick.AddListener(ConfirmLevelUp);
        _cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }

    public void InitSkillLevelUpPopup(SkillData skillData, Action<SkillData> callback) {
        if (skillData.NextSkillData is null) return;
        _beforeSkillData = skillData;
        _levelUpCallback = callback;

        UpdateBeforeSkill();
        UpdateAfterSkill();

        gameObject.SetActive(true);
    }

    private void Close() {
        _beforeSkillData = null;
        _levelUpCallback = null;
        gameObject.SetActive(false);
    }

    private void UpdateBeforeSkill() {
        _beforeSkillIcon.InitSkillIcon(_beforeSkillData);
        _beforeSkillName.text = _beforeSkillData.Name;
        _beforeDescription.text = _beforeSkillData.Description;
        _beforeLevel.text = _beforeSkillData.Level.ToString();
        var energy = _beforeSkillData.RequiredEnergy;
        _beforeRedEnergy.text = energy.red.ToString();
        _beforeGreenEnergy.text = energy.green.ToString();
        _beforeBlueEnergy.text = energy.blue.ToString();
    }

    private void UpdateAfterSkill() {
        var nextSkill = _beforeSkillData.NextSkillData;
        _afterSkillIcon.InitSkillIcon(nextSkill);
        _afterSkillName.text = nextSkill.Name;
        _afterDescription.text = HighLightDiff(_beforeSkillData.Description, nextSkill.Description);
        _afterLevel.text = nextSkill.Level.ToString();
        var beforeEnergy = _beforeSkillData.RequiredEnergy;
        var afterEnergy = nextSkill.RequiredEnergy;
        _afterRedEnergy.text = HighLightDiff(beforeEnergy.red.ToString(), afterEnergy.red.ToString());
        _afterGreenEnergy.text = HighLightDiff(beforeEnergy.green.ToString(), afterEnergy.green.ToString());
        _afterBlueEnergy.text = HighLightDiff(beforeEnergy.blue.ToString(), afterEnergy.blue.ToString());
        _levelupCostText.text = nextSkill.UnlockCost.ToString();

        bool isEnough = GameSessionService.Instance.SkillPoint >= nextSkill.UnlockCost;
        _levelUpButton.interactable = isEnough;
        _skillPointText.color = isEnough ? Color.white : Color.red;
        _skillPointText.text = GameSessionService.Instance.SkillPoint.ToString();
    }

    private static string HighLightDiff(string before, string after) {
        string startTag = "<color=#FF703D>";
        string endTag = "</color>";

        var beforeNumbersInfo = Regex.Matches(before, @"\d+");
        var afterNumbersInfo = Regex.Matches(after, @"\d+");
        Queue<int> startIndexQueue = new();
        Queue<int> endIndexQueue = new();
        for (int i = 0; i < beforeNumbersInfo.Count; i++) {
            var beforeInfo = beforeNumbersInfo[i];
            var afterInfo = afterNumbersInfo[i];
            if (beforeInfo.Value != afterInfo.Value) {
                var start = afterInfo.Index;
                startIndexQueue.Enqueue(start);
                var end = afterInfo.Index + afterInfo.Length;
                endIndexQueue.Enqueue(end);
            }
        }

        StringBuilder highLightedText = new();
        for (int idx = 0; idx < after.Length; idx++) {
            if (startIndexQueue.Count > 0 && idx == startIndexQueue.Peek()) {
                startIndexQueue.Dequeue();
                highLightedText.Append(startTag);
            }
            if (endIndexQueue.Count > 0 && idx == endIndexQueue.Peek()) {
                endIndexQueue.Dequeue();
                highLightedText.Append(endTag);
            }
            highLightedText.Append(after[idx]);
        }
        return highLightedText.ToString();
    }

    private void ConfirmLevelUp() {
        var newSkill = _beforeSkillData.NextSkillData;
        GameSessionService.Instance.UnlockSkill(newSkill);
        var compareSkill = newSkill.SkillType == SkillType.Attack
            ? GameSessionService.Instance.PlayerEnableAttackSkill
            : GameSessionService.Instance.PlayerEnableSupportSkill;
        if (_beforeSkillData == compareSkill) {
            GameSessionService.Instance.ChangePlayerSkill(newSkill);
        }
        _levelUpCallback?.Invoke(newSkill);
        Close();
    }
}
