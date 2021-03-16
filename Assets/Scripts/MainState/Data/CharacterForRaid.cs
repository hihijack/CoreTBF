using Data;
using DefaultNamespace;
using System;
using System.Collections.Generic;

public enum ECharacterForRaidState
{
    Normal,
    Dying,
    Dead
}

public class CharacterForRaid
{
    public RoleBaseData roleData;
    public PropData propData;
    public ECharacterForRaidState state;
    public List<SkillBaseData> lstSkill;

    public CharacterForRaid(int tid)
    {
       roleData = RoleDataer.Inst.Get(tid);
       propData = new PropData();
       propData.MaxHP = roleData.hp;
       propData.hp = propData.MaxHP;

       lstSkill = new List<SkillBaseData>(8);
        //设置初始技能
        for (int i = 0; i < GameCfg.CHARA_SKILL_COUNT; i++)
        {
            if (i < roleData.skills.Length)
            {
                var skillData = SkillDataer.Inst.Get(roleData.skills[i]);
                lstSkill.Add(skillData);
            }
            else
            {
                lstSkill.Add(null);
            }
        }

       state = ECharacterForRaidState.Normal;
    }

    internal List<SkillBaseData> GetSkillList()
    {
        return lstSkill;
    }

    internal bool IsSkillEquiped(SkillBaseData skillData)
    {
        if (skillData == null)
        {
            return false;
        }
        else
        {
            return lstSkill.Contains(skillData);
        }
    }

    internal void SetSkill(int skillIndex, SkillBaseData skillData)
    {
        lstSkill[skillIndex] = skillData;
    }
}