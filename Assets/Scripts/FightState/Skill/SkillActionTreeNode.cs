using System;
using System.Collections.Generic;

public class SkillActionTreeNode 
{
    ActionContent actoinContent;
    FightSkillProcessorBase proc;

    public SkillActionTreeNode(FightSkillProcessorBase proc, ActionContent actoinContent)
    {
        this.actoinContent = actoinContent;
        this.proc = proc;
    }

    public ActionContent Content
    {
        get
        {
            return this.actoinContent;
        }
    }
}
