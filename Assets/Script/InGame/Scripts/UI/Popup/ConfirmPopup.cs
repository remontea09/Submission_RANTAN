using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour {
    [SerializeField] private TMP_Text _bodyText;
    [SerializeField] private Button _okButton;
    [SerializeField] private TMP_Text _okText;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _cancelText;

    private Action<bool> _resultAction = null;

    private void Awake() {
        _okButton.onClick.AddListener(() => _resultAction?.Invoke(true));
        _okButton.onClick.AddListener(() => Destroy(gameObject));
        _cancelButton.onClick.AddListener(() => _resultAction?.Invoke(false));
        _cancelButton.onClick.AddListener(() => Destroy(gameObject));
    }

    public void InitConfirmPopup(string bodyText, string okText, string cancelText, Action<bool> resultAction) {
        _bodyText.text = bodyText;
        _okText.text = okText;
        _cancelText.text = cancelText;
        _resultAction = resultAction;
        _cancelButton.gameObject.SetActive(cancelText is not null);
    }
}
