using UnityEngine;

[CreateAssetMenu(fileName = "ConstellationData", menuName = "ScriptableObjects/ConstellationData")]
public class ConstellationData : ScriptableObject {
    [SerializeField] private ConstellationCategory constellationCategory;
    public Vector2[] nodePositions;
    public Vector2Int[] edges;

    public ConstellationCategory Category => constellationCategory;
}
