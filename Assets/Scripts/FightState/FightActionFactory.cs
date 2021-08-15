using System;
using System.Collections.Generic;
using Data;

namespace DefaultNamespace
{
    public class FightActionFactory
    {
        #region 单例
        private static FightActionFactory _inst;
        public static FightActionFactory Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new FightActionFactory();
                }
                return _inst;
            }
        }

        private FightActionFactory()
        {
           
        }
        #endregion
        
        public FightActionBase CreateFightAction(Skill skill, ActionContent content)
        {
            var skillData = skill.GetBaseData();
            switch (skillData.logic)
            {
                case ESkillLogic.Wait:
                    return new FightActionWait(skill, content);
                case ESkillLogic.Def:
                    return new FightActionDef (skill, content);
                case ESkillLogic.Atk:
                    return new FightActionAtk(skill, content);
                case ESkillLogic.ExchangeLoc:
                    return new FightActionExchangeLoc(skill, content);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ActionContent CreateActionContent(Character caster, Skill skill, List<Character> targets)
        {
            var content = new ActionContent();
            content.caster = caster;
            content.targets = targets;
            content.skill = skill;
            return content;
        }
    }
}