using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using DefaultNamespace;
using DG.Tweening;
using DefaultNamespace.FightStages;
using UnityEngine.Timeline;
using System.Collections.Generic;
using System;

public class FightViewBehav 
{
    TimeLineCtl _timeLineCtl;
    PostProcessVolume _ppv;

    GameObject _goEffRoot;

    List<FightViewCmdBase> _lstCmdCache;//表现命令缓存

    Queue<FightViewCmdBase> _queueViewCmd;

    Action onViewPlayComplete;

    FightViewCmdBase _curRunningCmd;

    public FightViewBehav(PlayableDirector director, PostProcessVolume ppv)
    {
        _timeLineCtl = new TimeLineCtl(director);
        _lstCmdCache = new List<FightViewCmdBase>();
        _queueViewCmd = new Queue<FightViewCmdBase>();
        _ppv = ppv;
    }

    internal bool HasCmdCached()
    {
        return _lstCmdCache.Count > 0;
    }

    public TimeLineCtl TimeLineCtl
    {
        get
        {
            return _timeLineCtl;
        }
    }

    GameObject GoEffRoot
    {
        get
        {
            if (_goEffRoot == null)
            {
                _goEffRoot = new GameObject("_EffRoot");
            }
            return _goEffRoot;
        }
    }

    public void DoUpate()
    {
        if (_curRunningCmd != null)
        {
            _curRunningCmd.Update(Time.deltaTime);
        }
    }

    /// <summary>
    /// 缓存表现命令
    /// </summary>
    /// <param name="cmd"></param>
    public void CacheViewCmd(FightViewCmdBase cmd)
    {
        Debug.Log("t>>ViewCmd:" + cmd);
        _lstCmdCache.Add(cmd);
    }

    /// <summary>
    /// 开始播放缓存表现命令
    /// </summary>
    public void StartPlayCachedViewCmd(Action onComplete)
    {
        this.onViewPlayComplete = onComplete;
        PreHandleViewCmd();
        PlayNextCmd();
    }

    private void PlayNextCmd()
    {
        if (_queueViewCmd.Count > 0)
        {
            var cmd = _queueViewCmd.Dequeue();
            cmd.SetEndCB(OnOneViewCmdComplete);
            _curRunningCmd = cmd;
            cmd.Play();
        }
        else
        {
            _curRunningCmd = null;
            OnViewCmdComplete();
        }
    }

    void OnOneViewCmdComplete(FightViewCmdBase cmd)
    {
        PlayNextCmd();
    }

    /// <summary>
    /// 所有表现完成
    /// </summary>
    void OnViewCmdComplete()
    {
        ClearViewCmd();
        onViewPlayComplete?.Invoke();
    }

    /// <summary>
    /// 预处理表现命令
    /// </summary>
    private void PreHandleViewCmd()
    {
        FightViewCmdBase lastSkillCastCmd = null; 
        foreach (var cmd in _lstCmdCache)
        {
            if (cmd.GetType() == typeof(FightViewCmdCastSkill))
            {
                lastSkillCastCmd = cmd;
            }
            if (cmd.GetType() == typeof(FightViewCmdHPChanged) || cmd.GetType() == typeof(FightViewCmdTenacityChange))
            {
                if (lastSkillCastCmd != null)
                {
                    lastSkillCastCmd.AddChildCmd(cmd);
                }
                else
                {
                    _queueViewCmd.Enqueue(cmd);
                }
            }
            else
            {
                _queueViewCmd.Enqueue(cmd);
            }
        }
    }

    void ClearViewCmd()
    {
        _lstCmdCache.Clear();
        _queueViewCmd.Clear();
    }


    public void Play(TimelineAsset asset)
    {
        TimeLineCtl.Play(asset);
    }

    internal void OnPlayableSetDepthOfFieldEnable(bool enable)
    {
        _ppv.weight = enable ? 1 : 0;
    }

    internal void OnPlayableEventLogic(EEventLogic enentType)
    {
        if (_curRunningCmd != null)
        {
            _curRunningCmd.OnPlayableEventLogic(enentType);
        }
    }

    internal void OnPlayableSetSprite(PBParamSetCharacterSprite param)
    {
        if (_curRunningCmd != null)
        {
            _curRunningCmd.OnPlayableSetSprite(param);
        }
    }

    internal void OnPlayableCreateEff(PBParamCreateEff param)
    {
        if (_curRunningCmd != null)
        {
            _curRunningCmd.OnPlayableCreateEff(param);
        }
    }

    internal void OnPlayableMoveCharacter(PBParamMoveCharacter param)
    {
        if (_curRunningCmd != null)
        {
            _curRunningCmd.OnPlayableMoveCharacter(param);
        }
    }

    /// <summary>
    /// 根据参数和序号移动一个角色
    /// </summary>
    /// <param name="target"></param>
    /// <param name="param"></param>
    /// <param name="index">从1开始</param>
    public void MoveACharacter(Character targetCharacter, PBParamMoveCharacter param, int index)
    {
        Vector3 pos = Vector3.zero;
        if (param.pointType == EPointType.CloseUp)
        {
            pos = FightState.Inst.GetCloseUpPos(targetCharacter.camp, index) + param.offsetPos * targetCharacter.entityCtl.face;
        }
        else if (param.pointType == EPointType.OriPos)
        {
            pos = FightState.Inst.GetOriPos(targetCharacter);
        }
        if (param.atonce)
        {
            //立即设置位置
            targetCharacter.entityCtl.SetPos(pos);
        }
        else
        {
            //缓动移动
            targetCharacter.entityCtl.transform.DOMove(pos, param.dur);
        }
    }

    internal void Clear()
    {
        
    }
}
