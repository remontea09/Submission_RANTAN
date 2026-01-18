using System.Collections;
using Abu;
using UnityEngine;

public class EndTutorialController : TutorialControllerBase {
    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("これで説明はおしまいだ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("また分からなくなったらいつでも聞きに来てくれ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("なにせ、私はお前の相棒だからな");
        yield return WaitClickOrTouch();
        messageWindow.SetText("じゃあ、またダンジョンで");
        yield return WaitClickOrTouch();
    }
}
