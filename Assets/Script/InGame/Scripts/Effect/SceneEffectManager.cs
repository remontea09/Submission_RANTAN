using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEffectManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private CanvasGroup canvasGroup;

    /// <summary>
    /// 外部から呼ばれるInitializeメソッド
    /// </summary>
    public void InitSceneEffectManager() {
        this.gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        SceneChangeEffect();
    }


    /// <summary>
    /// チュートリアルステージであればフェードイン時のエフェクトを変更する
    /// </summary>
    private void SceneChangeEffect() {

        if(SceneManager.GetActiveScene().name == "Tutorial") {
            stageText.text = "チュートリアル";
            floorText.text = "ステージ";
        }
        else {
            stageText.text = "第" + GameSessionService.Instance.CurrentStageCount + "ステージ";
            if (GameSessionService.Instance.CurrentDungeonType == DungeonType.Boss) {
                floorText.text = "ボスエリア";
                floorText.rectTransform.DOShakePosition(2f, 20f);
            }
            else {
                floorText.text = GameSessionService.Instance.CurrentFloorCount + "フロア目";
            }
        }

        StartCoroutine(Fade());
    }

    /// <summary>
    /// フェードアニメーション実行部分
    /// </summary>
    private IEnumerator Fade() {
        yield return new WaitForSeconds(1.5f);
        canvasGroup.DOFade(0, 2f).OnComplete(() => {
            this.gameObject.SetActive(false);
        });
    }

}
