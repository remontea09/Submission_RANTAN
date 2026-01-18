using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugInput : MonoBehaviour {
    [SerializeField] private PlayerDirectionButton centerButton;
    [SerializeField] private PlayerDirectionButton upButton;
    [SerializeField] private PlayerDirectionButton rightUpButton;
    [SerializeField] private PlayerDirectionButton rightButton;
    [SerializeField] private PlayerDirectionButton rightDownButton;
    [SerializeField] private PlayerDirectionButton downButton;
    [SerializeField] private PlayerDirectionButton leftDownButton;
    [SerializeField] private PlayerDirectionButton leftButton;
    [SerializeField] private PlayerDirectionButton leftUpButton;

    private Dictionary<PlayerDirectionButton, List<Key>> keyInput;

    private void Awake() {
        keyInput = new() {
            { centerButton, new List<Key> {Key.Space, Key.Numpad5 } },
            { upButton, new List<Key> { Key.UpArrow, Key.Numpad8 } },
            { rightButton, new List<Key> { Key.RightArrow, Key.Numpad6 } },
            { downButton, new List<Key> { Key.DownArrow, Key.Numpad2 } },
            { leftButton, new List<Key> { Key.LeftArrow, Key.Numpad4 } },
            { rightUpButton, new List<Key> { Key.W, Key.Numpad9 } },
            { rightDownButton, new List<Key> { Key.S, Key.Numpad3 } },
            { leftUpButton, new List<Key> { Key.Q, Key.Numpad7 } },
            { leftDownButton, new List<Key> { Key.A, Key.Numpad1 } },
        };
    }

    private void Update() {
        var kb = Keyboard.current;

        foreach (var button in keyInput.Keys) {
            bool pressed = false;
            bool released = false;
            foreach (var inputKey in keyInput[button]) {
                if (kb[inputKey].wasPressedThisFrame) {
                    pressed = true;
                }
                if (kb[inputKey].wasReleasedThisFrame) {
                    released = true;
                }
            }
            if (pressed) {
                ExecuteEvents.Execute<IPointerDownHandler>(
                    button.gameObject,
                    new PointerEventData(EventSystem.current),
                    (handler, eventData) => handler.OnPointerDown((PointerEventData)eventData)
                );
            }
            if (released) {
                ExecuteEvents.Execute<IPointerUpHandler>(
                    button.gameObject,
                    new PointerEventData(EventSystem.current),
                    (handler, eventData) => handler.OnPointerUp((PointerEventData)eventData)
                );
            }
        }
    }
}
