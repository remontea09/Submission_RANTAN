using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScenarioRunner : MonoBehaviour {

    private ScenarioCatalog scenarioCatalog;
    private string scenarioId;

    [Header("Other Catalogs")]
    [SerializeField] private PortraitCatalog portraitCatalog;
    [SerializeField] private BackgroundCatalog backgroundCatalog;
    [SerializeField] private SfxCatalog sfxCatalog;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private Image backgroundImage;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Typewriter")]
    [SerializeField] private float charsPerSecond = 45f;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("NextButton")]
    [SerializeField] private Button nextButton;

    [Header("BG Fade (DOTween)")]
    [SerializeField] private float bgFadeOutDuration = 0.12f;
    [SerializeField] private float bgFadeInDuration = 0.22f;
    [SerializeField] private Ease bgFadeEase = Ease.OutQuad;

    private ScenarioData data;
    private int index;

    private Coroutine typingCo;
    private bool isTyping;
    private int typingTotalChars;

    private Tween bgTween;
    private float bgBaseAlpha = 1f;

    private void Awake() {

        AudioService.Instance.InitStoryAudio();

        scenarioCatalog = StoryStorage.catalog;
        scenarioId = StoryStorage.id;

        if (nextButton != null) nextButton.onClick.AddListener(Next);

        if (backgroundImage != null)
            bgBaseAlpha = backgroundImage.color.a;

        if (!string.IsNullOrEmpty(scenarioId))
            LoadScenario(scenarioId);
    }

    private void OnDisable() {
        if (bgTween != null) bgTween.Kill();
        bgTween = null;
    }

    public void LoadScenario(string id) {
        StopTyping();

        if (scenarioCatalog == null) {
            Debug.LogWarning("ScenarioCatalog is null");
            return;
        }

        var jsonAsset = scenarioCatalog.Get(id);
        if (jsonAsset == null) {
            Debug.LogWarning($"Scenario not found: {id}");
            return;
        }

        data = JsonUtility.FromJson<ScenarioData>(jsonAsset.text);
        index = 0;

        if (data?.entries == null) {
            Debug.LogWarning($"Scenario JSON parse failed or entries null: {id}");
            return;
        }

        Next();
    }

    public void StartScenario(string id) {
        LoadScenario(id);
    }

    public void Next() {
        if (isTyping) {
            CompleteTyping();
            return;
        }

        if (data?.entries == null) {
            Debug.LogWarning("Scenario data is null. LoadScenario/StartScenario first.");
            return;
        }

        while (index < data.entries.Length) {
            var e = data.entries[index++];

            if (!string.IsNullOrEmpty(e.bg)) {
                var bg = backgroundCatalog != null ? backgroundCatalog.Get(e.bg) : null;

                if (bg != null && backgroundImage != null) {
                    ChangeBackgroundWithFade(bg);
                }
                else if (backgroundCatalog == null) Debug.LogWarning("BackgroundCatalog is null");
                else Debug.LogWarning($"BG not found: {e.bg}");
            }

            if (!string.IsNullOrEmpty(e.portrait)) {
                var sp = portraitCatalog != null ? portraitCatalog.Get(e.portrait) : null;
                if (sp != null && portraitImage != null) portraitImage.sprite = sp;
                else if (portraitCatalog == null) Debug.LogWarning("PortraitCatalog is null");
                else Debug.LogWarning($"Portrait not found: {e.portrait}");
            }

            if (!string.IsNullOrEmpty(e.sfx)) {
                var clip = sfxCatalog != null ? sfxCatalog.Get(e.sfx) : null;
                if (clip != null && sfxSource != null) sfxSource.PlayOneShot(clip);
                else if (sfxCatalog == null) Debug.LogWarning("SfxCatalog is null");
                else Debug.LogWarning($"SFX not found: {e.sfx}");
            }

            if (!string.IsNullOrEmpty(e.name) && nameText != null)
                nameText.text = e.name;

            if (!string.IsNullOrEmpty(e.text) && bodyText != null) {
                StartTyping(e.text);
                return;
            }
        }

        SceneManager.LoadScene("Story");
    }

    private void ChangeBackgroundWithFade(Sprite nextSprite) {
        if (backgroundImage == null) return;
        if (backgroundImage.sprite == nextSprite) return;

        if (bgTween != null) bgTween.Kill();

        bool independent = useUnscaledTime;

        var c = backgroundImage.color;
        float targetA = bgBaseAlpha <= 0f ? 1f : bgBaseAlpha;

        if (backgroundImage.sprite == null) {
            backgroundImage.sprite = nextSprite;
            backgroundImage.color = new Color(c.r, c.g, c.b, 0f);

            bgTween = backgroundImage
                .DOFade(targetA, bgFadeInDuration)
                .SetEase(bgFadeEase)
                .SetUpdate(independent);

            return;
        }

        var seq = DOTween.Sequence().SetUpdate(independent);

        seq.Append(backgroundImage.DOFade(0f, bgFadeOutDuration).SetEase(bgFadeEase));
        seq.AppendCallback(() => backgroundImage.sprite = nextSprite);
        seq.Append(backgroundImage.DOFade(targetA, bgFadeInDuration).SetEase(bgFadeEase));

        bgTween = seq;
    }

    private void StartTyping(string text) {
        StopTyping();

        bodyText.text = text;
        bodyText.maxVisibleCharacters = 0;
        bodyText.ForceMeshUpdate();

        typingTotalChars = bodyText.textInfo.characterCount;
        isTyping = true;
        typingCo = StartCoroutine(TypeCoroutine());
    }

    private IEnumerator TypeCoroutine() {
        float visible = 0f;

        while (true) {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            visible += charsPerSecond * dt;

            int v = Mathf.Clamp(Mathf.FloorToInt(visible), 0, typingTotalChars);
            bodyText.maxVisibleCharacters = v;

            if (v >= typingTotalChars) break;

            yield return null;
        }

        isTyping = false;
        typingCo = null;
    }

    private void CompleteTyping() {
        if (bodyText != null)
            bodyText.maxVisibleCharacters = int.MaxValue;

        StopTypingInternal();
    }

    private void StopTyping() {
        if (!isTyping && typingCo == null) return;
        StopTypingInternal();
    }

    private void StopTypingInternal() {
        if (typingCo != null) {
            StopCoroutine(typingCo);
            typingCo = null;
        }

        isTyping = false;
        typingTotalChars = 0;
    }
}
