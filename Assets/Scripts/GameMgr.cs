using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DefaultNamespace;
using DefaultNamespace.FightStages;
using UI;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

public enum EFightStage
{
    None, //空
    Normal, //常态
    ActionSelect,//行动选择
    ActionReady,//准备阶段,其他人行动选择
    ActionAct,//行动生效阶段
    ActionEnd//行动结束阶段
}

public class GameMgr : MonoBehaviour
{
    #region 单例
    private static GameMgr _inst;
    
    public static GameMgr Inst
    {
        get { return _inst; }
    }
    #endregion

    public Camera cameraMain;

    public GameObject unitRoot;
    public GameObject[] pointsAlly;
    public GameObject[] pointsEnemy;
    public GameObject[] pointsAllyCloseUp;
    public GameObject[] pointsEnemyCloseUP;

    public PlayableDirector director;

    public PostProcessVolume ppv;

    public int[] allyIDS;
    public int enemyID;

    public List<Character> lstCharacters;

    public EFightStage mCurStage = EFightStage.None;

    public Dictionary<EFightStage, FightStageBase> mDicFightStages;
    private Character _activeCharacter;

    public List<FightActionBase> lstActionData;

    private List<Character> lstChrActed = new List<Character>();//缓存已行动的角色

    public FightViewBehav fightViewBehav;

    private void Awake()
    {
        _inst = this;
        lstActionData = new List<FightActionBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lstCharacters = new List<Character>();

        var charaEnemy = new Character(enemyID);
        charaEnemy.teamLoc = 1;
        charaEnemy.camp = ECamp.Enemy;
        charaEnemy.entityCtl.transform.SetParent(unitRoot.transform);
        charaEnemy.entityCtl.transform.position = GetPosByTeamLoc(charaEnemy.camp,charaEnemy.teamLoc);
        charaEnemy.entityCtl.face = -1;

        int teamLoc = 0;
        foreach (var allyID in allyIDS)
        {
            teamLoc++;
            var chara = new Character(allyID);
            chara.teamLoc = teamLoc;
            chara.camp = ECamp.Ally;
            chara.entityCtl.transform.SetParent(unitRoot.transform);
            chara.entityCtl.transform.position = GetPosByTeamLoc(ECamp.Ally, chara.teamLoc);
            chara.entityCtl.face = 1;
            //todo 设置目标
            chara.target = charaEnemy;
            lstCharacters.Add(chara);
        }

       
        
        lstCharacters.Add(charaEnemy);
        
        InitFightStages();
        
        //初始化完毕,进入Normal状态
        SetFightStage(EFightStage.Normal);
        
        //初始化UI
        UIMgr.Inst.Init();
        
        //玩家属性数据
        PlayerRolePropDataMgr.Inst.Init();

        //刷新血量UI
        foreach (var character in lstCharacters)
        {
            UIMgr.Inst.uiHPRoot.RefreshTarget(character);
        }

        //战斗视图初始化
        fightViewBehav = new FightViewBehav(director, ppv);

        //TODO 仇恨目标
        //charaEnemy.ai.AddHatred(lstCharacters[0], 1);
    }

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

    // Update is called once per frame
    private void Update()
    {
        CurFightStage.OnUpdate();
        fightViewBehav.DoUpate();
    }

    /// <summary>
    /// 当尝试立即行动
    /// </summary>
    public void BtnActionAtOnce()
    {
        //是否有等待中
        var t = GetFirstAllyCanAction();
        if (t != null)
        {
            //取一个角色进入激活状态
            t.ActiveAction();
            GameMgr.Inst.SetFightStage(EFightStage.ActionSelect);
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
        get { return GetFighstStage(mCurStage);}
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
        ClearActedLst();
        ClearAction();
        SetActiveCharacte(null);
        SetFightStage(EFightStage.Normal);
        //AI行动刷新
        UIMgr.Inst.uiFight.RefreshAIItems();
    }

    /// <summary>
    /// 取指定站位的一个友军
    /// </summary>
    /// <param name="targetTeamLoc"></param>
    /// <returns></returns>
    internal Character GetAllyCharacterByTeamLoc(int teamLoc)
    {
        foreach (var character in lstCharacters)
        {
            if (character.camp == ECamp.Ally && character.teamLoc == teamLoc && character.IsAlive())
            {
                return character;
            }
        }
        return null;
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
        UIMgr.Inst.uiFightActionRoot.MarkSelectedLstChanged();
    }

    /// <summary>
    /// 取随机一个友军
    /// </summary>
    /// <returns></returns>
    internal Character GetARandomAllyCharacter()
    {
        var teamLoc = UnityEngine.Random.Range(1, 4);
        return GetAllyCharacterByTeamLoc(teamLoc);
    }

    /// <summary>
    /// 清理Action缓存
    /// </summary>
    public void ClearAction()
    {
        lstActionData.Clear();
    }

    /// <summary>
    /// 取指定阵营的所有角色
    /// </summary>
    /// <param name="camp"></param>
    /// <returns></returns>
    public List<Character> GetCharactersOfCamp(ECamp camp)
    {
        var lst = new List<Character>();
        foreach (var t in lstCharacters)
        {
            if (t.camp == camp)
            {
                lst.Add(t);
            }
        }
        return lst;
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
            bool r = true;
            ECamp enemyCamp = _activeCharacter.GetEnemyCamp();
            foreach (var t in lstCharacters)
            {
                if (t.camp == enemyCamp && t.IsEnableAction && t.HasQuickSkill() && t.IsInReady())
                {
                    r = false;
                    break;
                }
            }
            return r;
        }
    }


    /// <summary>
    /// 添加到已行动缓存
    /// </summary>
    /// <param name="chr"></param>
    public void CacheToActed(Character chr)
    {
        lstChrActed.Add(chr);
    }

    /// <summary>
    /// 清理已行动缓存
    /// </summary>
    public void ClearActedLst()
    {
        lstChrActed.Clear();
    }

    /// <summary>
    /// 是否已行动
    /// </summary>
    /// <param name="chr"></param>
    /// <returns></returns>
    public bool HasActed(Character chr)
    {
        return lstChrActed.Contains(chr);
    }

    /// <summary>
    /// 还有行动者友军可以选择行动
    /// </summary>
    /// <returns></returns>
    public bool HasAllyToSelectAction()
    {
        bool r = false;
        foreach (var t in lstCharacters)
        {
            if (t.camp == GetActiveCharacter().camp && t.IsEnableAction && t.IsInReady() && !lstChrActed.Contains(t))
            {
                r = true;
                break;
            }
        }
        return r;
    }

    /// <summary>
    /// 取一个可以选择行动的角色
    /// </summary>
    /// <returns></returns>
    public Character GetFirstAllyCanAction()
    {
        Character r = null;
        foreach (var t in lstCharacters)
        {
            if (t.camp == ECamp.Ally && t.IsEnableAction && t.IsInReady() && !lstChrActed.Contains(t))
            {
                r = t;
                break;
            }
        }
        return r;
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
                foreach (var t in lstCharacters)
                {
                    if (t.IsEnableAction && t.IsInReady())
                    {
                        r = false;
                        break;
                    }
                }
            }
            return r;
        }
    }


    /// <summary>
    /// 当一个角色死亡
    /// </summary>
    /// <param name="character"></param>
    internal void OnCharacterDead(Character character)
    {
        //更新其他同队角色teamloc
        //之后的角色teamLoc - 1
        foreach (var chr in lstCharacters)
        {
            if (chr.camp == character.camp && chr != character && chr.teamLoc > character.teamLoc )
            {
                chr.teamLoc--;
            }
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
            if (HasAllyToSelectAction())
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
}
