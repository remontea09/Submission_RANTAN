using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour {
    [SerializeField] private Image _iconImage;
    [SerializeField] private Toggle _toggle;

    public void InitBuffIcon(CharacterBuff buff, ToggleGroup toggleGroup, BuffDetail buffDetail) {
        _iconImage.sprite = buff.Icon;
        _toggle.group = toggleGroup;
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener((isOn) => {
            if (isOn) {
                buffDetail.OpenBuffDetail(buff.Description, buff.DurationTurns);
            }
            else {
                buffDetail.CloseBuffDetail();
            }
        });
    }

    public void UnSelect() {
        if (_toggle.isOn) {
            _toggle.isOn = false;
        }
    }
}
