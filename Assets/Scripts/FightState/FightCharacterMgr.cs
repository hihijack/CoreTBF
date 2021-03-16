using UnityEngine;
using UnityEditor;
using DefaultNamespace;
using UI;
using System;
using System.Collections.Generic;

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

    public Character AddCharacter(CharacterForRaid charaSource) 
    {
        var chara = new Character(charaSource);
        InitCharacter(chara, ECamp.Ally);
        return chara;
    }

    void InitCharacter(Character chara, ECamp camp)
    {
        int countOfCamp = GetCharactersCount(camp);
        chara.teamLoc = countOfCamp + 1;
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

    internal void HandleHPState()
    {
        foreach (var t in lstCharacters)
        {
            t.HandleHPState();
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
    List<Character> GetRandomOfCamp(int count, ECamp camp)
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