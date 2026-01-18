using System.Collections;
using Abu;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TutorialControllerBase : MonoBehaviour
{

    //各チュートリアルを実装するための基盤
    public abstract IEnumerator StartTutorial(TutorialFadeImage tutorialFadeImage, TutorialMessageWindow messageWindow);

    public IEnumerator WaitClickOrTouch() { //タッチかクリックを検知するまで待つ

        //長押しを検知
        while (Mouse.current != null && Mouse.current.leftButton.isPressed) {
            yield return null;
        }

        if (Touchscreen.current != null) {
            while (Touchscreen.current.primaryTouch.press.isPressed) {
                yield return null;
            }
        }

        //本体
        while (true) {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) {
                yield break;
            }

            if (Touchscreen.current != null &&
                Touchscreen.current.primaryTouch.press.wasPressedThisFrame) {
                yield break;
            }

            yield return null;
        }
    }
}
