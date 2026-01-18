using System.Collections;
using Abu;
using UnityEngine;

public class LogTutorialController : TutorialControllerBase {

    private TutorialHighlight tutorialHighlight;

    private void Start() {
        this.tutorialHighlight = GameObject.Find("LogService").AddComponent<TutorialHighlight>();
        tutorialHighlight.enabled = false;
    }

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        tutorialHighlight.enabled = true;
        messageWindow.SetText("これはログといって、\n攻撃やそのダメージなどの行動が記録される");
        yield return WaitClickOrTouch();
        tutorialFadeImage.enabled = false;
        tutorialHighlight.enabled = false;
    }
}
