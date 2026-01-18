using System.Collections;
using Abu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    //Controllers
    [SerializeField] private TutorialFadeImage tutorialFadeImagePrefab;
    [SerializeField] private TutorialMessageWindow tutorialMessageWindowPrefab;
    [SerializeField] private StartTutorialController startTutorialController;
    [SerializeField] private DirectionbuttonsTutorialController directionTutorialController;
    [SerializeField] private EnemyAttackTutorialController enemyAttackTutorialController;
    [SerializeField] private SkillTutotiralController skillTutotiralController;
    [SerializeField] private LevelTutorialController levelTutorialController;
    [SerializeField] private LogTutorialController logTutorialController;
    [SerializeField] private EnergyTutorialController energyTutorialController;
    [SerializeField] private MiniMapTutorialController miniMapTutorialController;
    [SerializeField] private RantanFlowerTutorialController rantanFlowerTutorialController;
    [SerializeField] private MiasmaTutorialController miasmaTutorialController;
    [SerializeField] private EndTutorialController endTutorialController;

    [SerializeField] private GameObject ExitTutorialObjPrefab;
    [SerializeField] private GameObject rantanFlowerPrefab;

    private TutorialFadeImage tutorialFadeImage;
    private TutorialMessageWindow tutorialMessageWindow;
    private GameObject ExitTutorialObj;

    public void InitTutorialManager() {
        var UIParent = FindFirstObjectByType<PlayerUI>().transform;
        this.tutorialFadeImage = Instantiate(tutorialFadeImagePrefab, UIParent);
        this.tutorialMessageWindow = Instantiate(tutorialMessageWindowPrefab, UIParent);
        this.rantanFlowerTutorialController.rantanFlower = Instantiate(rantanFlowerPrefab, UIParent);
        this.rantanFlowerTutorialController.rantanFlower.SetActive(false);

        this.ExitTutorialObj = Instantiate(ExitTutorialObjPrefab, UIParent.Find("Panels"));
        ExitTutorialObj.SetActive(false);
        tutorialFadeImage.enabled = false;
        Dao.StageInstance.Player.OnMove += ChangeDirecOnMove;

        //作られたEnemyを取得して各EnemyのEnemyControllerにチュートリアルの発火イベントを購読
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].OnDeathAction += ChangeOnDeadEnemy;
        }

        StartCoroutine(Tutorials());
    }

    private IEnumerator Tutorials() {
        yield return new WaitForSeconds(3f); //シーンのFadeが終わる丁度いい秒数
        tutorialMessageWindow.PopUp();
        tutorialFadeImage.enabled = true;

        //各チュートリアルをスタート
        yield return StartCoroutine(startTutorialController.StartTutorial(tutorialFadeImage,tutorialMessageWindow));
        yield return StartCoroutine(miniMapTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(directionTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(enemyAttackTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(logTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(levelTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(skillTutotiralController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(energyTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(rantanFlowerTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(miasmaTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        yield return StartCoroutine(endTutorialController.StartTutorial(tutorialFadeImage, tutorialMessageWindow));
        
        //チュートリアル終了後の処理
        tutorialMessageWindow.PopDown();
        tutorialFadeImage.enabled = false;
        StartCoroutine(ExitTutorial());
    }

    private IEnumerator ExitTutorial() {
        yield return new WaitForSeconds(tutorialMessageWindow.popSecond);
        ExitTutorialObj.SetActive(true);
    }

    //移動チュートリアルの発火イベント
    private void ChangeDirecOnMove() {
        directionTutorialController.OnMove();
    }

    //敵を倒すチュートリアルの発火イベント
    private void ChangeOnDeadEnemy() {
        enemyAttackTutorialController.OnDeadEnemy();
    }

    private void OnDestroy() {
        if (Dao.StageInstance.Player is not null) {
            Dao.StageInstance.Player.OnMove -= ChangeDirecOnMove;
        }
    }
}
