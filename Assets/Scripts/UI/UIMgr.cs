using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UITable;

namespace UI
{
    public class UIMgr : MonoBehaviour
    {
        public Canvas canvasRoot;

        public static UIMgr Inst { get; private set; }

        public Dictionary<EUITable, UIBase> _dicUIs = new Dictionary<EUITable, UIBase>();

        private void Awake()
        {
            Inst = this;
        }
       
        public UIBase ShowUI(EUITable uiName)
        {
            Debug.Log("ShowUI:" + uiName);
            UIBase ui = null;
            if (_dicUIs.ContainsKey(uiName))
            {
                ui = _dicUIs[uiName];
            }
            else
            {
                var uiAsset = Resources.Load<GameObject>(GamePathUtil.UI + uiName);
                if (uiAsset != null)
                {
                    var goUI = Instantiate(uiAsset, canvasRoot.transform);
                    ui = goUI.GetComponent<UIBase>();
                    if (ui != null)
                    {
                        _dicUIs.Add(uiName, ui);
                        ui.Init();
                    }
                    else
                    {
                        Debug.LogError("ui has no uiBase component:" + uiName);
                    }
                }
                else
                {
                    Debug.LogError("uiAsset is null:" + uiName);
                }

            }
            ui.SetVisible(true);
            ui.OnShow();
            return ui;
        }

        public void HideUI(EUITable uiName)
        {
            if (_dicUIs.ContainsKey(uiName))
            {
                var ui = _dicUIs[uiName];
                ui.SetVisible(false);
                ui.OnHide();
            }
        }

        public void HideAll()
        {
            foreach (var ui in _dicUIs.Values)
            {
                ui.SetVisible(false);
                ui.OnHide();
            }
        }

        public Vector2 GetMousePos()
        {
            Vector2 position;
            //获取鼠标在画布上的位置
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot.transform as RectTransform, Input.mousePosition, null, out position);
            return position;
        }

        public T GetUI<T>(EUITable uiName) where T : UIBase
        {
            if (_dicUIs.ContainsKey(uiName))
            {
                var ui = _dicUIs[uiName];
                if (ui.State == EUIState.Showing)
                {
                    return ui as T;
                }
            }
            return null;
        }
    }
}