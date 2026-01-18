using TMPro;
using UnityEngine;
using DG.Tweening;

public class TutorialMessageWindow : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    private GameObject me;
    public float popSecond { get; private set; }

    private void Awake() {
        popSecond = 0.3f;
        me = this.gameObject;
        me.SetActive(false);
    }

    public void SetText(string target) {
        text.text = target;
    }

    public void PopUp() {
        me.SetActive(true);
        me.transform.localScale = Vector3.zero;
        me.transform.DOScale(1f, popSecond).SetEase(Ease.OutBack);
    }
    public void PopDown() {
        me.transform.DOScale(0f, popSecond).SetEase(Ease.InBack).OnComplete(() => me.SetActive(false));
    }

}
