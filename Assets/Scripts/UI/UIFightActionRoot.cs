using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightActionRoot : MonoBehaviour
    {
        public GameObject goGridAction;
        public GameObject goGridActionSelected;
        public GameObject pfbCharacterActionItem;
        public GameObject pfbActionSelectedItem;

        private bool _flagSelectedLstChangeed = false;

        private CanvasGroup cgGridAction;

        private void Awake()
        {
            cgGridAction = goGridAction.GetComponent<CanvasGroup>();
        }

        public void MarkSelectedLstChanged()
        {
            _flagSelectedLstChangeed = true;
        }
        
        public void SetVisible(bool visible)
        {
            if (visible)
            {
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = Vector3.left * 10000;
            }
        }

        public void SetActionVisible(bool visible)
        {
            cgGridAction.alpha = visible ? 1f : 0f;
            cgGridAction.interactable = visible;
        }

        public void StartShow()
        {
            for (int i = goGridAction.transform.childCount - 1; i >= 0; i--)
            {
                GoPool.Inst.Cache(goGridAction.transform.GetChild(i).gameObject);
            } 
            for (int i = goGridActionSelected.transform.childCount - 1; i >= 0; i--)
            {
                GoPool.Inst.Cache(goGridActionSelected.transform.GetChild(i).gameObject);
            }
            
            //添加可行动列表
            foreach (var character in GameMgr.Inst.lstCharacters)
            {
                if (character.camp == ECamp.Ally && character.IsInReady())
                {
                    var uiItem = GameUtil.PopOrInst(pfbCharacterActionItem);
                    uiItem.transform.parent = goGridAction.transform;
                    var uiItemFightAction = uiItem.GetComponent<UIFightAction>();
                    uiItemFightAction.SetData(character);
                }
            }
            //敌人行动
            RefreshActionSelectedList();
        }

        private void RefreshActionSelectedList()
        {
            GameUtil.CacheChildren(goGridActionSelected);
            foreach (var actionData in GameMgr.Inst.lstActionData)
            {
                var uiItem = GameUtil.PopOrInst(pfbActionSelectedItem);
                uiItem.transform.parent = goGridActionSelected.transform;
                var itemSelected = uiItem.GetComponent<UIItemActionSelected>();
                itemSelected.SetData(actionData);
            }
        }

        private void Update()
        {
            if (_flagSelectedLstChangeed)
            {
                RefreshActionSelectedList();
                _flagSelectedLstChangeed = false;
            }
        }

        public void BtnStartAct()
        {
            GameMgr.Inst.ToNextStage();
        }
    }
}