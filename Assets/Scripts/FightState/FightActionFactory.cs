using System;
using System.Collections.Generic;
using Data;

namespace DefaultNamespace
{
    public class FightActionFactory
    {
        private Dictionary<int, FightActionBase> _dicActions;
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
        
        public FightActionBase CreateFightAction(Character caster, SkillBaseData skill, List<Character> targets)
        {
            switch (skill.logic)
            {
                case ESkillLogic.Wait:
                    return new FightActionWait(caster,skill,targets);
                case ESkillLogic.Def:
                    return new FightActionDef (caster, skill, targets);
                case ESkillLogic.Atk:
                    return new FightActionAtk(caster, skill, targets);
                case ESkillLogic.ExchangeLoc:
                    return new FightActionExchangeLoc(caster, skill, targets);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}