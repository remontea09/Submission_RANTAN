using System;
using static System.Drawing.Color;
using Common.Const;
using DG.Tweening;
using UnityEngine;

public class NumberEffectManager : MonoBehaviour {
    [SerializeField] private SpriteRenderer _numberEffectPrefab;
    [SerializeField] private Sprite[] _serializeNumberSpriteArray;

    private static Func<SpriteRenderer> CreateEffectFunc;
    private static Action<SpriteRenderer> ReleaseEffectAction;
    private static Sprite[] _numberSpriteArray;
    private static float _spriteWidth;

    private ObjectPool<SpriteRenderer> _effectPool;

    private void Awake() {
        _effectPool = new ObjectPool<SpriteRenderer>(
            createFunc: () => {
                return Instantiate(_numberEffectPrefab, transform);
            },
            actionOnGet: (obj) => {
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (obj) => {
                obj.gameObject.SetActive(false);
            }
        );

        CreateEffectFunc = () => _effectPool.Get();
        ReleaseEffectAction = (obj) => _effectPool.Release(obj);

        _spriteWidth = _numberEffectPrefab.bounds.size.x;
        _numberSpriteArray = _serializeNumberSpriteArray;
    }

    private void OnDestroy() {
        CreateEffectFunc = null;
        ReleaseEffectAction = null;
        _numberSpriteArray = null;
    }

    public static void PlayEffect(Vector3 position, int number) {
        string numberString = Math.Abs(number).ToString();
        int count = numberString.Length;
        float verticalOffset = -1 * (_spriteWidth / 2) * (count - 1);

        for (int i = 0; i < count; ++i) {
            var effect = CreateEffectFunc?.Invoke();
            effect.sprite = _numberSpriteArray[numberString[i] - '0'];
            effect.transform.position = position + (Vector3.right * verticalOffset);
            verticalOffset += _spriteWidth;

            Color color = number < 0 ? new Color32(0xFF, 0xFF, 0x00, 0x00) : new Color32(0x00, 0xFF, 0x00, 0x00);
            SetNumberAnimation(effect, color);
        }
    }

    private static void SetNumberAnimation(SpriteRenderer effect, Color color) {
        effect.color = color;
        effect.transform.localScale = Vector3.one * 3f;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(effect.DOFade(1, AnimationConst.EFFECT_DURATION / 3));
        sequence.Join(effect.transform.DOScale(1f, AnimationConst.EFFECT_DURATION / 3));
        sequence.AppendInterval(AnimationConst.EFFECT_DURATION / 6);
        sequence.Append(effect.DOFade(0, AnimationConst.EFFECT_DURATION / 2));
        sequence.Join(effect.transform.DOMoveY(0.5f, AnimationConst.EFFECT_DURATION / 2).SetRelative());
        sequence.OnComplete(() => ReleaseEffectAction?.Invoke(effect)).Play();
    }
}
