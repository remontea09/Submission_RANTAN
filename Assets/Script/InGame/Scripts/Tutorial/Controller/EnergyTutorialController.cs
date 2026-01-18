using System.Collections;
using Abu;
using UnityEngine;

public class EnergyTutorialController : TutorialControllerBase {

    private TutorialHighlight tutorialHighlight;

    private void Start() {
        this.tutorialHighlight = FindFirstObjectByType<EnergyUI>().gameObject.AddComponent<TutorialHighlight>();
        tutorialHighlight.enabled = false;
    }


    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialHighlight.enabled = true;
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("この宝石のようなものがエネルギーだ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("エネルギーはスキルを使うと消費される");
        yield return WaitClickOrTouch();
        messageWindow.SetText("さらに、これは所持数によってステータスが変化するんだ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("赤なら魔力\n緑なら防御力\n青なら素早さ\nが連動している");
        yield return WaitClickOrTouch();
        messageWindow.SetText("所持数に気をつけて戦うといい");
        yield return WaitClickOrTouch();
        tutorialHighlight.enabled = false;
    }
}
