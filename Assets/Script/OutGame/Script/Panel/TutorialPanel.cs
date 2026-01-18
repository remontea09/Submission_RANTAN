using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : HomeButtonBase {
    [SerializeField] private Button startTutorialButton;
    [SerializeField] private Button backButton;

    private void Awake() {
        startTutorialButton.onClick.AddListener(() => StartCoroutine(OnClickTutorialStart()));
        backButton.onClick.AddListener(() => this.gameObject.SetActive(false));
    }
}
