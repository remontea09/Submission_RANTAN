using System;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    [SerializeField] private SkillEffect _effect;

    private static Func<SkillEffect> CreateEffectFunc;
    private static Action<SkillEffect> ReleaseEffectAction;

    private ObjectPool<SkillEffect> _effectPool;

    private void Awake() {
        _effectPool = new ObjectPool<SkillEffect>(
            createFunc: () => {
                return Instantiate(_effect, transform);
            },
            actionOnGet: (obj) => {
                obj.transform.SetParent(transform);
                obj.gameObject.SetActive(true);
            },
            actionOnRelease: (obj) => {
                obj.gameObject.SetActive(false);
            }
        );

        CreateEffectFunc = () => _effectPool.Get();
        ReleaseEffectAction = (obj) => _effectPool.Release(obj);
    }

    private void OnDestroy() {
        CreateEffectFunc = null;
        ReleaseEffectAction = null;
    }

    public static void PlayEffect(Vector3 position, RuntimeAnimatorController animatorController) {
        if (animatorController == null) return;

        var effect = CreateEffectFunc?.Invoke();

        effect.InitSkillEffect(position, animatorController, ReleaseEffectAction);
    }
}
