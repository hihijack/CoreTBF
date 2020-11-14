using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using System;
using UnityEngine.Rendering.PostProcessing;
using DefaultNamespace;
using DG.Tweening;
using Boo.Lang;
using DefaultNamespace.FightStages;
using UI;
using UnityEngine.Timeline;

public class FightViewBehav 
{
    TimeLineCtl _timeLineCtl;
    PostProcessVolume _ppv;

    GameObject _goEffRoot;

    List<GameObject> _lstEffectsCache;

    public FightViewBehav(PlayableDirector director, PostProcessVolume ppv)
    {
        _timeLineCtl = new TimeLineCtl(director);
        _lstEffectsCache = new List<GameObject>(10);
        _ppv = ppv;
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
        switch (enentType)
        {
            case EEventLogic.End:
                FightState.Inst.OnEndPlayView();
                break;
            case EEventLogic.RealAct:
                FightStageActionAct.curAction.RealAct();
                break;
            case EEventLogic.RemoveEffects:
                //移除战斗特效
                foreach (var goEff in _lstEffectsCache)
                {
                    GoPool.Inst.Cache(goEff);
                }
                _lstEffectsCache.Clear();
                break;
            default:
                break;
        }
    }

    internal void OnPlayableSetSprite(PBParamSetCharacterSprite param)
    {
        if (param.target == ETargetType.Atker)
        {
            var taretEntity = FightState.Inst.GetAtkerEntityCtl();
            taretEntity.SetSprite(param.spriteName);
        }
        else if (param.target == ETargetType.Targets)
        {
            var targetCharacters = FightState.Inst.GetCurTargets();
            foreach (var character in targetCharacters)
            {
                if (character.State == ECharacterState.Power && param.spriteName != "power")
                {
                    continue;
                }
                if (((character.State == ECharacterState.Stiff || character.State == ECharacterState.Dying || character.State == ECharacterState.Dead) && param.spriteName == "hited") || param.spriteName != "hited")
                {
                    character.entityCtl.SetSprite(param.spriteName);
                }
            }
        }
    }

    internal void OnPlayableCreateEff(PBParamCreateEff param)
    {
        if (param.target == ETargetType.Atker)
        {
            Character targetCharacter = FightState.Inst.GetCurCaster();
            var goEff = EffectMgr.CreateEffForUnit(targetCharacter.entityCtl, new ParamEffectCreate() 
            { effName = param.effName, offsetPos = param.posOffset, bind = param.bind });
            _lstEffectsCache.Add(goEff);
        }
        else if (param.target == ETargetType.Targets)
        {
            var targetsCharacters = FightState.Inst.GetCurTargets();
            foreach (var targetCharacter in targetsCharacters)
            {
                var goEff = EffectMgr.CreateEffForUnit(targetCharacter.entityCtl, new ParamEffectCreate()
                { effName = param.effName, offsetPos = param.posOffset, bind = param.bind });
                _lstEffectsCache.Add(goEff);
            }
        }
    }

    internal void OnPlayableMoveCharacter(PBParamMoveCharacter param)
    {
        if (param.targetType == ETargetType.Atker)
        {
            Character targetCharacter = FightState.Inst.GetCurCaster();
            MoveACharacter(targetCharacter, param, 1);
        }
        else if (param.targetType == ETargetType.Targets)
        {
            var targetsCharacters = FightState.Inst.GetCurTargets();
            int index = 0;
            foreach (var targetCharacter in targetsCharacters)
            {
                index++;
                MoveACharacter(targetCharacter, param, index);
            }
        }
    }

    /// <summary>
    /// 根据参数和序号移动一个角色
    /// </summary>
    /// <param name="target"></param>
    /// <param name="param"></param>
    /// <param name="index">从1开始</param>
    void MoveACharacter(Character targetCharacter, PBParamMoveCharacter param, int index)
    {
        Vector3 pos = Vector3.zero; ;
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
