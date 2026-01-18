using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillButton : MonoBehaviour {
    [SerializeField] private Image _selectedImage;
    [Header("攻撃")]
    [SerializeField] private Button _attackSkillButton;
    [SerializeField] private TMP_Text _attackName;
    [SerializeField] private TMP_Text _attackRedEnergyCount;
    [SerializeField] private TMP_Text _attackGreenEnergyCount;
    [SerializeField] private TMP_Text _attackBlueEnergyCount;
    [Header("サポート")]
    [SerializeField] private Button _supportSkillButton;
    [SerializeField] private TMP_Text _supportName;
    [SerializeField] private TMP_Text _supportRedEnergyCount;
    [SerializeField] private TMP_Text _supportGreenEnergyCount;
    [SerializeField] private TMP_Text _supportBlueEnergyCount;

    public event Action<SkillType> OnClick;

    private void Start() {
        _attackSkillButton.onClick.AddListener(() => OnClick.Invoke(SkillType.Attack));
        _supportSkillButton.onClick.AddListener(() => OnClick.Invoke(SkillType.Support));
    }

    public void UpdateSkillData() {
        var attackSkill = GameSessionService.Instance.PlayerEnableAttackSkill;
        _attackName.text = attackSkill.Name;
        var requiredEnergy = attackSkill.RequiredEnergy;
        _attackRedEnergyCount.text = requiredEnergy.red.ToString();
        _attackGreenEnergyCount.text = requiredEnergy.green.ToString();
        _attackBlueEnergyCount.text = requiredEnergy.blue.ToString();

        var supportSkill = GameSessionService.Instance.PlayerEnableSupportSkill;
        if (supportSkill) {
            _supportName.text = supportSkill.Name;
            requiredEnergy = supportSkill.RequiredEnergy;
            _supportRedEnergyCount.text = requiredEnergy.red.ToString();
            _supportGreenEnergyCount.text = requiredEnergy.green.ToString();
            _supportBlueEnergyCount.text = requiredEnergy.blue.ToString();
        }
        else {
            _supportName.text = "-";
            _supportRedEnergyCount.text = "-";
            _supportGreenEnergyCount.text = "-";
            _supportBlueEnergyCount.text = "-";
        }
    }

    public void ChangePlayerSelected() {
        if (GameSessionService.Instance.PlayerType == PlayerType.Heroine) {
            _selectedImage.transform.SetParent(_supportSkillButton.transform, false);
        }
        else {
            _selectedImage.transform.SetParent(_attackSkillButton.transform, false);
        }
    }
}
