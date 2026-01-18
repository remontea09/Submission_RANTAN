using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuffIconManager : MonoBehaviour {
    [Header("バフ")]
    [SerializeField] private ScrollRect _buffScrollRect;
    [SerializeField] private BuffIcon _buffIconPrefab;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private BuffDetail _buffDetail;
    [SerializeField] private Button _closeButton;

    private bool _isBuffDirty = false;
    private List<BuffIcon> _buffIconList;

    public void InitPlayerBuffIcons() {
        Dao.StageInstance.Player.OnChangeBuff += SetBuffDirty;
        _buffDetail.CloseBuffDetail();
        _closeButton.onClick.AddListener(() => {
            _buffIconList.ForEach(icon => icon.UnSelect());
        });
        _buffIconList = new();
    }

    private void LateUpdate() {
        if (_isBuffDirty) {
            UpdateBuffs();
            _isBuffDirty = false;
        }
    }

    private void OnDestroy() {
        if (Dao.StageInstance.Player is not null) {
            Dao.StageInstance.Player.OnChangeBuff -= SetBuffDirty;
        }
    }

    private void SetBuffDirty() => _isBuffDirty = true;

    private void UpdateBuffs() {
        var player = Dao.StageInstance.Player;
        if (player is null) return;

        _buffIconList.Clear();
        var buffIconParent = _buffScrollRect.content;
        foreach (Transform child in buffIconParent) Destroy(child.gameObject);

        foreach (var x in player.StatusBuffs.Values) {
            foreach (var buffList in x.Values) {
                foreach (var buff in buffList) {
                    AddNewBuff(buff);
                }
            }
        }
        foreach (var buff in player.TurnEndBuffs) {
            AddNewBuff(buff);
        }
    }

    private void AddNewBuff(CharacterBuff buff) {
        if (buff.Icon is null) return;

        var buffIconParent = _buffScrollRect.content;
        var buffIcon = Instantiate(_buffIconPrefab, buffIconParent);
        buffIcon.InitBuffIcon(buff, _toggleGroup, _buffDetail);
        _buffIconList.Add(buffIcon);
    }
}
