using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFight : UIBase
    {
        public Image imageProg;
        public GameObject itemRoot;
        public Text txtStage;
        public float timeMax;
        public int spaceSize = 82;
        public int heightOffset = 86;
        public GameObject timeTip;

        private List<UIFightItemCharacter> lstItems;

        List<UIBuffRoot> lstBuffRoots;

        List<UIAINextAction> lstAINextActions;

        private float progMin, progMax;

        RectTransform rtTransform;

        public static UIFight Inst{get;private set;}

        protected override void OnAwake()
        {
            base.OnAwake();
            Inst = this;
            rtTransform = gameObject.GetComponent<RectTransform>();
            //Event.Inst.Register(Event.EEvent.CHARACTER_DIE, OnCharacterDie);
            //Event.Inst.Register(Event.EEvent.CHARACTER_TEAMLOC_CHANGED, OnCharacterTeamLocChanged);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //Event.Inst.UnRegister(Event.EEvent.CHARACTER_DIE, OnCharacterDie);
            //Event.Inst.UnRegister(Event.EEvent.CHARACTER_TEAMLOC_CHANGED, OnCharacterTeamLocChanged);
        }

        private void OnCharacterDie(object obj)
        {
            Character unit = obj as Character;

        }

        /// <summary>
        /// 角色队伍序列号改变
        /// </summary>
        /// <param name="obj"></param>
        private void OnCharacterTeamLocChanged(object obj)
        {
            Character unit = obj as Character;

        }

        public RectTransform GetRect()
        {
            return rtTransform;
        }

        private void Start()
        {
            progMin = imageProg.rectTransform.localPosition.x - imageProg.rectTransform.sizeDelta.x * 0.5f;
            progMax = progMin + imageProg.rectTransform.sizeDelta.x;
        }

        public override void Init()
        {
            base.Init();
            lstItems = new List<UIFightItemCharacter>();
            lstBuffRoots = new List<UIBuffRoot>();
            lstAINextActions = new List<UIAINextAction>();
        }

        /// <summary>
        /// 新增一个角色
        /// </summary>
        /// <param name="character"></param>
        public void RefreshCharacter(Character character)
        {
            var uiItem = UIItemBase.Create<UIFightItemCharacter>(itemRoot.transform, "Prefabs/UI", "ItemCharacter");

            uiItem.SetData(character);

            lstItems.Add(uiItem);

            var uiBuffRoot = UIBuffRoot.Create<UIBuffRoot>(transform, "Prefabs/UI", "UIBuff");
            uiBuffRoot.SetTarget(character);
            uiBuffRoot.Refresh();

            lstBuffRoots.Add(uiBuffRoot);

            if (character.camp == ECamp.Enemy)
            {
                var uiItemAINextAction = UIAINextAction.Create<UIAINextAction>(transform, "Prefabs/UI", "ItemAINextAction");
                uiItemAINextAction.SetTarget(character);
                uiItemAINextAction.SetRtParent(rtTransform);
                uiItemAINextAction.Refresh();
                lstAINextActions.Add(uiItemAINextAction);
            }
        }

        public override void OnShow()
        {
            base.OnShow();
            HideTimeTip();
            var lstCharacters = FightState.Inst.characterMgr.GetCharacters();
            foreach (var character in lstCharacters)
            {
                RefreshCharacter(character);
            }
        }

        public override void OnHide()
        {
            base.OnHide();
            foreach (var item in lstItems)
            {
                item.Cache();
            }
            foreach (var item in lstAINextActions)
            {
                item.Cache();
            }
            foreach (var item in lstBuffRoots)
            {
                item.Cache();
            }
            lstItems.Clear();
            lstAINextActions.Clear();
            lstBuffRoots.Clear();
        }

        /// <summary>
        /// 添加buff时刷新
        /// </summary>
        /// <param name="target"></param>
        /// <param name="buff"></param>
        public void RefreshBuffUIOnAdd(Character target, BuffBase buff)
        {
            var uibuffRoot = GetBuffRoot(target);
            uibuffRoot.RefreshOnAddABuff(buff);
        }

        public void RefreshBuffUI()
        {
            foreach (var item in lstBuffRoots)
            {
                item.Refresh();
            }
        }

        public void RefreshBuffUIOnRemove(Character target, BuffBase buff)
        {
            var uibuffRoot = GetBuffRoot(target);
            uibuffRoot.RefreshOnRemoveABuff(buff);
        }

        public UIBuffRoot GetBuffRoot(Character target)
        {
            foreach (var item in lstBuffRoots)
            {
                if (item.character == target)
                {
                    return item;
                }
            }
            return null;
        }

        void InitAINexts()
        {
            foreach (var character in FightState.Inst.characterMgr.GetCharacters())
            {
                if (character.camp == ECamp.Enemy)
                {
                    var uiItem = UIAINextAction.Create<UIAINextAction>(transform, "Prefabs/UI", "ItemAINextAction");
                    uiItem.SetTarget(character);
                    uiItem.SetRtParent(rtTransform);
                    uiItem.Refresh();
                    lstAINextActions.Add(uiItem);
                }
            }
        }

        public void RefreshAIItems()
        {
            foreach (var item in lstAINextActions)
            {
                item.Refresh();
            }
        }

        public void SetAIItemsVisible(bool visible)
        {
            foreach (var item in lstAINextActions)
            {
                item.SetVisible(visible);
            }
        }

        private void Update()
        {
            if (lstItems != null)
            {
                for (int i = 0; i < lstItems.Count; i++)
                {
                    var itemUI = lstItems[i];
                    var normalProg = 1 - itemUI.character.mTimeStiff / timeMax;
                    normalProg = Mathf.Clamp01(normalProg);
                    var localPos =  new Vector3(Mathf.Lerp(progMin, progMax, normalProg), 0f, 0f);
                    int nearCount = 0;
                    for (int j = 0; j < i; j++)
                    {
                        var itemUIOther = lstItems[j];
                        var dis = Mathf.Abs(itemUI.transform.localPosition.x - itemUIOther.transform.localPosition.x);
                        if (dis <= spaceSize) 
                        {
                            nearCount++;
                        }
                    }
                    var offset = heightOffset * nearCount;
                    localPos.y = offset;
                    itemUI.AddPointHeight(offset);
                    itemUI.transform.localPosition = localPos;
                }
            }

            RefreshBuffUI();

            if (FightState.Inst.IsInStage(EFightStage.ActionSelect))
            {
                txtStage.text = "主要阶段";
            }
            else if (FightState.Inst.IsInStage(EFightStage.ActionReady))
            {
                txtStage.text = "速攻反击阶段";
            }
            else if (FightState.Inst.IsInStage(EFightStage.ActionEnd))
            {
                txtStage.text = "结束阶段";
            }
            else
            {
                txtStage.text = "";
            }
        }

        /// <summary>
        /// 刷新仇恨目标
        /// </summary>
        /// <param name="target"></param>
        internal void RefreshHatredTarget(Character target)
        {
            foreach (var t in lstItems)
            {
                t.SetIsHatredTarget(t.character == target);
            }
        }

        public void ShowTimeTip(float time)
        {
            var normalProg = 1 - time / timeMax;
            normalProg = Mathf.Clamp01(normalProg);
            timeTip.transform.localPosition = new Vector3(Mathf.Lerp(progMin, progMax, normalProg), 0f, 0f);
        }

        public void HideTimeTip()
        {
            timeTip.transform.localPosition = Vector3.left * 10000;
        }
    }
}