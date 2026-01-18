using Abu;
using UnityEngine;

public class HighlightEnableController : MonoBehaviour
{

    [SerializeField] private TutorialHighlight tutorialHighlight;

    private void Awake() {
        tutorialHighlight.enabled = false;
    }

    private void OnEnable() {
        tutorialHighlight.enabled = true;
    }

    private void OnDisable() {
        tutorialHighlight.enabled = false;
    }

}
