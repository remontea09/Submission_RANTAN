using System;
using UnityEngine;

public abstract class CharacterBuff {
    public int? DurationTurns { get; protected set; }
    public abstract string Description { get; }
    public Sprite Icon { get; private set; }

    public event Action<CharacterBuff> OnEndBuff;

    protected CharacterBuff(int? duration, Sprite icon) {
        DurationTurns = duration;
        Icon = icon;

        if (DurationTurns.HasValue) {
            EventService.Instance.AddListner(EventType.TurnEnd, DecrementDuration);
        }
    }

    public void AddTurn(int turn) {
        if (DurationTurns.HasValue) {
            DurationTurns += turn;
        }
    }

    private void DecrementDuration() {
        DurationTurns--;
        if (DurationTurns <= 0) {
            EventService.Instance.RemoveListner(EventType.TurnEnd, DecrementDuration);
            OnEndBuff?.Invoke(this);
        }
    }
}
