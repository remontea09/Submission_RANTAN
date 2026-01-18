using System.Collections;
using Abu;
using UnityEngine;

public class MiasmaTutorialController : TutorialControllerBase {
    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("さて。綺麗な草原だが、長居するわけにはいかない");
        yield return WaitClickOrTouch();
        messageWindow.SetText("瘴気。そう言われているナニカが、次第に迫ってくるからだ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("瘴気に飲まれると継続的にダメージを受けてしまう");
        yield return WaitClickOrTouch();
        messageWindow.SetText("冒険もいいが、あまりはしゃぎすぎないようにな");
        yield return WaitClickOrTouch();
    }
}
