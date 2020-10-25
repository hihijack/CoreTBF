using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITip : UIBase
    {
        public Text txtTip;

        float border = 10;

        RectTransform rt;

        protected override void OnAwake()
        {
            base.OnAwake();
            rt = GetComponent<RectTransform>();
        }

        public void Refresh(string tip)
        {
            txtTip.text = tip;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            rt.sizeDelta = new Vector2(txtTip.rectTransform.sizeDelta.x + border * 2, txtTip.rectTransform.sizeDelta.y + border * 2);

            Vector2 offsetToMin = new Vector2(-1 * rt.sizeDelta.x / 2, 30);
            Vector2 min = UIMgr.Inst.GetMousePos() + offsetToMin;
            Rect rect = new Rect(min, rt.sizeDelta);
            Rect rectScreen = new Rect();
            rectScreen.center = Vector2.zero;
            rectScreen = rectScreen.AlignCenter(Screen.width - 20, Screen.height - 20);
            rect = GameUtil.LimitRectIn(rect, rectScreen);
            rt.localPosition = rect.center;
        }

        public override void OnShow()
        {
            base.OnShow();
        }
    }
}