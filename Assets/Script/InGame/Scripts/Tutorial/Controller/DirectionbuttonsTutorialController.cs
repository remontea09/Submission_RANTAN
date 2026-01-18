using System.Collections;
using Abu;
using Common;
using Common.Const;
using UnityEngine;

public class DirectionbuttonsTutorialController : TutorialControllerBase {

    private TutorialHighlight tutorialHighlight;
    private TutorialHighlight directionCenterHighlight;

    private bool onMove = false;

    private void Start() {
        this.tutorialHighlight = GameObject.Find("Directionbuttons").AddComponent<TutorialHighlight>();
        this.directionCenterHighlight = this.tutorialHighlight.transform.Find("Center").gameObject.AddComponent<TutorialHighlight>();

        tutorialHighlight.enabled = false;
        directionCenterHighlight.enabled = false;
    }

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        messageWindow.SetText("移動について説明しよう");
        yield return WaitClickOrTouch();
        messageWindow.SetText("真ん中を押すと足踏み\n矢印で移動だ");
        yield return WaitClickOrTouch();
        tutorialHighlight.enabled = true;
        messageWindow.SetText("ボタンを押してみよう");
        while (!onMove) {
            yield return null;
        }
        yield return new WaitForSeconds(AnimationConst.MOVE_DURATION * 2); //移動終了から表示までバッファを設ける
        tutorialHighlight.enabled = false;
        messageWindow.SetText("上手くできたじゃないか");
        yield return WaitClickOrTouch();
        messageWindow.SetText("移動ボタンは攻撃にも使う\n攻撃範囲内にいる敵の方向に移動したら自動で攻撃されるぞ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("敵が範囲に入っていたら\n矢印が赤くなるからな");
        yield return WaitClickOrTouch();
        messageWindow.SetText("また、私が使うサポートスキルは");
        yield return WaitClickOrTouch();
        directionCenterHighlight.enabled = true;
        messageWindow.SetText("敵が近くにいたら真ん中が青く光って使用可能になる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("攻撃とサポートのバランスを考えて戦おう");
        yield return WaitClickOrTouch();
        directionCenterHighlight.enabled = false;
    }

    public void OnMove() {
        onMove = true;
    }
}
