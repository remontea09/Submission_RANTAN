using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTutolialButton : HomeButtonBase
{

    [SerializeField] GameObject tutorialPanel;
    [SerializeField] private Button button;


    private void Awake() {
        button.onClick.AddListener(() => StartCoroutine(OnClickTutorialStart()));
    }
}
