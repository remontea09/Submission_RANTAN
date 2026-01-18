using System;
using System.Collections.Generic;
using System.Linq;

public static class LevelUtil {
    private readonly static float PER_LEVEL_INCREASE = 0.1f;
    private readonly static float PER_LEVEL_INCREASE_HP = 0.01f;

    private static List<int> _needXpList = new List<int>() { 0 };

    // 現在XPから到達済みレベル
    public static int GetLevelFromXp(int xp) {
        int level = _needXpList.UpperBound(xp);
        if (level == _needXpList.Count) {
            while (xp >= _needXpList.Last()) {
                _needXpList.Add(_needXpList.Last() + _needXpList.Count);
            }
            level = _needXpList.Count;
        }
        return level;
    }

    // 現在XPに対する次レベルへの進捗割合（0〜1）
    public static float GetXpProgressRate(int currentXp) {
        int level = GetLevelFromXp(currentXp);
        int nextXp = _needXpList[level];
        int preXp = _needXpList[level - 1];
        int right = nextXp - preXp;
        int current = currentXp - preXp;
        return (float)current / right;
    }

    // レベルの補正値
    public static float GetLevelCoefficient(int level) {
        return 1.0f + PER_LEVEL_INCREASE * (level - 1);
    }

    public static float GetLevelCoefficientForHp(int level) {
        return 1.0f + PER_LEVEL_INCREASE_HP * (level - 1);
    }
}
