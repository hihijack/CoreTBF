using System.Collections;
using System.Collections.Generic;
using Data;
using DefaultNamespace;
using DefaultNamespace.FightStages;
using UI;
using UnityEngine;
using UnityEngine.Playables;

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
    
    public GameObject unitRoot;
    public GameObject[] pointsAlly;
    public GameObject[] pointsEnemy;
    
    public int[] allyIDS;
    public int enemyID;

    public List<Character> lstCharacters;

    public EFightStage mCurStage = EFightStage.None;

    public Dictionary<EFightStage, FightStageBase> mDicFightStages;
    private Character _activeCharacter;

    public List<FightActionBase> lstActionData;
    
    private void Awake()
    {
        _inst = this;
        lstActionData = new List<FightActionBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lstCharacters = new List<Character>();
        int teamLoc = 0;
        foreach (var allyID in allyIDS)
        {
            teamLoc++;
            var chara = new Character(allyID);
            chara.teamLoc = teamLoc;
            chara.camp = ECamp.Ally;
            chara.entityCtl.transform.SetParent(unitRoot.transform);
            chara.entityCtl.transform.position = pointsAlly[teamLoc - 1].transform.position;
            lstCharacters.Add(chara);
        }

        var charaEnemy = new Character(enemyID);
        charaEnemy.teamLoc = 1;
        charaEnemy.camp = ECamp.Enemy;
        charaEnemy.entityCtl.transform.SetParent(unitRoot.transform);
        charaEnemy.entityCtl.transform.position = pointsEnemy[0].transform.position;
        //TODO 仇恨目标
        charaEnemy.target = lstCharacters[0];
        foreach (var t in lstCharacters)
        {
            t.target = charaEnemy;
        }
        
        lstCharacters.Add(charaEnemy);
        UIEnemyPlayerInfo.Inst.SetData(charaEnemy);
        
        InitFightStages();
        
        //初始化完毕,进入Normal状态
        SetFightStage(EFightStage.Normal);
        
        //初始化UI
        UIMgr.Inst.Init();
        
        FightViewBehav.Inst.Init();
        
        //玩家属性数据
        PlayerRolePropDataMgr.Inst.Init();
        
        //刷新血量UI
        UIEnemyPlayerInfo.Inst.Refresh();
        UIPlayerInfo.Inst.Refresh();
    }

    // Update is called once per frame
    private void Update()
    {
        CurFightStage.OnUpdate();
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
        ClearAction();
        SetActiveCharacte(null);
        SetFightStage(EFightStage.Normal);
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
                if (t.camp == enemyCamp && t.IsAlive() && t.HasQuickSkill() && t.IsInReady() && t.State != ECharacterState.Def)
                {
                    r = false;
                    break;
                }
            }
            return r;
        }
    }

    /// <summary>
    /// 是否跳过结束阶段:没有处于ready状态的人
    /// </summary>
    /// <returns></returns>
    public bool IsSkipEndStage
    {
        get
        {
            bool r = true;
            foreach (var t in lstCharacters)
            {
                if (t.IsAlive() && t.IsInReady())
                {
                    r = false;
                    break;
                }
            }
            return r;
        }
    }

    /// <summary>
    /// 进入下一个阶段
    /// </summary>
    public void ToNextStage()
    {
        if (IsInStage(EFightStage.ActionSelect))
        {
            if (lstActionData.Count == 0) return;
            //进入Ready状态
            SetFightStage(EFightStage.ActionReady);
        }
        else if (IsInStage(EFightStage.ActionReady))
        {
            //进入行动阶段
           SetFightStage(EFightStage.ActionAct);
        }
        else if (IsInStage(EFightStage.ActionAct))
        {
            //回合结束
            RoundEnd();
        }
        //else if (IsInStage(EFightStage.ActionEnd))
        //{
        //    if (lstActionData.Count == 0)
        //    {
        //        //回合结束
        //        RoundEnd();
        //    }
        //    else
        //    {
        //        //进入行动阶段
        //       SetFightStage(EFightStage.ActionAct);
        //    }
        //}
    }
}
