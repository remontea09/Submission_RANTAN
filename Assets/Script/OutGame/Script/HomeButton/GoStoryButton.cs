using UnityEngine;
using UnityEngine.UI;

public class GoStoryButton : HomeButtonBase
{

    [SerializeField] private Button button;

    private void Awake() {
        button.onClick.AddListener(() => StartCoroutine(OnClickGoStory()));
    }
}
