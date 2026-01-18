using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/PortraitCatalog")]
public class PortraitCatalog : ScriptableObject {
    [Serializable] public class Item { public string id; public Sprite sprite; }
    [SerializeField] private List<Item> items = new();

    private Dictionary<string, Sprite> dict;

    public Sprite Get(string id) {
        if (string.IsNullOrEmpty(id)) return null;
        dict ??= Build();
        return dict.TryGetValue(id, out var sp) ? sp : null;
    }

    private Dictionary<string, Sprite> Build() {
        var d = new Dictionary<string, Sprite>();
        foreach (var it in items)
            if (!string.IsNullOrEmpty(it.id) && it.sprite != null)
                d[it.id] = it.sprite;
        return d;
    }
}
