using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DefaultNamespace;

public class UIAINextAction : UIItemBase
{
    public Text text;
    public Character target;

    RectTransform rtParent;

    public void SetTarget(Character target)
    {
        this.target = target;
    }

    public void SetRtParent(RectTransform rt)
    {
        rtParent = rt;
    }

    private void Update()
    {
        Refresh();
    }

    public override void Refresh()
    {
        if (target != null && target.IsEnableAction)
        {
            var nextSkill = target.ai.GetNextSkillToCast();
            text.text = "Next:" + nextSkill.name;
            var posEntityHead = target.entityCtl.GetPos() + new Vector3(0, target.entityCtl.GetHeight(), 0);
            var screenPos = FightState.Inst.cameraMain.WorldToScreenPoint(posEntityHead);
            Vector2 locPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rtParent, screenPos, null, out locPos);
            transform.localPosition = locPos;
        }else
        {
            SetVisible(false);
        }
    }
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
