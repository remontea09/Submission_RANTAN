using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TenshiScenarioManager : ScenarioManagerBase {

    //好感度によってストーリーを解放
    public override void SetEnabled() {
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStory1Love) story1Button.interactable = true;
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStory2Love) story2Button.interactable = true;
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStory3Love) story3Button.interactable = true;
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStory4Love) story4Button.interactable = true;
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStory5Love) story5Button.interactable = true;
        if(outGameSaveData.WorldClearCount >= StoryStorage.givStoryEXLove) storyEXButton.interactable = true;
    }

    public override void SetOnClick() {
        story1Button.onClick.AddListener(() => GoRun("1", ScenarioType.tenshi));
        story2Button.onClick.AddListener(() => GoRun("2", ScenarioType.tenshi));
        story3Button.onClick.AddListener(() => GoRun("3", ScenarioType.tenshi));
        story4Button.onClick.AddListener(() => GoRun("4", ScenarioType.tenshi));
        story5Button.onClick.AddListener(() => GoRun("5", ScenarioType.tenshi));
        storyEXButton.onClick.AddListener(() => GoRun("EX", ScenarioType.tenshi));
    }


}
