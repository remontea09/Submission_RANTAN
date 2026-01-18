using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GameViewScaler : MonoBehaviour {
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasScaler scaler;

    private float aspectThreshold = 9f / 18f;

    private void Update() {
        UpdateAnchor();
    }

    private void UpdateAnchor() {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect <= aspectThreshold) {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaler.referenceResolution.y * currentAspect);
        }
    }
}
