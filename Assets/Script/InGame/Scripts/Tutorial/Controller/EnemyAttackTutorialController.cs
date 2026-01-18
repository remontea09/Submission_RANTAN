using System.Collections;
using Abu;
using UnityEngine;

public class EnemyAttackTutorialController : TutorialControllerBase {

    private TutorialHighlight gameViewHighlight;
    private TutorialHighlight directionButtonHighlight;

    private bool onDeadEnemy = false;
    private const float dedAnimWait = 1.2f;

    private void Start() {
        this.gameViewHighlight = GameObject.Find("GameView").AddComponent<TutorialHighlight>();
        this.directionButtonHighlight = GameObject.Find("Directionbuttons").AddComponent<TutorialHighlight>();

        gameViewHighlight.enabled = false;
        directionButtonHighlight.enabled = false;
    }

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("次は敵を倒してみよう");
        yield return WaitClickOrTouch();
        messageWindow.SetText("敵には色々種類があってな\n殴る力が強かったり、素早かったり");
        yield return WaitClickOrTouch();
        messageWindow.SetText("まあ、戦ってみれば分かるさ\n適当な敵を倒してみたまえ");
        yield return WaitClickOrTouch();
        directionButtonHighlight.enabled = true;
        gameViewHighlight.enabled = true;
        messageWindow.PopDown();
        while(!onDeadEnemy) {
            yield return null;
        }
        yield return new WaitForSeconds(dedAnimWait);
        gameViewHighlight.enabled = false;
        directionButtonHighlight.enabled = false;
        messageWindow.PopUp();
        messageWindow.SetText("いいじゃないか、その調子だ\n君は腕がいいな");
        yield return WaitClickOrTouch();
    }


    public void OnDeadEnemy() {
        onDeadEnemy = true;
    }
}
