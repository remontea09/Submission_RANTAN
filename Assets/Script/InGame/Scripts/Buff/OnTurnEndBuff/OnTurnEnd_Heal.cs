using UnityEngine;

public class OnTurnEnd_Heal : OnTurnEndBuff {
    public override TriggeredBuffType BuffType => TriggeredBuffType.OnTurnEnd_Heal;
    public override string Description => $"ターン終了時にHPを{_amount}回復";

    private int _amount;

    public OnTurnEnd_Heal(int? duration, int amount, Sprite icon) : base(duration, icon) {
        _amount = amount;
    }

    public override void ExecuteBuff(StageCharacter character) {
        character.HealHp(_amount);
    }
}
