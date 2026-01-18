
using UnityEngine;

public static class StoryStorage
{
    public static string id = "0";
    public static ScenarioCatalog catalog;

    public static int givStory1Love = 0;
    public static int givStory2Love = 1;
    public static int givStory3Love = 2;
    public static int givStory4Love = 3;
    public static int givStory5Love = 4;
    public static int givStoryEXLove = 10;

    /// <summary>
    /// 受け取ったtupeに応じて必要なカタログを返す
    /// </summary>
    /// <param name="type">誰のシナリオか</param>
    /// <returns>必要なシナリオが入ったカタログ</returns>
    public static ScenarioCatalog GetScenarioCatalog(ScenarioType type) {
        switch (type) {
            case ScenarioType.prologue:
                return Resources.Load<ScenarioCatalog>("ScenarioData/Catalogs/Prologue_Scenario_Catalog");
            case ScenarioType.tenshi:
                return Resources.Load<ScenarioCatalog>("ScenarioData/Catalogs/Tenshi_Scenario_Catalog");
            default:
                return null;
        }
    }
}

public enum ScenarioType : short{
    prologue,
    tenshi
}
