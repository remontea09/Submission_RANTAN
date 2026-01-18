using UnityEngine;

[CreateAssetMenu(fileName = "OnHitBuffSkillData", menuName = "ScriptableObjects/OnHitBuffSkillData")]
public class OnHitBuffSkillData : SupportSkillData {
    [Header("ヒット時バフ専用")]
    [SerializeField] private int duration;
    [TextArea]
    [SerializeField] private string descriptionText;
    private string DescriptionText => descriptionText.Replace("{power}", Power.ToString());
    public override string Description => $"[バフ]\n{DescriptionText}({duration}ターン)";

    public override void ExecuteSkill(StageCharacter attacker, Direction direction) {

    }
}
