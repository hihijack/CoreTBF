using System;
using System.Collections.Generic;
using UnityEngine;

public class FightEventRecorder
{
    public List<FightEventBase> events;

    public FightEventRecorder()
    {
        events = new List<FightEventBase>();
    }

    public void CacheEvent(FightEventBase envetObj)
    {
        events.Add(envetObj);
    }

    private void Clear()
    {
        events.Clear();
    }

    internal void ParseToViewCmd(List<FightViewCmdBase> lstCmdCache)
    {
        FightViewCmdBase lastViewCmdSkillCast = null;
        for (int indexEvent = 0; indexEvent < events.Count; indexEvent++)
        {
            var fightEvent = events[indexEvent];
            var viewCmd = fightEvent.ParseToViewCmd();
            if (viewCmd == null)
            {
                continue;
            }
            if (fightEvent.GetType() == typeof(FightEventCastSkill))
            {
                lastViewCmdSkillCast = viewCmd;
                lstCmdCache.Add(viewCmd);
            }
            else
            {
                if (lastViewCmdSkillCast != null)
                {
                    lastViewCmdSkillCast.AddChildCmd(viewCmd);
                }
                else
                {
                    lstCmdCache.Add(viewCmd);
                }
            }

        }

        Clear();
    }
}
