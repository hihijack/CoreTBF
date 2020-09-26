using UnityEngine;
using System.Collections;
using Boo.Lang;
using UI;
using DefaultNamespace;
using System.Collections.Generic;

public class UIHPRoot : MonoBehaviour
{
    public GameObject goGridAlly;
    public GameObject goGridEnemy;
    public GameObject pfbUIItemPlayerInfo;
    public GameObject pfbUIItemEnemyInfo;

    public GameObject goGridMP;
    public const int MPPerPoint = 10;

    Dictionary<Character, UIPlayerInfo> _dicPlayerInfo = new Dictionary<Character, UIPlayerInfo>(5);
    Dictionary<Character, UIEnemyPlayerInfo> _dicEnemyInfo = new Dictionary<Character, UIEnemyPlayerInfo>(5);

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
                var goUIItem = GameUtil.PopOrInst(pfbUIItemPlayerInfo);
                goUIItem.transform.SetParent(goGridAlly.transform, false);
                var uiItemPlayerInfo = goUIItem.GetComponent<UIPlayerInfo>();
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
                var goUIItem = GameUtil.PopOrInst(pfbUIItemEnemyInfo);
                goUIItem.transform.SetParent(goGridEnemy.transform, false);
                var uiItemPlayerInfo = goUIItem.GetComponent<UIEnemyPlayerInfo>();
                uiItemPlayerInfo.SetData(target);
                uiItemPlayerInfo.Refresh();
                _dicEnemyInfo.Add(target, uiItemPlayerInfo);
            }
        }
    }
}
