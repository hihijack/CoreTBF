using System;
using System.Collections.Generic;
using UI;

namespace DefaultNamespace.FightStages
{
    public class FightStageActionAct : FightStageBase
    {
        public static FightActionBase curAction;
        
        Queue<FightActionBase> _queueAction = new Queue<FightActionBase>();
        private bool _idleFlag = false;
        
        public override void OnEnter()
        {
            base.OnEnter();

           // UIMgr.Inst.uiFightLog.AppendLog(">>>进入行动阶段");

            //            UIMgr.Inst.uiFight.uiFightAction.SetVisible(false);
            //技能按速度排序
            foreach (var fightActionData in FightState.Inst.lstActionData)
            {
                _queueAction.Enqueue(fightActionData);
            }
            _idleFlag = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_idleFlag)
            {
                if (_queueAction.Count > 0)
                {
                    var action = _queueAction.Dequeue();
                    var caster = action.actionContent.caster;
                    var skillData = action.skill.GetBaseData();
                    if (caster != null && caster.IsInReady())
                    {
                        _idleFlag = false;
                        action.actionActEnd = OnAActionEnd;
                        curAction = action;

                        //能量消耗
                        if (caster.camp == ECamp.Ally)
                        {
                            int cost = skillData.cost;
                            if (caster.mSkillPowering == action.skill)
                            {
                                //发动蓄力技能
                                cost = 0;
                            }
                            if (cost > 0)
                            {
                                var mpVal = PlayerRolePropDataMgr.Inst.propData.mp;
                                PlayerRolePropDataMgr.Inst.ChangeMP(-1 * cost);
                                FightState.Inst.eventRecorder.CacheEvent(new FightEventMPChanged(mpVal, -1 * cost));
                            }
                        }

                        //缓存到已行动列表
                        FightState.Inst.characterMgr.CacheToActed(caster);

                        action.Act();
                        
                        //仇恨计算
                        HandleHatred(action);
                    }
                }
                else
                {
                    //行动结束
                    FightState.Inst.ToNextStage();
                }
            }
        }

        /// <summary>
        /// 仇恨处理
        /// </summary>
        /// <param name="action"></param>
        private void HandleHatred(FightActionBase action)
        {
            //if (action.caster.camp == ECamp.Ally)
            //{
            //    if (action.skill.targetType == Data.ESkillTarget.Self || action.skill.targetType == Data.ESkillTarget.Ally)
            //    {
            //        //以自己或友军为目标的,所有敌人产生仇恨
            //        foreach (var chr in GameMgr.Inst.lstCharacters)
            //        {
            //            if (chr.camp == ECamp.Enemy && chr.IsAlive())
            //            {
            //                chr.ai.AddHatred(action.caster, action.skill.hatred);
            //            }
            //        }
            //    }
            //    else if (action.skill.targetType == Data.ESkillTarget.Enemy)
            //    {
            //        //技能目标产生仇恨
            //        foreach (var t in action.targets)
            //        {
            //            t.ai.AddHatred(action.caster, action.skill.hatred);
            //        }
            //    }
            //}
        }

        private void OnAActionEnd()
        {
            _idleFlag = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            curAction = null;
            _queueAction.Clear();
            FightState.Inst.ClearAction();
        }
    }
}