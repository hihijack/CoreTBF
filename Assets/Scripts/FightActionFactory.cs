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
            _dicActions = new Dictionary<int, FightActionBase>();
            _dicActions.Add(1, new FightActionWait());
            _dicActions.Add(2, new FightActionDef());
            var atks = new[]{3,4,6,7,9};
            var actionAtk = new FightActionAtk();
            foreach (var t in atks)
            {
                _dicActions.Add(t, actionAtk);
            }
        }
        #endregion
        
        public FightActionBase CreateFightAction(Character caster, SkillData skill, List<Character> targets)
        {
            switch (skill.logic)
            {
                case ESkillLogic.Wait:
                    return new FightActionWait {caster = caster,skill = skill, targets = targets};
                case ESkillLogic.Def:
                    return new FightActionDef {caster = caster,skill = skill, targets = targets};
                case ESkillLogic.Atk:
                    return new FightActionAtk {caster = caster,skill = skill, targets = targets};
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}