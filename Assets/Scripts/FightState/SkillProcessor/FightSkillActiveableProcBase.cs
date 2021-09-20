using SimpleJSON;
using System;
using System.Collections.Generic;

public abstract class FightSkillActiveableProcBase : FightSkillProcessorBase
{
    public FightSkillActiveableProcBase(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {
    }

    public abstract SkillProcResult UnProc(ActionContent content);
}
