using System.Collections;
using Abu;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class LevelTutorialController : TutorialControllerBase {

    private TutorialHighlight levelAndSkillPointHighlight;
    private TutorialHighlight expGaugeHighlight;

    private void Start() {
        this.levelAndSkillPointHighlight = GameObject.Find("GameView").transform.Find("Level/LevelBack").gameObject.AddComponent<TutorialHighlight>();
        this.expGaugeHighlight = GameObject.Find("ExpBack").AddComponent<TutorialHighlight>();

        levelAndSkillPointHighlight.enabled = false;
        expGaugeHighlight.enabled = false;
    }


    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        levelAndSkillPointHighlight.enabled = true;
        messageWindow.SetText("レベルの話をしようか");
        yield return WaitClickOrTouch();
        messageWindow.SetText("レベルは敵を倒して経験値を得ると上がっていく");
        yield return WaitClickOrTouch();
        messageWindow.SetText("レベルが上がると\nステータスが向上する");
        yield return WaitClickOrTouch();
        messageWindow.SetText("また、レベルが上がるとスキルポイントを入手できる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("スキルポイントを使えば\nスキルを解放できたり\nスキルのレベルアップを行えるぞ");
        yield return WaitClickOrTouch();
        levelAndSkillPointHighlight.enabled = false;
        expGaugeHighlight.enabled = true;
        messageWindow.SetText("現在持っている経験値はこのバーで確認できる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("今どんな状況にあるのか、それを把握する参考にしてくれ");
        yield return WaitClickOrTouch();
        expGaugeHighlight.enabled = false;
    }
}
