using UnityEngine;

[CreateAssetMenu(fileName = "OnTurnEndBuffSkillData", menuName = "ScriptableObjects/OnTurnEndBuffSkillData")]
public class OnTurnEndBuffSkillData : SupportSkillData {
    [Header("ヒット時バフ専用")]
    [SerializeField] private TriggeredBuffType buffType;
    [SerializeField] private int duration;
    [TextArea]
    [SerializeField] private string descriptionText;
    private string DescriptionText => descriptionText.Replace("{power}", Power.ToString());
    public override string Description => $"[バフ]\n{DescriptionText}({duration}ターン)";

    public override void ExecuteSkill(StageCharacter attacker, Direction direction) {
        attacker.AddTurnEndBuff(OnTurnEOnndBuffFactory.Create(buffType, duration, Power, Sprite));
    }
}
