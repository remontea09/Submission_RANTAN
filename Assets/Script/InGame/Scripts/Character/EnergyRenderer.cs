using DG.Tweening;
using UnityEngine;

public class EnergyRenderer : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _red;
    [SerializeField] private Sprite _green;
    [SerializeField] private Sprite _blue;

    private EnergyType _energyType = EnergyType.Red;
    private Sequence _changeSequence;

    private void Awake() {
        SetSprite();
    }

    public void PlayChangeSpriteAnimation(EnergyType type) {
        if (type == _energyType) return;

        _energyType = type;

        if (_changeSequence is null) {
            _changeSequence = DOTween.Sequence().SetAutoKill(false).Pause();
            _changeSequence.Append(
                transform
                .DOScale(0, 0.25f));
            _changeSequence.AppendCallback(SetSprite);
            _changeSequence.Append(
                transform
                .DOScale(1, 0.25f));
        }
        _changeSequence.Restart();
    }

    private void SetSprite() {
        _spriteRenderer.sprite = _energyType switch {
            EnergyType.Red => _red,
            EnergyType.Green => _green,
            EnergyType.Blue => _blue,
            _ => null
        };
    }
}
