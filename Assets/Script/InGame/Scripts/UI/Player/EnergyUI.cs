using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour {
    [SerializeField] private TMP_Text _redEnergyText;
    [SerializeField] private TMP_Text _greenEnergyText;
    [SerializeField] private TMP_Text _blueEnergyText;

    private Energy _preEnergy;

    public void InitEnergyUI() {
        GameSessionService.Instance.OnPlayerEnergyChanged += UpdateEnergy;
        UpdateEnergy(GameSessionService.Instance.Energy);
    }

    private void OnDestroy() {
        GameSessionService.Instance.OnPlayerEnergyChanged -= UpdateEnergy;
    }

    private void UpdateEnergy(Energy energy) {
        _redEnergyText.text = energy.red.ToString();
        _greenEnergyText.text = energy.green.ToString();
        _blueEnergyText.text = energy.blue.ToString();

        var diffEnergy = energy - _preEnergy;
        if (diffEnergy.red > 0) JumpAnim(_redEnergyText.rectTransform);
        if (diffEnergy.green > 0) JumpAnim(_greenEnergyText.rectTransform);
        if (diffEnergy.blue > 0) JumpAnim(_blueEnergyText.rectTransform);

        _preEnergy = energy;
    }

    /// <summary>
    /// 入手したエネルギーをアニメーションさせる
    /// </summary>
    /// <param name="transform">入手したエネルギーのUI</param>
    private void JumpAnim(RectTransform transform) {
        var seq = DOTween.Sequence();

        seq.Append(transform.DOAnchorPosY(transform.anchoredPosition.y + 20f, 0.1f))
           .Append(transform.DOAnchorPosY(transform.anchoredPosition.y, 0.1f));
    }
}
