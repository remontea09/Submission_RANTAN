using UnityEngine;

[CreateAssetMenu(fileName = "StatusBuffSkillData", menuName = "ScriptableObjects/StatusBuffSkillData")]
public class StatusBuffSkillData : SupportSkillData {
    [Header("ステータスバフ専用")]
    [SerializeField] private int duration;
    [SerializeField] private StatusType[] types;
    [SerializeField] private StatusBuffType formula;
    [TextArea]
    [SerializeField] private string descriptionText;
    private string DescriptionText => descriptionText.Replace("{power}", Power.ToString());
    public override string Description => $"[バフ]\n{DescriptionText}({duration}ターン)";

    public override void ExecuteSkill(StageCharacter attacker, Direction direction) {
        var targetList = BattleHelper.GetTargetList(attacker.StagePosition, RangeType, direction);
        foreach (var type in types) {
            BattleHelper.ApplyStatusBuff(attacker, targetList, () => StatusBuffFactory.Create(formula, type, duration, Power, Sprite));
        }
    }
}
