using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UI;

public class UIItemFightDmg : UIItemBase
{
    public Text txtDmg;
    public GameObject goRoot;
    public Color colDmg;
    public Color colHeal;

    DmgResult dmgResult;
    HealResult healResult;
    Character target;

    bool isDmg;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(Character target, DmgResult dmgResult)
    {
        this.target = target;
        this.dmgResult = dmgResult;
        this.isDmg = true;
    }

    public void SetData(Character target, HealResult healResult)
    {
        this.target = target;
        this.healResult = healResult;
        this.isDmg = false;
    }


    public override void Refresh()
    {
        if (isDmg)
        {
            txtDmg.text = "-" + dmgResult.dmg;
            txtDmg.color = colDmg;
        }
        else
        {
            txtDmg.text = "+" + healResult.heal;
            txtDmg.color = colHeal;
        }
        
        SetPos();
        PlayAnim();
    }

    private void PlayAnim()
    {
        goRoot.transform.localPosition = Vector3.zero;
        var tweenMove = goRoot.transform.DOLocalMove(new Vector3(0f, 50f, 0f), 1f);
        tweenMove.OnComplete(OnAnimEnd);
        txtDmg.DOFade(0f, 0.3f).SetDelay(0.7f);
    }

    private void OnAnimEnd()
    {
        Cache();
    }

    private void SetPos()
    {
        var posEntity = target.entityCtl.GetPos();
        transform.localPosition = UIUtil.CalUIPosOfUI(UIFightTipRoot.Inst.GetRectTransform(), posEntity, 10, 100);
    }
}
