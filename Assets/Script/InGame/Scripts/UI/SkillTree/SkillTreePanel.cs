using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreePanel : MonoBehaviour {
    [SerializeField] private Button _closeButton;
    [SerializeField] private RectTransform _skillNodesParent;
    [SerializeField] private RectTransform _skillEdgesParent;
    [SerializeField] private SkillNode _skillNodePrefab;
    [SerializeField] private Image _skillEdgePrefab;
    [SerializeField] private SkillDetailPopup _skillDetailPopup;
    [SerializeField] private ConfirmPopup _skillSetConfirmPopupPrefab;
    [Header("星座")]
    [SerializeField] private ConstellationData[] _constellationDataArray;

    private static Dictionary<ConstellationCategory, ConstellationData> _constellationDataDict;

    private SkillNode[] _skillNodeArray = null;
    private Dictionary<int, HashSet<int>> _skillNetwork = new();
    private SkillNode _selectedNode = null;
    private Rect _parentRect;

    private void Awake() {
        _constellationDataDict = _constellationDataArray.ToDictionary(data => data.Category, data => data);
    }

    private void Start() {
        _closeButton.onClick.AddListener(() => Close());
        GameSessionService.Instance.OnPlayerSkillUnlocked += OpenSetConfirmPopup;
    }

    private void OnDestroy() {
        GameSessionService.Instance.OnPlayerSkillUnlocked -= OpenSetConfirmPopup;
    }

    public void OpenSkillTreeTab(SkillType skillType) {
        _parentRect = default;
        var targetSkill = GameSessionService.Instance.PlayerEnableSupportSkill;
        if (skillType == SkillType.Attack || targetSkill is null) {
            targetSkill = GameSessionService.Instance.PlayerEnableAttackSkill;
        }
        SetUpSkillNetwork(_constellationDataDict[targetSkill.Category]);

        Dictionary<int, SkillData> nodeSkillDictionary = new();
        foreach (var skill in Dao.Master.SkillMaster.Values) {
            if (skill.Category == targetSkill.Category) {
                var idx = skill.Index;
                if (nodeSkillDictionary.ContainsKey(idx)) {
                    var preSkill = nodeSkillDictionary[idx];
                    if (preSkill.Level < skill.Level && GameSessionService.Instance.UnlockedSkillIdSet.Contains(skill.Id)) {
                        nodeSkillDictionary[idx] = skill;
                    }
                }
                else {
                    nodeSkillDictionary[idx] = skill;
                }
            }
        }

        foreach (var nodeIdxAndSkill in nodeSkillDictionary) {
            var skill = nodeIdxAndSkill.Value;
            var node = _skillNodeArray[nodeIdxAndSkill.Key];
            node.InitSkillNode(skill);
            bool isUnlocked = GameSessionService.Instance.UnlockedSkillIdSet.Contains(skill.Id);
            if (isUnlocked) node.Unlock();
        }

        var startIndex = targetSkill.Index;
        _skillNodesParent.localPosition = -(_skillNodeArray[startIndex].transform.localPosition) + Vector3.up * 200f;
        SelectInNodeList(startIndex);
    }

    private void Close() {
        foreach (var node in _skillNodeArray) Destroy(node.gameObject);
        foreach (Transform edge in _skillEdgesParent) Destroy(edge.gameObject);
        _skillNodeArray = null;
        _selectedNode = null;
        _skillNetwork.Clear();

        gameObject.SetActive(false);
    }

    private void SetUpSkillNetwork(ConstellationData data) {
        _skillNodeArray = new SkillNode[data.nodePositions.Length];

        for (int i = 0; i < data.nodePositions.Length; ++i) {
            var nodePosition = data.nodePositions[i];
            var node = Instantiate(_skillNodePrefab, _skillNodesParent);
#if UNITY_EDITOR
            node.name = i.ToString();
#endif
            node.transform.localPosition = nodePosition;
            _skillNodeArray[i] = node;

            var nodeListIndex = i;
            node.OnSelectNode += (() => SelectInNodeList(nodeListIndex));

            _parentRect.xMin = Math.Min(_parentRect.xMin, nodePosition.x);
            _parentRect.xMax = Math.Max(_parentRect.xMax, nodePosition.x);
            _parentRect.yMin = Math.Min(_parentRect.yMin, nodePosition.y);
            _parentRect.yMax = Math.Max(_parentRect.yMax, nodePosition.y);
        }

        var nodeOffset = Vector3.up * _parentRect.height / 4;

        foreach (var pair in data.edges) {
            if (_skillNetwork.ContainsKey(pair.x)) _skillNetwork[pair.x].Add(pair.y);
            else _skillNetwork.Add(pair.x, new HashSet<int> { pair.y });
            if (_skillNetwork.ContainsKey(pair.y)) _skillNetwork[pair.y].Add(pair.x);
            else _skillNetwork.Add(pair.y, new HashSet<int> { pair.x });

            var posA = data.nodePositions[pair.x] + (Vector2)nodeOffset;
            var posB = data.nodePositions[pair.y] + (Vector2)nodeOffset;
            var diff = posB - posA;
            float distance = diff.magnitude;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            var edge = Instantiate(_skillEdgePrefab, _skillEdgesParent);
            _skillNodeArray[pair.x].AddEdge(edge);
            _skillNodeArray[pair.y].AddEdge(edge);
            var lineRect = edge.transform as RectTransform;
            lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y);
            lineRect.anchoredPosition = posA;
            lineRect.rotation = Quaternion.Euler(0, 0, angle);
        }

        foreach (var node in _skillNodeArray) {
            node.transform.localPosition += nodeOffset;
        }

        _skillNodesParent.sizeDelta = new Vector2(
            _parentRect.width * 1.5f,
            _parentRect.height * 2f
        );
    }

    private void SelectInNodeList(int nodeListIndex) {
        _selectedNode?.SetSelect(false);
        var newNode = _skillNodeArray[nodeListIndex];
        bool isConnected = _skillNetwork[nodeListIndex]
            .Any(skillIndex => GameSessionService.Instance.UnlockedSkillIdSet.Contains(_skillNodeArray[skillIndex].SkillData.Id));
        newNode.SetSelect(true);
        _skillDetailPopup.InitSkillDetailPopup(newNode, isConnected);
        _selectedNode = newNode;
    }

    private void OpenSetConfirmPopup(SkillData newSkill) {
        var compareSkill = newSkill.SkillType == SkillType.Attack
            ? GameSessionService.Instance.PlayerEnableAttackSkill
            : GameSessionService.Instance.PlayerEnableSupportSkill;
        if (newSkill == compareSkill?.NextSkillData) {
            return;
        }

        var popup = Instantiate(_skillSetConfirmPopupPrefab, transform);
        Action<bool> popupAction = (isOk) => {
            if (isOk) {
                GameSessionService.Instance.ChangePlayerSkill(newSkill);
                _skillDetailPopup.UpdateSetState();
            }
        };
        popup.InitConfirmPopup("解放したスキルを\nセットしますか", "セット", "キャンセル", popupAction);
    }
}
