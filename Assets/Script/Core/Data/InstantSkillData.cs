using Common;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantSkillData", menuName = "ScriptableObjects/InstantSkillData")]
public class InstantSkillData : SupportSkillData {
    [Header("即時発動専用")]
    [TextArea]
    [SerializeField] private string descriptionText;
    private string DescriptionText => descriptionText.Replace("{power}", Power.ToString());
    public override string Description => $"{DescriptionText}";

    public override void ExecuteSkill(StageCharacter attacker, Direction direction) {
        switch (id) {
            case 107:
            case 207:
            case 307:
                attacker.HealHp(Power);
                break;
            case 112:
            case 212:
            case 312:
                attacker.TakeDamage(attacker.CurrentHp / 2);
                (int a, int b, int c) = RandomHelper.SplitIntoThree(Power);
                GameSessionService.Instance.AcquireEnergy(EnergyType.Red, a);
                GameSessionService.Instance.AcquireEnergy(EnergyType.Green, b);
                GameSessionService.Instance.AcquireEnergy(EnergyType.Blue, c);
                break;
            case 117:
            case 217:
            case 317:
                attacker.AddBuffTurn(Power);
                break;
        }
    }
}
