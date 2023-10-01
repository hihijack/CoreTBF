using System;
using System.Collections.Generic;

public enum ESkillProcState
{
    Ready,
    Linking
}

public class SkillProcLinkHandler
{
    Queue<ActionContent> queueContent;

    ESkillProcState state;

    public SkillProcLinkHandler()
    {
        queueContent = new Queue<ActionContent>();
    }

    /// <summary>
    /// 处理一个主动行为
    /// </summary>
    /// <param name="contentRoot"></param>
    internal void ActiveProc(ActionContent contentRoot)
    {
        if (state == ESkillProcState.Ready)
        {
            StartProcActionContentLink(contentRoot);
            EndProcLink();
        }
    }

    private void EndProcLink()
    {
        ClearContentQueue();
        state = ESkillProcState.Ready;
    }

    private void StartProcActionContentLink(ActionContent contentRoot)
    {
        state = ESkillProcState.Linking;
        ClearContentQueue();
        if (contentRoot.skill != null)
        {
            contentRoot.skill.Proc(contentRoot);
        }
        else if (contentRoot.buff != null)
        {
            contentRoot.buff.PassiveProc(contentRoot);
        }
        PassiveProcNext();
    }

    void ClearContentQueue()
    {
        queueContent.Clear();
    }

    private void PassiveProcNext()
    {
        if (queueContent.Count > 0)
        {
            var nextContent = queueContent.Dequeue();
            PassiveProc(nextContent);
            PassiveProcNext();
        }
    }

    /// <summary>
    /// 处理一个被动content
    /// </summary>
    /// <param name="nextContent"></param>
    private void PassiveProc(ActionContent nextContent)
    {
        if (nextContent.skill != null)
        {
            nextContent.skill.PassiveProc(nextContent);
        }
        else if (nextContent.buff != null)
        {
            nextContent.buff.PassiveProc(nextContent);
        }
    }

    internal void EnqueueNode(ActionContent content)
    {
        if (state == ESkillProcState.Ready)
        {
            StartProcActionContentLink(content);
            EndProcLink();
        }
        else if (state == ESkillProcState.Linking)
        {
            queueContent.Enqueue(content);
        }
    }
}
