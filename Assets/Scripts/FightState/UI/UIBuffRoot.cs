using UnityEngine;
using System.Collections;
using DefaultNamespace;
using Boo.Lang;
using System;
using UI;

public class UIBuffRoot : MonoBehaviour
{
    public GameObject goGrid;
    public GameObject pfbUIItemBuff;

    public Character character;

    List<UIItemBuff> _lstBuffItems = new List<UIItemBuff>(10);

    public void SetTarget(Character target)
    {
        character = target;
    }

    public void RefreshOnAddABuff(BuffBase buff)
    {
        var goUIItem = GameUtil.PopOrInst(pfbUIItemBuff);
        goUIItem.transform.SetParent(goGrid.transform, false);
        var uiItemBuff = goUIItem.GetComponent<UIItemBuff>();
        uiItemBuff.SetTarget(buff);
        _lstBuffItems.Add(uiItemBuff);
        uiItemBuff.Refresh();
    }

    public void Refresh()
    {
        foreach (var uiBuffItem in _lstBuffItems)
        {
            uiBuffItem.Refresh();
        }
        if (character != null)
        {
            //跟随角色位置
            var posEntity = character.entityCtl.GetPos();
            var screenPos = FightState.Inst.cameraMain.WorldToScreenPoint(posEntity) + new Vector3(0, -50f, 0);
            Vector2 locPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIMgr.Inst.uiFight.GetRect(), screenPos, null, out locPos);
            transform.localPosition = locPos;
        }
    }

    internal void RefreshOnRemoveABuff(BuffBase buff)
    {
        UIItemBuff uiItembuff = null;
        foreach (var item in _lstBuffItems)
        {
            if (item.data == buff)
            {
                uiItembuff = item;
            }
        }
        GoPool.Inst.Cache(uiItembuff.gameObject);
        _lstBuffItems.Remove(uiItembuff);
    }
}
