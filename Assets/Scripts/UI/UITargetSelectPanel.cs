﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UI;
using System;
using DefaultNamespace;
using Data;

public class UITargetSelectPanel : MonoBehaviour
{
    public Text txtSkillName;
    public Button btnCancel;
    public GameObject goGrid;
    public GameObject pfbTargetItem;
    public UIFightAction uiFightAction;

    Character caster;
    SkillData skillData;

    private void Start()
    {
        btnCancel.onClick.AddListener(OnBtnCancel);
    }


    public void SetData(Character caster, SkillData skillData)
    {
        this.caster = caster;
        this.skillData = skillData;
    }

    public void Refresh()
    {
        txtSkillName.text = skillData.name;

        GameUtil.CacheChildren(goGrid);

        foreach (var character in GameMgr.Inst.lstCharacters)
        {
            if (character.State == ECharacterState.Dead)
            {
                continue;
            }
            if (character.camp == ECamp.Ally && skillData.targetType == ESkillTarget.Ally || character.camp == ECamp.Enemy && skillData.targetType == ESkillTarget.Enemy)
            {
                var goUIItem = GameUtil.PopOrInst(pfbTargetItem);
                goUIItem.transform.SetParent(goGrid.transform, false);
                var uiItemTarget = goUIItem.GetComponent<UIItemTarget>();
                uiItemTarget.target = character;
                uiItemTarget.selectPanel = this;
                uiItemTarget.Refresh();
            }
        }
    }

    internal void OnTargetCick(UIItemTarget uiItemtarget)
    {
        uiFightAction.OnSkillTargetClick(skillData, uiItemtarget.target);
    }

    public void SetVisible(bool visible)
    {
        var locPos = transform.localPosition;
        locPos.x =  visible? 0f : 1000f;
        transform.localPosition = locPos;
    }

    private void OnBtnCancel()
    {
        SetVisible(false);
    }
}
