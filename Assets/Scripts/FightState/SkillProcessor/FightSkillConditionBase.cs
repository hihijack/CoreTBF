using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public abstract class FightSkillConditionBase
{
    protected FightActionBase fightAction;
    public FightSkillConditionBase(FightActionBase fightAction, JSONNode jsonData)
    {
        this.fightAction = fightAction;
        ParseFrom(jsonData);
    }
    public abstract bool IsTrue();
    protected abstract void ParseFrom(JSONNode jsonData);
}
