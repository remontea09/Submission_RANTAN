using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMainData", menuName = "ScriptableObjects/EnemyMainData")]
public class EnemyMainData : ScriptableObject, IStatus {
    [SerializeField] private Sprite sprite;
    [SerializeField] private string enemyName;
    [SerializeField] private int defaultHp;
    [SerializeField] private int defaultMagicPower;
    [SerializeField] private int defaultDefense;
    [SerializeField] private int defaultSpeed;
    [SerializeField] private SkillData skillData;
    [SerializeField] private int viewSide;

    public string Name => enemyName;
    public Sprite Sprite => sprite;
    public int MaxHp => defaultHp;
    public int MagicPower => defaultMagicPower;
    public int Defense => defaultDefense;
    public int Speed => defaultSpeed;
    public SkillData SkillData => skillData;
    public int ViewSide => viewSide;
}
