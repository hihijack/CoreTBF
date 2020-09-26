using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DefaultNamespace;

public class UIAINextAction : MonoBehaviour
{
    public Text text;
    public Character target;

    RectTransform rtParent;

    private void Awake()
    {
        rtParent = transform.parent.GetComponent<RectTransform>();
    }
  

    public void SetTarget(Character target)
    {
        this.target = target;
    }

    public void Refresh()
    {
        if (target.IsEnableAction)
        {
            var nextSkill = target.ai.GetNextSkillToCast();
            text.text = "Next:" + nextSkill.name;
            var posEntityHead = target.entityCtl.GetPos() + new Vector3(0, target.entityCtl.GetHeight(), 0);
            var screenPos = FightState.Inst.cameraMain.WorldToScreenPoint(posEntityHead);
            Vector2 locPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rtParent, screenPos, null, out locPos);
            transform.localPosition = locPos;
        }
    }
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
