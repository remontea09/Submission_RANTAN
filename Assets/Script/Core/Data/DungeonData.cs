using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DungeonData", menuName = "ScriptableObjects/DungeonData")]
public class DungeonData : MasterData {
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Sprite flowerSprite;
    [SerializeField] private AudioClip bgm;
    [SerializeField] private DungeonData nextDungeonData;
    [SerializeField] private EnemyMainData[] enemyMainDataArray;

    public TileBase FloorTile => floorTile;
    public TileBase WallTile => wallTile;
    public Sprite FlowerSprite => flowerSprite;
    public AudioClip Bgm => bgm;
    public DungeonData NextDungeonData => nextDungeonData;
    public EnemyMainData[] EnemyMainDataArray => enemyMainDataArray;
}
