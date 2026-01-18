using System;
using UnityEngine;

public class PopupManager : MonoBehaviour {
    [SerializeField] private ConfirmPopup confirmPopup;

    public static Func<ConfirmPopup> CreateConfirmPopupFunc;
    private ConfirmPopup CreateConfirmPopup() => Instantiate(confirmPopup, transform);
    private void Awake() {
        CreateConfirmPopupFunc = CreateConfirmPopup;
    }
    private void OnDestroy() {
        CreateConfirmPopupFunc = null;
    }
}
