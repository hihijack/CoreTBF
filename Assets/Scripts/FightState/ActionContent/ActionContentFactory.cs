using System;
using System.Collections.Generic;

public class ActionContentFactory
{
    public static ActionContent Create(Character caster, List<Character> targets, Skill skill)
    {
        var t = new ActionContent();
        t.caster = caster;
        t.targets = targets;
        t.skill = skill;
        return t;
    }

    public static ActionContent Create(Character caster, Skill skill, ActionContent triContent, string triKey)
    {
        var t = new ActionContent();
        t.caster = caster;
        t.skill = skill;
        t.contentTri = triContent;
        t.tri = triKey;
        return t;
    }

    public static ActionContent Create(Character caster, BuffBase buff, ActionContent triContent, string triKey)
    {
        var t = new ActionContent();
        t.buff = buff;
        t.caster = caster;
        t.contentTri = triContent;
        t.tri = triKey;
        return t;
    }
}
