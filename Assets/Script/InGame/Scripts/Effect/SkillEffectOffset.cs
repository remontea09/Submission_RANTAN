using Common.Const;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SkillEffectOffset : MonoBehaviour {
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Sprite _lastSprite;

    private void Update() {
        var sprite = _spriteRenderer.sprite;
        if (sprite != _lastSprite) {
            _lastSprite = sprite;
            if (sprite is not null) {
                Vector2 normalizedPivot = new Vector2(
                    sprite.pivot.x / sprite.rect.width,
                    sprite.pivot.y / sprite.rect.height
                );
                transform.localPosition = normalizedPivot * GrobalConst.CELL_SIZE - Vector2.one;
            }
        }
    }
}
