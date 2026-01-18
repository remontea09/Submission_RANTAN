using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver, IReadOnlySet<T> {
    [SerializeField] private List<T> _items = new();
    private HashSet<T> _set = new();

    public bool Add(T item) => _set.Add(item);
    public bool Remove(T item) => _set.Remove(item);
    public void Clear() => _set.Clear();

    public int Count => _set.Count;
    public bool Contains(T item) => _set.Contains(item);

    public void OnBeforeSerialize() => _items = new List<T>(_set);
    public void OnAfterDeserialize() => _set = new HashSet<T>(_items);

    public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
