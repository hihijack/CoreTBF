using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.FightStages;
using UI;
using UnityEditorInternal;

public enum EFightStage
{
    None, //空
    Normal, //常态
    ActionSelect,//行动选择
    ActionReady,//准备阶段,其他人行动选择
    ActionAct,//行动生效阶段
    ActionEnd//行动结束阶段
}

[System.Serializable]
public class FightState : GameStateBase
{
    public Camera cameraMain;

    public GameObject unitRoot;
    public GameObject[] pointsAlly;
    public GameObject[] pointsEnemy;
    public GameObject[] pointsAllyCloseUp;
    public GameObject[] pointsEnemyCloseUP;

    public PlayableDirector director;

    public PostProcessVolume ppv;

    private EFightStage mCurStage = EFightStage.None;

    public Dictionary<EFightStage, FightStageBase> mDicFightStages;
    private Character _activeCharacter;

    public List<FightActionBase> lstActionData;

    public FightViewBehav fightViewBehav;

    public FightCharacterMgr characterMgr;

    public static FightState Inst { get; private set; }

    /// <summary>
    /// 获取站位位置
    /// </summary>
    /// <param name="camp"></param>
    /// <param name="teamLoc"></param>
    /// <returns></returns>
    public Vector3 GetPosByTeamLoc(ECamp camp, int teamLoc)
    {
        if (camp == ECamp.Ally)
        {
            return pointsAlly[teamLoc - 1].transform.position;
        }
        else if (camp == ECamp.Enemy)
        {
            return pointsEnemy[teamLoc - 1].transform.position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 当一个角色死亡
    /// </summary>
    /// <param name="character"></param>
    internal void OnCharacterDead(Character character)
    {
        //更新其他同队角色teamloc
        //之后的角色teamLoc - 1
        characterMgr.ChangeTeamLocOnSomeOneDie(character);
    }

    /// <summary>
    /// 当尝试立即行动
    /// </summary>
    public void BtnActionAtOnce()
    {
        //是否有等待中
        var t = characterMgr.GetFirstAllyCanAction();
        if (t != null)
        {
            //取一个角色进入激活状态
            t.ActiveAction();
            SetFightStage(EFightStage.ActionSelect);
        }
    }

    private void InitFightStages()
    {
        mDicFightStages = new Dictionary<EFightStage, FightStageBase>
        {
            {EFightStage.Normal, new FightStageNormal()},
            {EFightStage.ActionSelect, new FightStageActionSelect()},
            {EFightStage.ActionReady, new FightStageActionReady()},
            {EFightStage.ActionAct, new FightStageActionAct()},
            {EFightStage.ActionEnd, new FightStageActionEnd()}
        };
    }


    public void SetFightStage(EFightStage stage)
    {
        Debug.Log("SetFightStage:" + stage);//#######
        var oriStage = mCurStage;
        mCurStage = stage;
        if (oriStage != mCurStage)
        {
            GetFighstStage(oriStage)?.OnExit();
            CurFightStage?.OnEnter();
        }
    }

    private FightStageBase CurFightStage
    {
        get { return GetFighstStage(mCurStage); }
    }

    public FightStageBase GetFighstStage(EFightStage stage)
    {
        return mDicFightStages.ContainsKey(stage) ? mDicFightStages[stage] : null;
    }

    /// <summary>
    /// 当前进入触发角色                
    /// </summary>
    /// <param name="character"></param>
    public void SetActiveCharacte(Character character)
    {
        _activeCharacter = character;
    }
    /// <summary>
    /// 获取当前触发角色
    /// </summary>
    /// <returns></returns>
    public Character GetActiveCharacter()
    {
        return _activeCharacter;
    }

    /// <summary>
    /// 回合结束
    /// </summary>
    public void RoundEnd()
    {
        characterMgr.ClearActedLst();
        ClearAction();
        SetActiveCharacte(null);
        SetFightStage(EFightStage.Normal);
        //AI行动刷新
        UIFight.Inst.RefreshAIItems();
    }


    /// <summary>
    /// 缓存一个action,在行动阶段执行
    /// </summary>
    /// <param name="character"></param>
    /// <param name="skillData"></param>
    public void CaheAction(FightActionBase action)
    {

        Debug.Log("CacheAction:" + action.caster.roleData.name + "," + action.skill.name);//###########

        if (action == null)
        {
            Debug.LogError("Cache A Null Action");//#####
            return;
        }

        for (int i = lstActionData.Count - 1; i >= 0; i--)
        {
            if (lstActionData[i].caster == action.caster)
            {
                lstActionData.RemoveAt(i);
                break;
            }
        }

        lstActionData.Add(action);
        UIFightActionRoot.Inst.MarkSelectedLstChanged();
    }

   

    /// <summary>
    /// 清理Action缓存
    /// </summary>
    public void ClearAction()
    {
        lstActionData.Clear();
    }
    

    public bool IsInStage(EFightStage stage) => mCurStage == stage;

    /// <summary>
    /// 是否跳过Ready阶段:行动方敌方没有速攻反击的跳过
    /// </summary>
    /// <returns></returns>
    public bool IsSkipReadyStage
    {
        get
        {
            ECamp enemyCamp = _activeCharacter.GetEnemyCamp();
            return characterMgr.CheckSkipReadyStage(enemyCamp);
        }
    }


    /// <summary>
    /// 是否跳过结束阶段:回合触发为友军或没有处于ready状态的人
    /// </summary>
    /// <returns></returns>
    public bool IsSkipEndStage
    {
        get
        {
            bool r = true;
            if (GetActiveCharacter().camp == ECamp.Enemy)
            {
                r =  characterMgr.CheckSkipEndStage();
            }
            return r;
        }
    }


    /// <summary>
    /// 是否敌人的仇恨目标
    /// </summary>
    /// <param name="charater"></param>
    /// <returns></returns>
    //public bool IsHatredTarget(Character charater)
    //{
    //    bool r = false;
    //    foreach (var chr in lstCharacters)
    //    {
    //        if (chr.camp == ECamp.Enemy && chr.IsAlive() && chr.ai.GetHatredTarget() == charater)
    //        {
    //            r = true;
    //            break;
    //        }
    //    }
    //    return r;
    //}

    /// <summary>
    /// 进入下一个阶段
    /// </summary>
    public void ToNextStage()
    {
        Debug.Log("ToNextState,curState = " + mCurStage);//#########
        if (IsInStage(EFightStage.ActionSelect))
        {
            //if (lstActionData.Count == 0) return;
            ////进入Ready状态
            //SetFightStage(EFightStage.ActionReady);
            SetFightStage(EFightStage.ActionAct);
        }
        //else if (IsInStage(EFightStage.ActionReady))
        //{
        //    //进入行动阶段
        //   SetFightStage(EFightStage.ActionAct);
        //}
        else if (IsInStage(EFightStage.ActionAct))
        {
            if (characterMgr.HasAllyToSelectAction())
            {
                SetFightStage(EFightStage.ActionSelect);
            }
            else
            {
                //回合结束
                SetFightStage(EFightStage.ActionEnd);
            }
        }

        else if (IsInStage(EFightStage.ActionEnd))
        {
            if (lstActionData.Count == 0)
            {
                //回合结束
                RoundEnd();
            }
            else
            {
                //进入行动阶段
                SetFightStage(EFightStage.ActionAct);
            }
        }
    }

    public Character GetCurCaster()
    {
        return FightStageActionAct.curAction.caster;
    }

    public List<Character> GetCurTargets()
    {
        return FightStageActionAct.curAction.targets;
    }


    public RoleEntityCtl GetAtkerEntityCtl()
    {
        return FightStageActionAct.curAction.caster.entityCtl;
    }

    public List<RoleEntityCtl> GetTargetsEntityCtl()
    {
        List<RoleEntityCtl> r = new List<RoleEntityCtl>();
        foreach (var t in FightStageActionAct.curAction.targets)
        {
            r.Add(t.entityCtl);
        }
        return r;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="camp"></param>
    /// <param name="index">从1开始</param>
    /// <returns></returns>
    public Vector3 GetCloseUpPos(ECamp camp, int index)
    {
        if (camp == ECamp.Ally)
        {
            return pointsAllyCloseUp[index - 1].transform.position;
        }
        else if (camp == ECamp.Enemy)
        {
            return pointsEnemyCloseUP[index - 1].transform.position;
        }
        return Vector3.zero;
    }

    public Vector3 GetOriPos(Character character)
    {
        if (character.camp == ECamp.Ally)
        {
            return pointsAlly[character.teamLoc - 1].transform.position;
        }
        else if (character.camp == ECamp.Enemy)
        {
            return pointsEnemy[character.teamLoc - 1].transform.position;
        }
        return Vector3.zero;
    }

    public override void OnEnter()
    {
        unitRoot = GameObject.FindGameObjectWithTag("UnitRoot");
        characterMgr.SetUnitRoot(unitRoot);
        characterMgr.AddCharacter(1, ECamp.Ally);
        characterMgr.AddCharacter(2, ECamp.Ally);
        characterMgr.AddCharacter(3, ECamp.Ally);
        characterMgr.AddCharacter(4, ECamp.Enemy);

        InitFightStages();

        //初始化战斗UI
        var uiFight = UIMgr.Inst.ShowUI(UITable.EUITable.UIFight);
        //刷新血量UI
        var uiHPRoot = UIMgr.Inst.ShowUI(UITable.EUITable.UIHPRoot) as UIHPRoot;
        var lstCharacters = characterMgr.GetCharacters();
        foreach (var character in lstCharacters)
        {
            uiHPRoot.RefreshTarget(character);
        }

        //玩家属性数据
        PlayerRolePropDataMgr.Inst.Init();

        //战斗日志UI
        UIMgr.Inst.ShowUI(UITable.EUITable.UIFightLog);

        //初始化完毕,进入Normal状态
        SetFightStage(EFightStage.Normal);
    }

    public override void OnUpdate()
    {
        CurFightStage.OnUpdate();
        fightViewBehav.DoUpate();
    }

    public override void OnExit()
    {
       
    }

    public override void Init()
    {
        if (!hasInit)
        {
            Inst = this;
            characterMgr = new FightCharacterMgr();
            lstActionData = new List<FightActionBase>();
            //战斗视图初始化
            fightViewBehav = new FightViewBehav(director, ppv);
        }
        hasInit = true;
    }
}