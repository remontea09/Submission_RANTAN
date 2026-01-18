using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Debug_ReLoad : MonoBehaviour {
    [SerializeField] private Button reStartButton;

    private void Start() {
        if (reStartButton) reStartButton.onClick.AddListener(ReLoadInGame);
    }

    private void Update() {
        if (Keyboard.current.digit0Key.wasPressedThisFrame) ReLoadInGame();
    }

    private void ReLoadInGame() {
        if (!reStartButton) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
