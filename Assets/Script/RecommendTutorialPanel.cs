using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RecommendTutorialPanel : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    public float popSecond { get; private set; }

    private void Awake() {
        popSecond = 0.3f;
        closeButton.onClick.AddListener(() => PopDown());
    }

    public void PopUp() {
        gameObject.SetActive(true);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(1f, popSecond).SetEase(Ease.OutBack);
    }
    public void PopDown() {
        gameObject.transform.DOScale(0f, popSecond).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }

}
