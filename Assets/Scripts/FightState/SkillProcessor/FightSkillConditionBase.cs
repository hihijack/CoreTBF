using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;

public abstract class FightSkillConditionBase
{
    protected FightActionBase fightAction;
    protected FightSkillProcessorBase ownerProc;
    public FightSkillConditionBase(JSONNode jsonData)
    {
        ParseFrom(jsonData);
    }

    public void SetOwnerProc(FightSkillProcessorBase proc)
    {
        this.ownerProc = proc;
    }

    public abstract bool IsTrue();
    protected abstract void ParseFrom(JSONNode jsonData);

    public Character GetOwner()
    {
        return ownerProc.owner.GetOwnerCharacter();
    }
}
