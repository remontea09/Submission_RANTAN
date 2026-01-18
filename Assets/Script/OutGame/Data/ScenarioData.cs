using System;
using UnityEngine;

[Serializable]
public class ScenarioData {
    public ScenarioEntry[] entries;
}

[Serializable]
public class ScenarioEntry {
    public string name;
    public string text;
    public string portrait;
    public string bg;
    public string sfx;
}
