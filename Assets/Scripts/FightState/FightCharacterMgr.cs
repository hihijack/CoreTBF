using UnityEngine;
using UnityEditor;
using DefaultNamespace;
using UI;
using System;
using System.Collections.Generic;
using Data;

public class FightCharacterMgr
{
    List<Character> lstCharacters;

    private List<Character> lstChrActed = new List<Character>();//缓存已行动的角色

    GameObject unitRoot;

    public FightCharacterMgr()
    {
        lstCharacters = new List<Character>();
    }

    public void SetUnitRoot(GameObject go)
    {
        unitRoot = go;
    }

    public Character AddCharacter(int id, ECamp camp)
    {
        var chara = new Character(id);
        InitCharacter(chara, camp);
        return chara;
    }

    internal Character GetCharacterByID(int roleID)
    {
        foreach (var chara in lstCharacters)
        {
            if (chara.roleData.ID == roleID)
            {
                return chara;
            }
        }
        return null;
    }

    public Character AddCharacter(CharacterForRaid charaSource) 
    {
        var chara = new Character(charaSource);
        InitCharacter(chara, ECamp.Ally);
        return chara;
    }

    /// <summary>
    /// 刷新阵营的所有角色位置
    /// </summary>
    /// <param name="camp"></param>
    internal void RefreshAllUnitPos(ECamp camp)
    {
        foreach (var chara in lstCharacters)
        {
            chara.entityCtl.transform.position = FightState.Inst.GetPosByTeamLoc(chara.camp, chara.teamLoc);
        }
    }

    /// <summary>
    /// 取坦克角色：活着的最前排单位
    /// </summary>
    /// <param name="eCamp"></param>
    /// <returns></returns>
    internal Character GetTankCharacter(ECamp eCamp)
    {
        Character chr = null;

        foreach (var charaT in lstCharacters)
        {
            if (charaT.camp == eCamp && charaT.IsAlive())
            {
                if (chr == null)
                {
                    chr = charaT;
                }
                else
                {
                    if (charaT.teamLoc < chr.teamLoc)
                    {
                        chr = charaT;
                    }
                }
            }
        }
        return chr;
    }

    /// <summary>
    /// 添加一个角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="camp"></param>
    /// <param name="isAhead">是否往前添加，冲突的角色往后移动。否则往后添加，冲突的角色往前移动</param>
    /// <returns></returns>
    public Character AddCharacter(int id, ECamp camp, bool isAhead)
    {
        Debug.Log("AddCharacter:" + id + "," + camp + "," + isAhead);//#########
        if (GetValidCharacterCount(camp) >= GameCfg.MAX_CHARACTER_PER_CAMP)
        {
            //满员,无法继续添加
            return null;
        }
        var chara = new Character(id);
        int newLoc = AdjustCharacterLocForNew(camp, isAhead);
        InitCharacter(chara, camp, newLoc);
        return chara;
    }

    /// <summary>
    /// 为新角色加入调整角色站位，并返回新角色的站位loc
    /// </summary>
    /// <param name="camp"></param>
    /// <param name="isAhead"></param>
    /// <returns></returns>
    private int AdjustCharacterLocForNew(ECamp camp, bool isAhead)
    {
        int newLoc = 0;
        if (isAhead)
        {
            newLoc = 1;
            //移动冲突的角色
            InsertToLocByMoveBack(newLoc, camp);
        }
        else
        {
            newLoc = GetCharactersCount(camp) + 1;
            //移动冲突的角色
            InsertToLocByMoveAhead(newLoc, camp);
        }
        return newLoc;
    }

    /// <summary>
    /// 往loc位置插入一个新单位，已存在的单位往后移，并往新loc插入
    /// </summary>
    /// <param name="loc"></param>
    void InsertToLocByMoveBack(int loc, ECamp camp)
    {
        Character chara = GetAliveCharacter(camp, loc);
        if (chara != null)
        {
            if (loc == GameCfg.MAX_CHARACTER_PER_CAMP)
            {
                //最后位置有人，不能再往后插入了
                return;
            }
            InsertToLocByMoveBack(chara.teamLoc + 1, camp);
            chara.teamLoc++;
        }
    }

    void InsertToLocByMoveAhead(int loc, ECamp camp)
    {
        Character chara = GetAliveCharacter(camp, loc);
        if (chara != null)
        {
            if (loc == 1)
            {
                //最前位置有人，不能再往前插入了
                return;
            }
            InsertToLocByMoveAhead(chara.teamLoc - 1, camp);
            chara.teamLoc--;
        }
    }

    /// <summary>
    /// 将目标变更到指定loc，调整其他冲突单位的loc
    /// </summary>
    /// <param name="target"></param>
    /// <param name="loc"></param>
    public bool ChangeToLoc(Character target, int loc)
    {
        if (target.teamLoc == loc)
        {
            return false;
        }
        //往前换位，其他人需要往后移动
        //往后换位，其他人需要往前移动
        bool isMoveAHead = loc < target.teamLoc;
        //先设置为其他阵营，在其他人调整位置时让出
        ECamp oriCamp = target.camp;
        target.camp = target.GetEnemyCamp();
        if (isMoveAHead)
        {
            InsertToLocByMoveBack(loc, oriCamp);
        }
        else
        {
            InsertToLocByMoveAhead(loc, oriCamp);
        }
        target.camp = oriCamp;
        target.teamLoc = loc;
        return true;
    }

    private Character GetCharacter(ECamp camp, int loc)
    {
        foreach (var chara in lstCharacters)
        {
            if (chara.camp == camp && chara.teamLoc == loc)
            {
                return chara;
            }
        }
        return null;
    }

    void InitCharacter(Character chara, ECamp camp, int teamLoc = 0)
    {
        if (teamLoc > 0)
        {
            chara.teamLoc = teamLoc;
        }
        else
        {
            int countOfCamp = GetCharactersCount(camp);
            chara.teamLoc = countOfCamp + 1;
        }
       
        chara.camp = camp;
        chara.entityCtl.transform.SetParent(unitRoot.transform);
        chara.entityCtl.transform.position = FightState.Inst.GetPosByTeamLoc(camp, chara.teamLoc);
        chara.entityCtl.face = camp == ECamp.Ally ? 1 : -1;
        lstCharacters.Add(chara);
    }

    public List<Character> GetCharacters()
    {
        return lstCharacters;
    }

    /// <summary>
    /// 获取技能允许的目标
    /// </summary>
    /// <param name="skillData"></param>
    /// <param name="caster"></param>
    /// <returns></returns>
    public List<Character> GetSkillTargets(SkillBaseData skillData, Character caster)
    {
        List<Character> result = new List<Character>();

        if (!skillData.IsNeedSelectTarget())
        {
            return result;
        }

        foreach (var character in FightState.Inst.characterMgr.GetCharacters())
        {
            if (character.State == ECharacterState.Dead)
            {
                continue;
            }

            if (!skillData.IsTargetTypeContainSelf() && character == caster)
            {
                continue;
            }

            if (!skillData.IsTargetTypeContainAlly() && character.camp == caster.camp)
            {
                continue;
            }

            if (skillData.targetType != ESkillTarget.Enemy && character.camp != caster.camp)
            {
                continue;
            }

            if (skillData.targetType == ESkillTarget.Enemy)
            {
                if (skillData.distance == 1 && character.teamLoc > 1)
                {
                    //近距离.无法选择2/3号位
                    continue;
                }
            }

            result.Add(character);
        }
        return result;
    }

    internal void CacheAllAIAction()
    {
        foreach (var t in lstCharacters)
        {
            if (t.camp == ECamp.Enemy && t.IsEnableAction && t.IsInReady())
            {
                FightState.Inst.CaheAction(t.ai.ActionForMain());
            }
        }
    }

    /// <summary>
    /// 取随机一个友军
    /// </summary>
    /// <returns></returns>
    internal Character GetARandomAllyCharacter()
    {
        var teamLoc = UnityEngine.Random.Range(1, 4);
        return GetAliveCharacter(ECamp.Ally, teamLoc);
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

    internal void HandleHPState(ActionContent content)
    {
        foreach (var t in lstCharacters)
        {
            t.HandleHPState(content);
        }
    }

    public int GetCharactersCount(ECamp camp)
    {
        int count = 0;
        foreach (var t in lstCharacters)
        {
            if (t.camp == camp)
            {
                count++;
            }
        }
        return count;
    }

    public int GetValidCharacterCount(ECamp camp)
    {
        int count = 0;
        foreach (var t in lstCharacters)
        {
            if (t.camp == camp && t.IsAlive())
            {
                count++;
            }
        }
        return count;
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
            if (t.camp == FightState.Inst.GetActiveCharacter().camp && t.IsEnableAction && t.IsInReady() && !lstChrActed.Contains(t))
            {
                r = true;
                break;
            }
        }
        return r;
    }

    internal void ChangeTeamLocOnSomeOneDie(Character character)
    {
        foreach (var chr in lstCharacters)
        {
            if (chr.camp == character.camp && chr != character && chr.teamLoc > character.teamLoc)
            {
                chr.teamLoc--;
                FightState.Inst.eventRecorder.CacheEvent(new FightEventChangePos(chr, true));
            }
        }
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

    internal bool CheckSkipReadyStage(ECamp enemyCamp)
    {
        bool r = true;
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

    internal bool CheckSkipEndStage()
    {
        bool r = true;
        foreach (var t in lstCharacters)
        {
            if (t.IsEnableAction && t.IsInReady())
            {
                r = false;
                break;
            }
        }
        return r;
    }

    /// <summary>
    /// 取指定站位的一个角色
    /// </summary>
    /// <param name="targetTeamLoc"></param>
    /// <returns></returns>
    internal Character GetAliveCharacter(ECamp camp, int teamLoc)
    {
        foreach (var character in lstCharacters)
        {
            if (character.camp == camp && character.teamLoc == teamLoc && character.IsAlive())
            {
                return character;
            }
        }
        return null;
    }

    /// <summary>
    /// 查找正在蓄力的角色
    /// </summary>
    /// <param name="camp"></param>
    /// <returns></returns>
    public Character FindThatIsPowering(ECamp camp) 
    {
        foreach (var character in lstCharacters)
        {
            if (character.camp == camp && character.IsAlive() && character.State == ECharacterState.Power)
            {
                return character;
            }
        }
        return null;
    }

    /// <summary>
    /// 取随机N个敌方
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<Character> GetRandomOfCamp(int count, ECamp camp)
    {
        if (count == 0)
        {
            return null;
        }

        List<Character> r = new List<Character>();
        //先加入所有活着的敌人
        foreach (var t in lstCharacters)
        {
            if (t.propData.hp > 0 && t.camp == camp)
            {
                r.Add(t);
            }
        }

        int countToRemove = r.Count - count;//需要移除的数量
        if (countToRemove > 0)
        {
            for (int i = 0; i < countToRemove; i++)
            {
                //移除随机一个
                int indexToRemove = UnityEngine.Random.Range(0, r.Count);
                r.RemoveAt(indexToRemove);
            }
        }

        return r;
    }

    internal void UpdateAllCharacterInNormalStage()
    {
        foreach (var character in lstCharacters)
        {
            character.UpdateInNormalStage();
        }
    }

    /// <summary>
    /// 当角色死亡
    /// </summary>
    /// <param name="character"></param>
    internal void OnCharacterDead(Character character)
    {
        ChangeTeamLocOnSomeOneDie(character);
        Event.Inst.Fire(Event.EEvent.CHARACTER_DIE, character);
    }

    /// <summary>
    /// 检测一队全灭
    /// </summary>
    /// <returns></returns>
    public bool CheckATeamDieOut(ECamp camp)
    {
        bool r = true;
        foreach (var chara in lstCharacters)
        {
            if (chara.camp == camp && chara.IsAlive())
            {
                r = false;
            }
        }
        return r;
    }

    /// <summary>
    /// 清理角色实体
    /// </summary>
    internal void Clear()
    {
        foreach (var character in lstCharacters)
        {
            character.Clear();
        }
        lstCharacters.Clear();
        lstChrActed.Clear();
        unitRoot = null;
    }
}