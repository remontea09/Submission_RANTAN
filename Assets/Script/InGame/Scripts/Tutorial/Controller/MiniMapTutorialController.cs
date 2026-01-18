using System.Collections;
using Abu;
using UnityEngine;

public class MiniMapTutorialController : TutorialControllerBase {

    private TutorialHighlight tutorialHighlight;

    private void Start() {
        this.tutorialHighlight = GameObject.Find("miniMap").AddComponent<TutorialHighlight>();
        tutorialHighlight.enabled = false;
    }

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        tutorialHighlight.enabled = true;
        messageWindow.SetText("ここではミニマップを確認できる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("道に迷わぬよう、こまめに確認するといい");
        yield return WaitClickOrTouch();
        tutorialHighlight.enabled = false;
    }
}
