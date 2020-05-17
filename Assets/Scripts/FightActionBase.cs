using System;
using System.Collections.Generic;
using Data;
using UI;

namespace DefaultNamespace
{
    public class FightActionBase
    {
        public Character caster;
        public SkillData skill;
        public List<Character> targets;

        public Action actionEnd;
        
        public virtual void Act()
        {
            UIMgr.Inst.uiFightLog.AppendLog($"{caster.roleData.name}发动了{skill.name}");
        }
    }
}