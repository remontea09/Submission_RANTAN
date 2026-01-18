using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonBase : MonoBehaviour {

    [SerializeField] private CanvasGroup blackBack;
    private bool isClick = false;

    public IEnumerator OnClickGameStart() {
        if (!isClick) {
            isClick = true;
            AudioService.Instance.PlaySE(AudioType.HomeClickSE);
            blackBack.gameObject.SetActive(true);
            blackBack.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Dao.GameSettings.GameType = GameType.Normal;
            SceneManager.LoadScene("Main");
        }
    }

    public IEnumerator OnClickTutorialStart() {
        if (!isClick) {
            isClick = true;
            AudioService.Instance.PlaySE(AudioType.HomeClickSE);
            blackBack.gameObject.SetActive(true);
            blackBack.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            Dao.GameSettings.GameType = GameType.Tutorial;
            SceneManager.LoadScene("Main");
        }
    }

    public IEnumerator OnClickGoStory() {
        isClick = true;
        AudioService.Instance.PlaySE(AudioType.HomeClickSE);
        blackBack.gameObject.SetActive(true);
        blackBack.DOFade(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Story");
    }
}
