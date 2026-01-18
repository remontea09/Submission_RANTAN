using TMPro;
using UnityEngine;

public class HomeManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI stageCountText;
    private OutGameSaveData outGameSaveData;

    /// <summary>
    /// セーブデータからクリア数を取得し好感度として表示する
    /// </summary>
    private void Start() {
        outGameSaveData = SaveSystem.LoadOutGame();
        stageCountText.text = outGameSaveData.WorldClearCount.ToString();
        AudioService.Instance.InitHomeAudio();
    }

}
