using UnityEngine;

public class PlayerMainData : MasterData, IStatus {
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private string playerName;
    [SerializeField] private int defaultHp;
    [SerializeField] private int defaultMagicPower;
    [SerializeField] private int defaultDefense;
    [SerializeField] private int defaultSpeed;

    public Sprite PlayerSprite => playerSprite;
    public string Name => playerName;
    public int MaxHp => defaultHp;
    public int MagicPower => defaultMagicPower;
    public int Defense => defaultDefense;
    public int Speed => defaultSpeed;
}
