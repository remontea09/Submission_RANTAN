using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogService : MonoBehaviour, ILogWriter {
    [SerializeField] private TMP_Text _logTextPrefab;
    [SerializeField] private ScrollRect _scrollRect;

    public static ILogWriter Instance;

    private static readonly int LOG_LIMIT = 100;

    private Queue<TMP_Text> _logQueue;

    public void Initialize() {
        Instance = this;
        _logQueue = new();
        WriteLog("[0] 朝日が昇った、冒険の時間だ");
    }

    public void WriteLog(string msg) {
        var log = Instantiate(_logTextPrefab, _scrollRect.content.transform);
        log.text = msg;
        _logQueue.Enqueue(log);
        while (_logQueue.Count > LOG_LIMIT ) {
            Destroy(_logQueue.Dequeue().gameObject);
        }
        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom() {
        yield return null;
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    private void OnDestroy() {
        Instance = null;
    }
}

public interface ILogWriter {
    void WriteLog(string msg);
}
