using System;
using System.Collections.Generic;

public static class FightLogicUtil
{
    public static void InitSkillHitInfo(ActionContent actionContent)
    {
        var hitInfos = new bool[actionContent.targets.Count];
        for (int i = 0; i < actionContent.targets.Count; i++)
        {
            var target = actionContent.targets[i];
            bool isHit = CalSkillHit(actionContent.caster, target, actionContent.skill);
            hitInfos[i] = isHit;
        }
        actionContent.hitInfos = hitInfos;
    }

    private static bool CalSkillHit(Character caster, Character target, Skill skill)
    {
        //防御中不触发闪避
        if (target.State == ECharacterState.Def)
        {
            return true;
        }

        //TODO 被连击中不触发闪避

        //
        var odds = skill.GetBaseData().odds / 100f * (1 - target.propData.Dodge / 100f);
        return odds > UnityEngine.Random.Range(0f, 1f);
    }
}
