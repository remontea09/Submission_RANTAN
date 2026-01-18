using System;
using EnumUtilities;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour {
    [SerializeField] private Image[] rangeArray;
    [SerializeField] private Image skillIcon;

    public void InitSkillIcon(SkillData skill) {
        skillIcon.sprite = skill.Sprite;

        int maxSideLength = (int)Math.Sqrt(rangeArray.Length);
        var range = ActionRangeTable.ActionRangeDictionary[skill.RangeType];
        int offset = (maxSideLength - range.GetLength(0)) / 2;
        for (int i = 0; i < maxSideLength; ++i) {
            for (int j = 0; j < maxSideLength; ++j) {
                Color cellColor = new Color(1f, 1f, 1f, 0.25f);
                int h = i - offset;
                int v = j - offset;
                if ((0 <= h && h < range.GetLength(0)) && (0 <= v && v < range.GetLength(1)) && range[h, v] == 1) {
                    cellColor = new Color(0, 0, 0, 0.75f);
                }

                rangeArray[i * maxSideLength + j].color = cellColor;
            }
        }
    }
}
