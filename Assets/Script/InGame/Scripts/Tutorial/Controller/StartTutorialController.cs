using System.Collections;
using Abu;
using UnityEngine;

public class StartTutorialController : TutorialControllerBase {

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        messageWindow.SetText("やあ。冒険のやり方を教わりに来たんだね？");
        yield return WaitClickOrTouch();
        messageWindow.SetText("いいだろう。\nといっても、簡単なことだがな");
        yield return WaitClickOrTouch();
    }

}
