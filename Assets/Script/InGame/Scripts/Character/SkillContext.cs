using System.Collections.Generic;

public class SkillContext {
    public StageCharacter attacker;
    public List<StageCharacter> defenderList;
    public SkillData skillData;

    public SkillContext(StageCharacter attacker, List<StageCharacter> defenderList, SkillData skillData) {
        this.attacker = attacker;
        this.defenderList = defenderList;
        this.skillData = skillData;
    }
}
