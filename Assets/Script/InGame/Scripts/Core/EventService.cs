using System;
using System.Collections.Generic;

public class EventService : Base.Singleton<EventService> {
    private Dictionary<EventType, List<Action>> _eventDictionary = new Dictionary<EventType, List<Action>>() {
        [EventType.TurnStart] = new List<Action>(),
        [EventType.TurnEnd] = new List<Action>(),
        [EventType.TurnEndBuff] = new List<Action>(),
    };
    public void AddListner(EventType type, Action action) {
        _eventDictionary[type].Add(action);
    }

    public void RemoveListner(EventType type, Action action) {
        _eventDictionary[type].Remove(action);
    }

    public void RemoveAllListners() {
        foreach (var list in _eventDictionary.Values) {
            list.Clear();
        }
    }

    public void DispatchEvent(EventType type) {
        for (int idx = _eventDictionary[type].Count - 1; idx >= 0; --idx) {
            var action = _eventDictionary[type][idx];
            action.Invoke();
        }
    }
}

public enum EventType : short {
    TurnStart,
    TurnEnd,
    TurnEndBuff,
}
