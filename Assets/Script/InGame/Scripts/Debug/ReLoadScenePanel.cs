using UnityEngine;
using UnityEngine.UI;

public class ReLoadScenePanel : MonoBehaviour {
    [SerializeField] private Button button;
    [SerializeField] private GameObject ReLoadPopup;
    private void Awake() {
        button.onClick.AddListener(TogglePopup);
    }

    private void TogglePopup() {
        ReLoadPopup.SetActive(!ReLoadPopup.activeInHierarchy);
    }
}
