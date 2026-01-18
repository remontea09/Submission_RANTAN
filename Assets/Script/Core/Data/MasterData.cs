using UnityEngine;

public abstract class MasterData : ScriptableObject, IMaster {
    [SerializeField] protected int id;
    public string Id => id.ToString("D6");
}
