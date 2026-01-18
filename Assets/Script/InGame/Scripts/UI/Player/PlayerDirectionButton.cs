using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerDirectionButton : Button, IPointerDownHandler, IPointerUpHandler {
    public event Action OnClickStay;

    private static PlayerDirectionButton holdingButton;

    private void Update() {
        if (holdingButton == this) {
            OnClickStay?.Invoke();
        }
    }

    public override void OnPointerDown(PointerEventData pointer) => BeginHold();

    public override void OnPointerUp(PointerEventData pointer) => EndHold();

    private void BeginHold() {
        if (!this.interactable || holdingButton) return;
        holdingButton = this;
        // this.DoStateTransition(SelectionState.Pressed, false);
        transform.DOScale(0.9f, 0.1f).SetLink(gameObject);
    }

    private void EndHold() {
        transform.DOScale(1f, 0.1f).SetLink(gameObject);
        if (holdingButton != this) return;
        holdingButton = null;
        this.DoStateTransition(SelectionState.Normal, false);
    }

    public void SetButton(DirectionInfo info) {
        switch (info) {
            case DirectionInfo.None: SetNoneButton(); break;
            case DirectionInfo.Attack: SetAttackButton(); break;
            case DirectionInfo.Support: SetSupportButton(); break;
            case DirectionInfo.Move: SetMoveButton(); break;
            case DirectionInfo.Camp: SetCampButton(); break;
        }
    }

    private void SetNoneButton() {
        image.color = Color.white;
        EndHold();
        this.interactable = false;
    }
    private void SetAttackButton() {
        this.interactable = true;
        image.color = Color.red;
    }
    private void SetSupportButton() {
        this.interactable = true;
        image.color = Color.cyan;
    }
    private void SetMoveButton() {
        this.interactable = true;
        image.color = Color.white;
    }
    private void SetCampButton() {
        this.interactable = true;
        image.color = Color.green;
    }
}
