using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour {
    [SerializeField] private Button _selectButton;
    [SerializeField] private SkillIcon _icon;

    public Color NodeColor => _selectButton.image.color;

    public SkillData SkillData { get; private set; }
    public event Action OnSelectNode;

    private List<Image> _skillEdgeList = new();

    private readonly Color UNLOCKED_COLOR = new Color32(255, 225, 126, 255);
    private readonly Color UNLOCKED_COLOR_ATTACK = new Color32(255, 126, 135, 255);
    private readonly Color UNLOCKED_COLOR_SUPPORT = new Color32(126, 192, 255, 255);


    private void Awake() {
        _selectButton.onClick.AddListener(() => OnSelectNode.Invoke());
    }

    public void InitSkillNode(SkillData skillData) {
        SkillData = skillData;
        _icon.InitSkillIcon(skillData);
        SetSelect(false);
        SetColor();
    }

    public void AddEdge(Image edge) {
        edge.color = UNLOCKED_COLOR * Color.gray;
        _skillEdgeList.Add(edge);
    }

    public void Unlock(bool select = false) {
        foreach (var edge in _skillEdgeList) {
            edge.color = UNLOCKED_COLOR;
        }
        SetColor();
        SetSelect(select);
    }

    public void SetSelect(bool isSelect) {
        var targetScale = isSelect ? Vector3.one * 1.5f : Vector3.one;
        transform.DOScale(targetScale, 0.1f).SetEase(Ease.InSine).SetLink(gameObject);
        if (isSelect) {
            var targetColor = SkillData.SkillType == SkillType.Attack ? Color.red : Color.cyan;
            _selectButton.image.color = Color.Lerp(_selectButton.image.color, targetColor, 0.2f);
        }
        else {
            SetColor();
        }

        if (isSelect) {
            transform.parent.DOLocalMove(-transform.localPosition + Vector3.up * 200f, 0.1f).SetEase(Ease.InSine).SetLink(gameObject);
        }
    }

    private void SetColor() {
        bool isUnlocked = GameSessionService.Instance.UnlockedSkillIdSet.Contains(SkillData.Id);
        if (SkillData.SkillType == SkillType.Attack) {
            _selectButton.image.color = isUnlocked ? UNLOCKED_COLOR_ATTACK : UNLOCKED_COLOR_ATTACK * Color.gray;
        }
        else {
            _selectButton.image.color = isUnlocked ? UNLOCKED_COLOR_SUPPORT : UNLOCKED_COLOR_SUPPORT * Color.gray;
        }
    }
}
