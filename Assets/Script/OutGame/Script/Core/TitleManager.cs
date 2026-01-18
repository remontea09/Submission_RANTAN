using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup blackBack;
    [SerializeField] private CanvasGroup titles;
    [SerializeField] private CanvasGroup needTapText;

    private Tween tween;
    private bool isClick = false;

    private void Awake() {
        button.onClick.AddListener(() => StartCoroutine(ChangeHomeScene()));
        button.gameObject.SetActive(false);
        StartCoroutine(TitleDirection());
    }

    /// <summary>
    /// タイトル表示時の演出
    /// </summary>
    private IEnumerator TitleDirection() {
        titles.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        blackBack.DOFade(0f, 1f);
        AudioService.Instance.InitTitleAudio();
        yield return new WaitForSeconds(1f);
        button.gameObject.SetActive(true);
        tween = needTapText.DOFade(0f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }


    /// <summary>
    /// ホームシーン遷移時の演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeHomeScene() {
        if (!isClick) {
            isClick = true;
            AudioService.Instance.PlaySE(AudioType.TitleClickSE);
            titles.DOFade(0f, 0.5f);
            blackBack.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Home");
        }
    }

    private void OnDisable() {
        tween?.Kill();
    }
}
