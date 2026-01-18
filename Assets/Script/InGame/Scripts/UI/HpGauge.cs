using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour {
    [SerializeField] private Image backImage;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image maskImage;

    public void SetFill(float amount) {
        amount = Mathf.Clamp01(amount);
        frontImage.fillAmount = amount;

        float hue = Mathf.Lerp(0f, 0.33f, amount);
        frontImage.color = Color.HSVToRGB(hue, 1, 1);
        backImage.color = Color.HSVToRGB(hue, 1, 0.4f);
    }

    public void PlayChangeAnimation(bool isActive, float duration) {
        RectTransform rectTransform = transform as RectTransform;
        int padding = isActive ? 0 : 10;
        DOTween.To(() => rectTransform.offsetMin, v => rectTransform.offsetMin = v,
            new Vector2(padding, rectTransform.offsetMin.y), duration);

        DOTween.To(() => rectTransform.offsetMax, v => rectTransform.offsetMax = v,
            new Vector2(-padding, rectTransform.offsetMax.y), duration);

        float alpha = isActive ? 0 : 0.75f;
        maskImage.DOFade(alpha, duration);
    }
}
