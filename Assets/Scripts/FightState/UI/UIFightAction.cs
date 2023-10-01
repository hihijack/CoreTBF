using Data;
using DefaultNamespace;
using System;
using System.Text;
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
        public Text txtName;
        public Text txtProp;
        public UITargetSelectPanel uiTargetSelectPanel;

        private Character _character;

        StringBuilder _sbProp;

        public void SetData(Character character)
        {
            _character = character;
            GameUtil.CacheChildren(gridItemAction);
            _sbProp = new StringBuilder();
            Refresh();
        }

        private void Refresh()
        {
            //头像
            GameUtil.SetSprite(headIcon, _character.roleData.headicon);
            //名字
            txtName.text = _character.roleData.name;
            //属性
            _sbProp.Clear();
            _sbProp.AppendLine($"攻击力:{_character.roleData.atk}");
            //_sbProp.AppendLine($"防御力:{_character.roleData.def}");
            //_sbProp.AppendLine($"抗火:{_character.roleData.resFire}");
            txtProp.text = _sbProp.ToString();
            //状态
            iconState.gameObject.SetActive(true);
            if (_character.State == ECharacterState.Wait)
            {
                GameUtil.SetSprite(iconState, "wait");
            }
            else if (_character.State == ECharacterState.Power)
            {
                GameUtil.SetSprite(iconState, "power");
            }
            else if (_character.State == ECharacterState.Def)
            {
                GameUtil.SetSprite(iconState, "shield");
            }
            else if (_character.State == ECharacterState.Dying)
            {
                GameUtil.SetSprite(iconState, "Sprites/Buffs", "Icons8_28");
            }
            else if (_character.State == ECharacterState.Dead)
            {
                GameUtil.SetSprite(iconState, "Sprites/Buffs", "Icon.1_36");
            }
            else
            {
                iconState.gameObject.SetActive(false);
            }
            //仇恨
            //if (GameMgr.Inst.IsHatredTarget(_character))
            //{
            //    headIcon.color = Color.red;
            //}
            //else
            //{
            //    headIcon.color = Color.white;
            //}
            //action列表
            foreach (var skill in _character.lstSkill)
            {
                var skillData = skill.GetBaseData();
                if (skillData == null)
                {
                    continue;
                }
                var uiItem = GameUtil.PopOrInst(pfbItemAction);
                uiItem.transform.SetParent(gridItemAction.transform, false);
                var itemAction = uiItem.GetComponent<UIFightItemAction>();
                itemAction.uiFightAction = this;
                var actionEnable = true;

                //非行动者,不能行动
                if (_character.camp == ECamp.Enemy || !_character.IsInReady() || FightState.Inst.characterMgr.HasActed(_character) || !_character.IsEnableAction)
                {
                    actionEnable = false;
                }

                if (_character.mSkillPowering != null && _character.mSkillPowering != skill)
                {
                    //有正在蓄力的技能,只能使用正在蓄力的技能
                    actionEnable = false;
                }
                //if (GameMgr.Inst.IsInStage(EFightStage.ActionReady) && !skillData.quick)
                //{
                //    //Ready阶段只能选择速攻技能
                //    actionEnable = false;
                //}
                //能量不足,无法选择
                int cost = skillData.cost;
                //发动蓄力,不需要能量消耗
                if (_character.mSkillPowering == skill)
                {
                    cost = 0;
                }
                if (PlayerRolePropDataMgr.Inst.propData.mp < cost)
                {
                    actionEnable = false;
                }

                //防御或等待中,不可再使用相同技能
                if (_character.State == ECharacterState.Wait && skillData.logic == Data.ESkillLogic.Wait || 
                    _character.State == ECharacterState.Def && skillData.logic == Data.ESkillLogic.Def)
                {
                    actionEnable = false;
                }

                itemAction.SetData(skill, _character, actionEnable);
            }

            //默认隐藏目标选择面板
            uiTargetSelectPanel.SetVisible(false);
        }

        internal void OnSkillClick(UIFightItemAction uiItemAction)
        {
            bool needShowTargetSelectUI = true;

            var skill = uiItemAction.GetData();
            var skillData = skill.GetBaseData();
            if (skillData.targetType == ESkillTarget.Self || skillData.targetType == ESkillTarget.None)
            {
                //不需要选择目标
                //对自身释放不需要
                needShowTargetSelectUI = false;
            }

            if (needShowTargetSelectUI)
            {
                uiTargetSelectPanel.SetVisible(true);
                uiTargetSelectPanel.SetData(_character, skill);
                uiTargetSelectPanel.Refresh();
            }
            else
            {
                //不需要选择目标
                //AOE不需要;对自身释放不需要
                uiTargetSelectPanel.SetVisible(false);
                RealSkillCast(uiItemAction.GetData(), null);
            }
        }

        private void RealSkillCast(Skill skill, Character target)
        {
            _character.OnActionSelected(skill, target);
            UIMgr.Inst.HideUI(UITable.EUITable.UIFightActionPanel);
        }

        internal void OnSkillTargetClick(Skill skill, Character target)
        {
            RealSkillCast(skill, target);
        }
    }
}