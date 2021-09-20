using System;
using System.Collections.Generic;

/// <summary>
/// 技能触发次数管理
/// </summary>
public class SkillProcTimeMgr
{
    struct SkillProcTimeRecorder
    {
        public int skillID;
        public int time;
    }

    Dictionary<int, SkillProcTimeRecorder> m_dicRecorder;

    public SkillProcTimeMgr()
    {
        m_dicRecorder = new Dictionary<int, SkillProcTimeRecorder>();
    }

    internal void OnSkillProc(Skill skill)
    {
        if (skill.GetBaseData().timeLimit > 0)
        {
            int id = skill.GetBaseData().ID;
            if (m_dicRecorder.ContainsKey(id))
            {
                var t = m_dicRecorder[id];
                t.time++;
            }
            else
            {
                m_dicRecorder.Add(id, new SkillProcTimeRecorder() { skillID = id, time = 1 });
            }
        }
    }

    public bool CheckTimeLimit(Skill skill)
    {
        var timeLimit = skill.GetBaseData().timeLimit;
        if (timeLimit > 0)
        {
            int curTime = 0;
            int id = skill.GetBaseData().ID;
            if (m_dicRecorder.ContainsKey(id))
            {
                curTime = m_dicRecorder[id].time;
            }
            return curTime < timeLimit;
        }
        else
        {
            return true;
        }
    }
}
