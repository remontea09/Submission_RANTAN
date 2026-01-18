using UnityEngine;
using TMPro;

public class BuffDetail : MonoBehaviour {
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _turnText;

    public void OpenBuffDetail(string description, int? turn) {
        _descriptionText.text = description;
        _turnText.text = turn is null ? "常時" :  $"残り {turn.ToString()} ターン";
        gameObject.SetActive(true);
    }

    public void CloseBuffDetail() {
        gameObject.SetActive(false);
    }
}
