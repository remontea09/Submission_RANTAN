using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour {
    [SerializeField] private Image playerXpImage;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text playerSkillPointText;

    private const float LEVEL_ANIMATION_DURATION = 0.5f;

    private int _preLevel;

    public void InitPlyaerLevelUI() {
        GameSessionService.Instance.OnPlayerXpChanged += UpdateXp;
        playerXpImage.fillAmount = LevelUtil.GetXpProgressRate(GameSessionService.Instance.PlayerXp);
        playerLevelText.text = GameSessionService.Instance.PlayerLevel.ToString();
        playerSkillPointText.text = GameSessionService.Instance.SkillPoint.ToString();
        _preLevel = GameSessionService.Instance.PlayerLevel;
    }

    private void OnDestroy() {
        GameSessionService.Instance.OnPlayerXpChanged -= UpdateXp;
    }

    private void UpdateXp() {
        var xp = GameSessionService.Instance.PlayerXp;
        int level = GameSessionService.Instance.PlayerLevel;
        Sequence sequence = DOTween.Sequence();
        int levelDiff = level - _preLevel;
        for (int i = 0; i < levelDiff; ++i) {
            Tween tween =
                playerXpImage.DOFillAmount(1, LEVEL_ANIMATION_DURATION / (levelDiff + 1)).Pause().OnComplete(() => {
                    playerXpImage.fillAmount = 0;
                    AudioService.Instance.PlayLevelUpSE();
                });
            sequence.Append(tween);
        }
        float progressRate = LevelUtil.GetXpProgressRate(xp);
        if (progressRate > 0) {
            sequence.Append(playerXpImage.DOFillAmount(progressRate, LEVEL_ANIMATION_DURATION / (levelDiff + 1)).Pause());
        }
        sequence.SetLink(gameObject).OnComplete(() => {
            playerLevelText.text = level.ToString();
            playerSkillPointText.text = GameSessionService.Instance.SkillPoint.ToString();
        }).Play();

        _preLevel = level;
    }
}
