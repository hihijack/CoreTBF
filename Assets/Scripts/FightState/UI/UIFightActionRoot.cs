using DefaultNamespace;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightActionRoot : UIBase
    {
        public GameObject goGridAction;
        public GameObject goGridActionSelected;
        public GameObject pfbCharacterActionItem;
        public GameObject pfbActionSelectedItem;

        private bool _flagSelectedLstChangeed = false;

        private CanvasGroup cgGridAction;

        bool isVisible;

        public static UIFightActionRoot Inst{get;private set;}

        protected override void OnAwake()
        {
            base.OnAwake();
            Inst = this;
            cgGridAction = goGridAction.GetComponent<CanvasGroup>();
        }

        public override void OnHide()
        {
            base.OnHide();
             CacheItems();
        }

        public void MarkSelectedLstChanged()
        {
            _flagSelectedLstChangeed = true;
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
        }

        public void SetActionVisible(bool visible)
        {
            cgGridAction.alpha = visible ? 1f : 0f;
            cgGridAction.interactable = visible;
        }

        void CacheItems()
        {
            for (int i = goGridAction.transform.childCount - 1; i >= 0; i--)
            {
                GoPool.Inst.Cache(goGridAction.transform.GetChild(i).gameObject);
            }
        }

        public void StartShow(Character target)
        {
            CacheItems();

             //添加可行动列表
             var uiItem = GameUtil.PopOrInst(pfbCharacterActionItem);
            uiItem.transform.parent = goGridAction.transform;
            var uiItemFightAction = uiItem.GetComponent<UIFightAction>();
            uiItemFightAction.SetData(target);

            //敌人行动
            //RefreshActionSelectedList();
        }

        private void RefreshActionSelectedList()
        {
            GameUtil.CacheChildren(goGridActionSelected);
            foreach (var actionData in FightState.Inst.lstActionData)
            {
                var uiItem = GameUtil.PopOrInst(pfbActionSelectedItem);
                uiItem.transform.parent = goGridActionSelected.transform;
                var itemSelected = uiItem.GetComponent<UIItemActionSelected>();
                itemSelected.SetData(actionData);
            }
        }

        private void Update()
        {
            //if (_flagSelectedLstChangeed)
            //{
            //    RefreshActionSelectedList();
            //    _flagSelectedLstChangeed = false;
            //}
        }

        public void BtnStartAct()
        {
            //行动结束
            FightState.Inst.RoundEnd();
        }

        internal void OnSelectACharacter(Character character)
        {
            StartShow(character);
        }
    }
}