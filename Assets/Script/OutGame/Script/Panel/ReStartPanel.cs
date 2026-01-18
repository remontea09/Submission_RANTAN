using UnityEngine;
using UnityEngine.UI;

public class ReStartPanel : HomeButtonBase {

    [SerializeField] private Button reStarButton;
    [SerializeField] private Button backButton;

    private void Awake() {
        reStarButton.onClick.AddListener(() => OnClickReStart());
        backButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }

    private void OnClickReStart() {
        SaveSystem.DeleteInGameSaveData();
        StartCoroutine(OnClickGameStart());
    }

}
