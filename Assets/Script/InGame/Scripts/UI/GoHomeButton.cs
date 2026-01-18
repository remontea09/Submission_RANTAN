using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoHomeButton : MonoBehaviour {

    [SerializeField] private Button button;

    private void Awake() {
        button.onClick.AddListener(() => SceneManager.LoadScene("Home"));
    }

}
