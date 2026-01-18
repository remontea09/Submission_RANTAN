using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common.Const;
using UnityEngine;

public class SkillEffect : MonoBehaviour {
    [SerializeField] private Animator _effectAnimator;

    private readonly static int AnimatorTrigger = Animator.StringToHash("Trigger");

    private Action<SkillEffect> _releaseAction;

    public void InitSkillEffect(Vector3 position, RuntimeAnimatorController animatorController, Action<SkillEffect> releaseAction) {
        _releaseAction = releaseAction;

        transform.position = position;
        _effectAnimator.runtimeAnimatorController = animatorController;
        _effectAnimator.SetTrigger(AnimatorTrigger);

        Regex regex = new Regex(@"^([A-Za-z_]+)(\d+)$");
        HashSet<string> randomClipSet = new();
        float sumSeconds = 0f;
        foreach (var clip in animatorController.animationClips) {
            var match = regex.Match(clip.name);
            if (match.Success) {
                if (randomClipSet.Contains(match.Groups[1].Value)) continue;
                else randomClipSet.Add(match.Groups[1].Value);
            }
            sumSeconds += clip.length;
        }
        _effectAnimator.speed = sumSeconds / AnimationConst.EFFECT_DURATION;

        StartCoroutine(ReleaseAfter());
    }

    private IEnumerator ReleaseAfter() {
        yield return new WaitForSeconds(AnimationConst.EFFECT_DURATION);
        _releaseAction?.Invoke(this);
    }
}
