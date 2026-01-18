using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : HomeButtonBase {
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject reStartPanel;

    public void Awake() {
        reStartPanel.SetActive(false);
        startButton.onClick.AddListener(FromTheBeginning);
    }

    private void FromTheBeginning() {
        bool save = SaveSystem.ExistsInGameSaveData();
        if (!save) {
            StartCoroutine(OnClickGameStart());
        }
        else {
            reStartPanel.SetActive(true);
        }
    }

}
