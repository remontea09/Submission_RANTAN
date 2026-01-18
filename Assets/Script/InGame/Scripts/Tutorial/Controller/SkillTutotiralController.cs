using System.Collections;
using Abu;
using UnityEngine;
using UnityEngine.UI;

public class SkillTutotiralController : TutorialControllerBase {
    
    private TutorialHighlight tutorialHighlight;
    private TutorialHighlight backButtonHighlight;

    private Button skillTreeCloseButton;
    private Button playerSkillAttackButton;
    private Button playerSkillSupportButton;

    private bool clickSkillButton = false;
    private bool clickBackButton = false;

    private void Start() {
        var skillTree = FindFirstObjectByType<SkillTreePanel>(FindObjectsInactive.Include);
        this.tutorialHighlight = FindFirstObjectByType<PlayerSkillButton>().gameObject.AddComponent<TutorialHighlight>();
        this.skillTreeCloseButton = skillTree.transform.Find("CloseButton").GetComponent<Button>();
        this.backButtonHighlight = this.skillTreeCloseButton.gameObject.AddComponent<TutorialHighlight>();
        var playerSkillButton = FindFirstObjectByType<PlayerSkillButton>();
        this.playerSkillAttackButton = playerSkillButton.transform.Find("Mask/AttackButton").GetComponent<Button>();
        this.playerSkillSupportButton = playerSkillButton.transform.Find("Mask/SupportButton").GetComponent<Button>();

        tutorialHighlight.enabled = false;
        backButtonHighlight.enabled = false;
        skillTreeCloseButton.onClick.AddListener(() => ClickCloseButton());
        playerSkillAttackButton.onClick.AddListener(() => ClickSkillButton());
        playerSkillSupportButton.onClick.AddListener(() => ClickSkillButton());
    }

    public override IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow) {
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("スキルの説明をしようか");
        yield return WaitClickOrTouch();
        messageWindow.SetText("スキルには多様な種類があり、内容もそれぞれ異なる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("威力、攻撃範囲、\n消費するエネルギーの量……");
        yield return WaitClickOrTouch();
        messageWindow.SetText("レベルアップをすれば、それらは向上していく");
        yield return WaitClickOrTouch();
        tutorialHighlight.enabled = true;
        messageWindow.SetText("ここをクリックすれば\nスキルツリーが開けるぞ");
        yield return WaitClickOrTouch();
        while (!clickSkillButton) {
            yield return null;
        }
        tutorialHighlight.enabled = false;
        messageWindow.SetText("いいじゃないか");
        yield return WaitClickOrTouch();
        messageWindow.SetText("ここでは様々なスキルを解放し、さらにそのスキルをレベルアップすることができる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("解放するもよし\nレベルアップするもよし\n自由に選択するんだぞ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("解放したスキルはセットしないと使えないから、気をつけてくれ");
        yield return WaitClickOrTouch();
        messageWindow.SetText("各アイコンをタップすると詳細が見れる");
        yield return WaitClickOrTouch();
        messageWindow.SetText("解放、セット。この順序を忘れないようにな");
        yield return WaitClickOrTouch();
        messageWindow.PopDown();
        tutorialFadeImage.enabled = false;
        while(!clickBackButton) {
            yield return null;
        }
        messageWindow.PopUp();
        tutorialFadeImage.enabled = true;
        messageWindow.SetText("どうだったかな？");
        yield return WaitClickOrTouch();
        messageWindow.SetText("これからはスキルを上手く使って、ダンジョンを攻略していこう");
        yield return WaitClickOrTouch();

    }

    public void ClickSkillButton() {
        clickSkillButton = true;
        playerSkillAttackButton.onClick.RemoveListener(ClickSkillButton);
        playerSkillSupportButton.onClick.RemoveListener(ClickSkillButton);
    }

    public void ClickCloseButton() {
        clickBackButton = true;
        skillTreeCloseButton.onClick.RemoveListener(ClickCloseButton);
    }


}
