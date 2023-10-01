using DefaultNamespace;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FightSkillProcDmgTarget : FightSkillProcessorBase
{
    public FightSkillProcDmgTarget(ISkillProcOwner owner, JSONNode jsonData, FightSkillConditionBase condition) : base(owner, jsonData, condition)
    {

    }

    public override SkillProcResult Proc(ActionContent content)
    {
        List<Character> targets = GetTargets(content);
        var selfCharacter = owner.GetOwnerCharacter();
        var skill = owner.GetOwnerSkill();
        var skillBaseData = skill.GetBaseData();
        //List<SkillProcResultNode> lstProcResult = new List<SkillProcResultNode>();
        foreach (var target in targets)
        {
            bool isHit = IsHitTarget(content, target);

            if (isHit)
            {
                var oriHP = target.propData.hp;
                var oriTenacity = target.propData.tenacity;
                var result = selfCharacter.DamageTarget(target, new DmgData() { dmgPrecent = skillBaseData.dmg, timeAtkStiff = skillBaseData.timeAtkStiff, tenAtk = skillBaseData.dmgTenacity });

                var curHP = target.propData.hp;
                var curTenacity = target.propData.tenacity;

                FightState.Inst.eventRecorder.CacheEvent(new FightEventHPHurted(target, oriHP, curHP, result));
                FightState.Inst.eventRecorder.CacheEvent(new FightEventTenHurted(target, oriTenacity, curTenacity - oriTenacity));

                Debug.Log($"Damage Prco:{selfCharacter.roleData.name}->{target.roleData.name}:{result.dmg}");//#########

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
            //lstProcResult.Add(new SkillProcResultNode() { target = target, isHit = isHit });
        }
        return new SkillProcResult() {  };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {

    }
}
