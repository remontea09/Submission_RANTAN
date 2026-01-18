using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour {

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Button turorialButton;
    [SerializeField] private Button tutorialBackButton;

    private void Awake() {
        tutorialPanel.SetActive(false);
        turorialButton.onClick.AddListener(() => OnTutorial());
        tutorialBackButton.onClick.AddListener(() => Back());
    }

    private void OnTutorial() {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
        tutorialPanel.transform.SetAsLastSibling();
    }

    private void Back() {
        tutorialPanel.SetActive(false);
    }

}
