using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/ScenarioCatalog")]
public class ScenarioCatalog : ScriptableObject {
    [Serializable]
    public class Item {
        public string id;
        public TextAsset json;
    }

    [SerializeField] private List<Item> items = new();

    private Dictionary<string, TextAsset> dict;

    public TextAsset Get(string id) {
        if (string.IsNullOrEmpty(id)) return null;
        dict ??= Build();
        return dict.TryGetValue(id, out var asset) ? asset : null;
    }

    private Dictionary<string, TextAsset> Build() {
        var d = new Dictionary<string, TextAsset>();
        foreach (var it in items) {
            if (string.IsNullOrEmpty(it.id) || it.json == null) continue;
            d[it.id] = it.json;
        }
        return d;
    }
}
