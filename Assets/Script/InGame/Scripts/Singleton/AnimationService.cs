using System;
using System.Collections.Generic;
using Common.Const;
using DG.Tweening;
using EnumUtilities;
using UnityEngine;
using Utilities;

public class AnimationService : Base.Singleton<AnimationService> {
    private Queue<Tween> moveQueue = new();
    private Queue<(Tween tween, bool wait)> queue = new();
    private Sequence sequence;

    public bool IsPlaying { get; private set; }

    public void AddMoveAnimation(Transform transform, EntityType entityType, Index2D stagePosition) {
        var targetPosition = CoordinateConvertUtil.StageToWorldPosition(stagePosition);
        Tween tween = transform.DOMove(targetPosition, AnimationConst.MOVE_DURATION).SetLink(transform.gameObject).Pause();
        if (entityType == EntityType.Player && targetPosition != (Vector2)transform.position) {
            tween.OnStart(() => AudioService.Instance.PlaySE(AudioType.WalkSE));
        }
        moveQueue.Enqueue(tween);
    }
    public void AddSkillAnimation(Transform transform, EntityType entityType, Index2D stagePosition, Direction attackDireciton, SkillData skillData, Action startAction = null, Action completeAction = null) {
        Vector3 distance = CoordinateConvertUtil.StageToWorldPosition((Index2D)attackDireciton) / 2f;
        var originPosition = CoordinateConvertUtil.StageToWorldPosition(stagePosition);
        var targetPosition = transform.position + distance;
        Tween tween = transform.DOMove(targetPosition, AnimationConst.SKILL_DURATION / 2f).SetEase(Ease.OutSine).SetLink(transform.gameObject).Pause();
        if (entityType == EntityType.Player) {
            tween.OnComplete(() => AudioService.Instance.PlaySE(AudioType.AttackSE));
        }

        var range = ActionRangeTable.ActionRangeDictionary[skillData.RangeType];
        range = ArrayUtil.Skew45(range, attackDireciton);
        int height = range.GetLength(0);
        int width = range.GetLength(1);
        Index2D offset = new Index2D(height / 2, width / 2);
        for (int i = 0; i < height; ++i) {
            for (int j = 0; j < width; ++j) {
                if (range[i, j] == 1) {
                    var position = stagePosition + new Index2D(i, j) - offset;
                    var worldPosition = CoordinateConvertUtil.StageToWorldPosition(position);
                    startAction += () => EffectManager.PlayEffect(worldPosition, skillData.Effect);
                }
            }
        }
        tween.OnStart(() => startAction?.Invoke());
        queue.Enqueue((tween, true));

        tween = transform.DOMove(originPosition, AnimationConst.SKILL_DURATION / 2f).SetEase(Ease.InSine).SetLink(transform.gameObject).Pause().OnComplete(() => {
            completeAction?.Invoke();
        });
        queue.Enqueue((tween, true));
    }

    public void AddActionToAnimation(Action startAction = null, Action completeAction = null) {
        Tween emptyTween = DOTween.To(() => 0f, _ => { }, 0f, 0f).Pause().OnStart(() => startAction?.Invoke()).OnComplete(() => completeAction?.Invoke());
        queue.Enqueue((emptyTween, true));
    }

    public void AddHitAnimation(Transform transform, EntityType entityType, int damage, Action completeAction = null, bool wait = false) {
        Tween tween = transform.DOShakePosition(AnimationConst.HIT_DURATION, Vector3.right / 4f, vibrato: 20, randomness: 0).SetLink(transform.gameObject).Pause().OnStart(() => {
            AudioService.Instance.PlaySE(AudioType.TakeDamageSE);
            if (entityType != EntityType.Player) {
                NumberEffectManager.PlayEffect(transform.position, -damage);
            }
        }).OnComplete(() => {
            completeAction?.Invoke();
        });
        queue.Enqueue((tween, wait));
    }

    public void AddDeathAnimation(Transform transform, Action onComplete) {
        Tween tween = transform.DOScale(Vector3.zero, AnimationConst.DEATH_DURATION).SetEase(Ease.InBack).SetLink(transform.gameObject).Pause().OnComplete(onComplete.Invoke);
        queue.Enqueue((tween, true));
    }

    public void AddChangePlayerAnimation(Transform transform, Action changeAction) {
        Tween tween = transform.DOLocalRotate(new Vector3(0, -90f, 0), AnimationConst.CHANGE_PLAYER_DURATION).SetLink(transform.gameObject).Pause().OnComplete(() => {
            changeAction?.Invoke();
            transform.localRotation = Quaternion.Euler(0, 90f, 0);
        });
        queue.Enqueue((tween, true));
        tween = transform.DOLocalRotate(Vector3.zero, AnimationConst.CHANGE_PLAYER_DURATION).SetLink(transform.gameObject).Pause().OnComplete(() => {
            transform.localRotation = Quaternion.identity;
        });
        queue.Enqueue((tween, true));
    }

    public void Play(Action callback = null) {
        if (IsPlaying || (moveQueue.Count == 0 && queue.Count == 0)) return;

        IsPlaying = true;

        Sequence moveSequence = DOTween.Sequence().SetAutoKill(true).Pause();
        while (moveQueue.Count > 0) {
            moveSequence.Join(moveQueue.Dequeue());
        }
        Queue<(Tween, bool)> seqQueue = new(queue);
        queue.Clear();
        moveSequence.OnComplete(() => {
            PlaySequence(seqQueue, callback);
        }).Play();
    }

    private void PlaySequence(Queue<(Tween, bool)> seqQueue, Action callback = null) {
        CreateNewBuffer();

        while (seqQueue.Count > 0) {
            var (tween, wait) = seqQueue.Dequeue();
            if (wait) {
                sequence.Append(tween);
            }
            else {
                sequence.Join(tween);
            }
        }

        sequence.OnComplete(() => {
            IsPlaying = false;
            callback?.Invoke();
        }).Play();
    }

    private void CreateNewBuffer() {
        sequence = DOTween.Sequence().SetAutoKill(true).Pause();
    }
}
