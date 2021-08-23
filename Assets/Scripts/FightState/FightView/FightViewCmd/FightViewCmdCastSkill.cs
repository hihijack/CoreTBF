using DefaultNamespace;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// 释放技能
/// </summary>
public class FightViewCmdCastSkill : FightViewCmdBase
{
    List<GameObject> _lstEffectsCache;
    private readonly Character caster;
    private readonly List<Character> targets;
    private readonly Skill skill;
    private readonly bool isPowStartAct;

    public FightViewCmdCastSkill(Character caster, List<Character> targets, Skill skill, bool isPowStartAct)
    {
        _lstEffectsCache = new List<GameObject>(10);
        this.caster = caster;
        this.targets = targets;
        this.skill = skill;
        this.isPowStartAct = isPowStartAct;
    }

    public override void Play()
    {
        base.Play();
        //播放TimeLine
        TimelineAsset tlAssetToPlay;

        if (isPowStartAct)
        {
            //蓄力表现
            tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skill.GetBaseData().tlAssetPower}");
        }
        else
        {
            tlAssetToPlay = Resources.Load<TimelineAsset>($"TimeLines/{skill.GetBaseData().tlAsset}");
        }

        if (tlAssetToPlay != null)
        {
            //UI关闭AI提示显示
            UIFight.Inst.SetAIItemsVisible(false);
            FightState.Inst.fightViewBehav.Play(tlAssetToPlay);
        }
        else
        {
            //防御表现
            if (skill.GetBaseData().logic == Data.ESkillLogic.Def)
            {
                caster.entityCtl.SetSprite("def");
            }
            End();
        }
    }

    public override void OnPlayableEventLogic(EEventLogic enentType)
    {
        switch (enentType)
        {
            case EEventLogic.End:
                End();
                break;
            case EEventLogic.RealAct:
                //执行子命令
                if (lstChildrenCmd != null)
                {
                    foreach (var cmd in lstChildrenCmd)
                    {
                        cmd.Play();
                    }
                }
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

    public override void OnPlayableSetSprite(PBParamSetCharacterSprite param)
    {
        if (param.target == ETargetType.Atker)
        {
            var taretEntity = caster.entityCtl;
            taretEntity.SetSprite(param.spriteName);
        }
        else if (param.target == ETargetType.Targets)
        {
            var targetCharacters = targets;
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

    public override void OnPlayableCreateEff(PBParamCreateEff param)
    {
        if (param.target == ETargetType.Atker)
        {
            Character targetCharacter = caster;
            var goEff = EffectMgr.CreateEffForUnit(targetCharacter.entityCtl, new ParamEffectCreate()
            { effName = param.effName, offsetPos = param.posOffset, bind = param.bind });
            _lstEffectsCache.Add(goEff);
        }
        else if (param.target == ETargetType.Targets)
        {
            var targetsCharacters = targets;
            foreach (var targetCharacter in targetsCharacters)
            {
                var goEff = EffectMgr.CreateEffForUnit(targetCharacter.entityCtl, new ParamEffectCreate()
                { effName = param.effName, offsetPos = param.posOffset, bind = param.bind });
                _lstEffectsCache.Add(goEff);
            }
        }
    }

    public override void OnPlayableMoveCharacter(PBParamMoveCharacter param)
    {
        if (param.targetType == ETargetType.Atker)
        {
            Character targetCharacter = caster;
            FightState.Inst.fightViewBehav.MoveACharacter(targetCharacter, param, 1);
        }
        else if (param.targetType == ETargetType.Targets)
        {
            var targetsCharacters = FightState.Inst.GetCurTargets();
            int index = 0;
            foreach (var targetCharacter in targetsCharacters)
            {
                index++;
                FightState.Inst.fightViewBehav.MoveACharacter(targetCharacter, param, index);
            }
        }
    }
}
