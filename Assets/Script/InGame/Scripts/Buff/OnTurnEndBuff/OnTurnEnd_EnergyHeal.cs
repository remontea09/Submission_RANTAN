using UnityEngine;

public class OnTurnEnd_EnergyHeal : OnTurnEndBuff {
    public override TriggeredBuffType BuffType => TriggeredBuffType.OnTurnEnd_Heal;
    public override string Description => $"ターン終了時に全エネルギーを{_amount}個ずつ取得";

    private int _amount;

    public OnTurnEnd_EnergyHeal(int? duration, int amount, Sprite icon) : base(duration, icon) {
        _amount = amount;
    }

    public override void ExecuteBuff(StageCharacter character) {
        GameSessionService.Instance.AcquireEnergy(EnergyType.Red, _amount);
        GameSessionService.Instance.AcquireEnergy(EnergyType.Green, _amount);
        GameSessionService.Instance.AcquireEnergy(EnergyType.Blue, _amount);
    }
}
