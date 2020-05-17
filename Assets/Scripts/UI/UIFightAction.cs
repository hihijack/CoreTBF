using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightAction : MonoBehaviour
    {
        public Image headIcon;
        public Image iconState;
        public GameObject pfbItemAction;
        public GameObject gridItemAction;

        private Character _character;

        public void SetData(Character character)
        {
            _character = character;
            GameUtil.CacheChildren(gridItemAction);
            Refresh();
        }

        private void Refresh()
        {
            //头像
            GameUtil.SetSprite(headIcon, _character.roleData.headicon);
            //状态
            iconState.gameObject.SetActive(true);
            if (_character.State == ECharacterState.Def)
            {
                GameUtil.SetSprite(iconState, "shield");
            }
            else if (_character.State == ECharacterState.Wait)
            {
                GameUtil.SetSprite(iconState, "wait");
            }
            else if (_character.State == ECharacterState.Power)
            {
                GameUtil.SetSprite(iconState, "power");
            }
            else
            {
                iconState.gameObject.SetActive(false);
            }
            //action列表
            foreach (var skillData in _character.lstSkillData)
            {
                var uiItem = GameUtil.PopOrInst(pfbItemAction);
                uiItem.transform.parent = gridItemAction.transform;
                var itemAction = uiItem.GetComponent<UIFightItemAction>();
                var actionEnable = !(_character.State == ECharacterState.Def && skillData.ID != 1 && skillData.ID != 2 || _character.mSkillPowering != null);
                if (GameMgr.Inst.IsInStage(EFightStage.ActionReady) && !skillData.quick)
                {
                    //Ready阶段只能选择速攻技能
                    actionEnable = false;
                }
                itemAction.SetData(skillData, _character, actionEnable);
            }
        }
    }
}