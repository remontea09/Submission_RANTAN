using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : HomeButtonBase {

    [SerializeField] private Button continueButton;

    public void Awake() {
        bool save = SaveSystem.ExistsInGameSaveData();
        if (save) {
            continueButton.enabled = true;
        }
        else {
            continueButton.enabled = false;
            continueButton.image.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        }
        continueButton.onClick.AddListener(() => StartCoroutine(OnClickGameStart()));
    }

}
