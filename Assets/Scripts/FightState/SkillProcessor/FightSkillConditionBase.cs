using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public abstract class FightSkillConditionBase
{
    protected FightActionBase fightAction;
    public FightSkillConditionBase(JSONNode jsonData)
    {
        ParseFrom(jsonData);
    }
    public abstract bool IsTrue();
    protected abstract void ParseFrom(JSONNode jsonData);
}
