﻿using UnityEngine;
using System.Collections;
using Boo.Lang;
using UI;
using DefaultNamespace;
using System.Collections.Generic;

public class UIHPRoot : UIBase
{
    public GameObject goGridAlly;
    public GameObject goGridEnemy;
    public GameObject pfbUIItemPlayerInfo;
    public GameObject pfbUIItemEnemyInfo;

    public GameObject goGridMP;
    public const int MPPerPoint = 10;

    public static UIHPRoot Inst{get;private set;}

    Dictionary<Character, UIPlayerInfo> _dicPlayerInfo = new Dictionary<Character, UIPlayerInfo>(5);
    Dictionary<Character, UIEnemyPlayerInfo> _dicEnemyInfo = new Dictionary<Character, UIEnemyPlayerInfo>(5);

    protected override void OnAwake()
    {
        base.OnAwake();
        Inst = this;
    }

    public void RefreshMP()
    {
        int mpCount = PlayerRolePropDataMgr.Inst.propData.mp / MPPerPoint;
        int mpLeft = PlayerRolePropDataMgr.Inst.propData.mp % MPPerPoint;
        int uiPointCount = mpLeft > 0 ? mpCount + 1 : mpCount;
        GameUtil.CacheChildren(goGridMP);
        for (int i = 0; i < uiPointCount; i++)
        {
            var pfbItem = Resources.Load<GameObject>("Prefabs/UI/ItemMP");
            var uiItemMP = GameUtil.PopOrInst(pfbItem);
            uiItemMP.transform.SetParent(goGridMP.transform);
            var itemMP = uiItemMP.GetComponent<UIItemMP>();
            if (i == mpCount)
            {
                itemMP.SetVal((float)mpLeft / MPPerPoint);
            }
            else
            {
                itemMP.SetVal(1f);
            }
        }
    }

    public void RefreshTarget(Character target)
    {
        if (target.camp == ECamp.Ally)
        {
            if (_dicPlayerInfo.ContainsKey(target))
            {
                _dicPlayerInfo[target].Refresh();
            }
            else
            {
                var uiItemPlayerInfo = UIPlayerInfo.Create<UIPlayerInfo>(goGridAlly.transform, pfbUIItemPlayerInfo);
                uiItemPlayerInfo.data = target;
                uiItemPlayerInfo.Refresh();
                _dicPlayerInfo.Add(target, uiItemPlayerInfo);
            }
        }
        else if (target.camp == ECamp.Enemy)
        {
            if (_dicEnemyInfo.ContainsKey(target))
            {
                _dicEnemyInfo[target].Refresh();
            }
            else
            {
                var uiItemPlayerInfo = UIEnemyPlayerInfo.Create<UIEnemyPlayerInfo>(goGridEnemy.transform, pfbUIItemEnemyInfo);
                uiItemPlayerInfo.SetData(target);
                uiItemPlayerInfo.Refresh();
                _dicEnemyInfo.Add(target, uiItemPlayerInfo);
            }
        }
    }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in _dicPlayerInfo.Values)
        {
            item.Cache();
        }
        foreach (var item in _dicEnemyInfo.Values)
        {
            item.Cache();
        }
        _dicPlayerInfo.Clear();
        _dicEnemyInfo.Clear();

    }
}
