using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{

    [SerializeField] private Button helpButton;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private Button helpBackButton;

    private void Awake() {
        helpPanel.SetActive(false);
        helpButton.onClick.AddListener(() => helpPanel.SetActive(true));
        helpBackButton.onClick.AddListener(() => helpPanel.SetActive(false));
    }

}
