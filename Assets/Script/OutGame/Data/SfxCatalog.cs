using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/SfxCatalog")]
public class SfxCatalog : ScriptableObject {
    [Serializable] public class Item { public string id; public AudioClip clip; }
    [SerializeField] private List<Item> items = new();
    private Dictionary<string, AudioClip> dict;

    public AudioClip Get(string id) {
        if (string.IsNullOrEmpty(id)) return null;
        dict ??= Build();
        return dict.TryGetValue(id, out var c) ? c : null;
    }

    private Dictionary<string, AudioClip> Build() {
        var d = new Dictionary<string, AudioClip>();
        foreach (var it in items)
            if (!string.IsNullOrEmpty(it.id) && it.clip != null)
                d[it.id] = it.clip;
        return d;
    }
}
