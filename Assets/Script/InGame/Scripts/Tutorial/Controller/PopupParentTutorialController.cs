using Abu;
using UnityEngine;

public class PopupParentTutorialController : MonoBehaviour
{

    [SerializeField] private TutorialHighlight tutorialHighlight;

    private void Awake() {
        tutorialHighlight.enabled = false;
    }

    private void Update() {
        if (transform.childCount > 0) {
            tutorialHighlight.enabled = true;
        }
        else {
            tutorialHighlight.enabled = false;
        }
    }

}
