using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 直接执行伤害。不考虑防御
/// </summary>
internal class FightSkillProcHurted : FightSkillProcessorBase
{
    int val;
    public FightSkillProcHurted(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override SkillProcResult Proc(ActionContent content)
    {
        var targets = GetTargets(content);

        foreach (var target in targets)
        {
            bool isHit = IsHitTarget(content, target);
            if (isHit)
            {
                var oriHP = target.propData.hp;
                var result = target.HurtSelf(new DmgData() { val = val });
                var curHP = target.propData.hp;

                Debug.Log($"t[{Time.frameCount}]>>添加更新血条命令,{curHP}");//############

                FightState.Inst.eventRecorder.CacheEvent(new FightEventHPHurted(target, oriHP, curHP, result));

                if (result.dmg > 0 && target.IsAlive())
                {
                    target.OnHurtd(content);
                }
                target.HandleHPState(content);
            }
            else
            {
                FightState.Inst.eventRecorder.CacheEvent(new FightEventDodge(target));
            }
        }

        return new SkillProcResult() { };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        val = jsonData["val"].AsInt;
    }
}