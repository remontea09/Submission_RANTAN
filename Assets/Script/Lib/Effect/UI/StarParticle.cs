using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarParticle : MonoBehaviour {
    [SerializeField] private Image _starPrefab;
    [SerializeField] private Vector2 _starSize = new Vector2(10f, 10f);
    [SerializeField] private int _maxStarCount = 1000;
    [Header("範囲")]
    [SerializeField] private Vector2 _lifeTimeRange = new Vector2(0.5f, 1f);
    [SerializeField] private Vector2 _scaleRange = new Vector2(1f, 1.5f);

    private ObjectPool<Image> _starPool;
    private int _nowCount;

    private void Start() {
        _starPool = new ObjectPool<Image>(
            createFunc: () => {
                var star = Instantiate(_starPrefab, transform);
                star.rectTransform.sizeDelta = _starSize;
                return star;
            },
            actionOnGet: (obj) => {
                obj.gameObject.SetActive(true);
                _nowCount++;
            },
            actionOnRelease: (obj) => {
                obj.gameObject.SetActive(false);
                _nowCount--;
            }
        );
    }

    private void FixedUpdate() {
        if (_nowCount < _maxStarCount) {
            var pos = GetRandomLocalPosition();
            var star = _starPool.Get();
            star.transform.localPosition = pos;
            StartCoroutine(PlayStarAnimation(star));
        }
    }

    private Vector2 GetRandomLocalPosition() {
        Rect rect = (transform as RectTransform).rect;
        float x = Random.Range(rect.xMin, rect.xMax);
        float y = Random.Range(rect.yMin, rect.yMax);
        return new Vector2(x, y);
    }

    private IEnumerator PlayStarAnimation(Image star) {
        float lifeTime = Random.Range(_lifeTimeRange.x, _lifeTimeRange.y);
        float maxScale = Random.Range(_scaleRange.x, _scaleRange.y);

        var color = star.color;
        color.a = 0f;
        star.color = color;
        star.transform.localScale = Vector3.zero;

        float elapsed = 0f;
        while (elapsed < lifeTime) {
            float normalized = elapsed / lifeTime;
            float phase = normalized <= 0.5f
                ? normalized * 2f
                : (1f - normalized) * 2f;

            star.transform.localScale = Vector3.one * (maxScale * phase);
            var newColor = star.color;
            newColor.a = phase;
            star.color = newColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        _starPool.Release(star);
    }
}
