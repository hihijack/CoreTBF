using DefaultNamespace;
using System;
using System.Collections.Generic;

/// <summary>
/// 主动技能内容
/// </summary>
public class ActionContent : IActionContent
{
    public Character caster;
    public List<Character> targets;
    public Skill skill;
    public BuffBase buff;
    public string tri; //被动触发器
    public ActionContent contentTri;//触发的上下文

    public bool[] hitInfos;//是否命中

    internal int GetIndex(Character target)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == target)
            {
                return i;
            }
        }
        return -1;
    }
}
