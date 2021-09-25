using SimpleJSON;
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
            var oriHP = target.propData.hp;
            var result = target.HurtSelf(new DmgData() { val = val });
            var curHP = target.propData.hp;

            Debug.Log($"t[{Time.frameCount}]>>添加更新血条命令,{curHP}");//############
            FightState.Inst.fightViewBehav.CacheViewCmd(new FightViewCmdHPChanged(target, oriHP, curHP));

            if (result.dmg > 0 && target.IsAlive())
            {
                target.OnHurtd(content);
            }
            target.HandleHPState(content);
        }

        return new SkillProcResult() { targets = targets };
    }

    protected override void ParseFrom(JSONNode jsonData)
    {
        val = jsonData["val"].AsInt;
    }
}