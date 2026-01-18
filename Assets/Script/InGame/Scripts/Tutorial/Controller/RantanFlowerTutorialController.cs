using System.Collections;
using Abu;
using UnityEngine;
using DG.Tweening;

public class RantanFlowerTutorialController : TutorialControllerBase {

    public GameObject rantanFlower;


    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("さて、この冒険の目的について話そうか");
        yield return WaitClickOrTouch();
        messageWindow.SetText("ここでは最終的に\nランタンの花を手に入れることが目的となる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("ランタンの花ってのは、こういうやつだ");
        RantanPopUp();
        yield return WaitClickOrTouch();
        messageWindow.SetText("まあ、見つけたら分かるはずだ\n妙なオーラも漂ってるからな");
        yield return WaitClickOrTouch();
        RantanPopDown();
        messageWindow.SetText("ランタンの花を見つけたら、それをキャンプに持ち帰ろう");
        yield return WaitClickOrTouch();
        messageWindow.SetText("キャンプはマップに黄色で表示されてるから安心してくれ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("ランタンの花は遠くに生えているらしいから、\nくまなく探すようにな");
        yield return WaitClickOrTouch();
        messageWindow.SetText("噂では、稀に敵が持っていることもあるらしいが……");
        yield return WaitClickOrTouch();
    }

    public void RantanPopUp() {
        rantanFlower.SetActive(true);
        rantanFlower.transform.localScale = Vector3.zero;
        rantanFlower.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
    public void RantanPopDown() {
        rantanFlower.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() => rantanFlower.SetActive(false));
    }
}
