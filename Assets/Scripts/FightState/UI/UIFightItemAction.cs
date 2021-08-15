using System.Text;
using Data;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightItemAction : MonoBehaviour
    {
        public Text txtName;
        public Text txtCost;

        public UIFightAction uiFightAction;

        private Skill  _skill;

        private Character _character;

        private bool _actionEnable;

        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        public void SetData(Skill skill, Character character, bool actionEnable)
        {
            _skill = skill;
            _character = character;
            _actionEnable = actionEnable;
            Refresh();
        }

        private void Refresh()
        {
            string nameDesc = _skill.GetBaseData().name;
            int cost = _skill.GetBaseData().cost;
            //发动蓄力技能
            if (_character.mSkillPowering == _skill)
            {
                nameDesc = "!发动-" + _skill.GetBaseData().name;
                cost = 0;
            }
            txtName.text = nameDesc;

            //if (cost > 0)
            //{
            //    txtCost.text = "COST:" + cost;
            //}
            //else
            //{
            //    txtCost.text = "";
            //}

            _btn.interactable = _actionEnable;
        }

        public void OnHoverIn()
        {
            StringBuilder sb = new StringBuilder();
            var skillData = _skill.GetBaseData();
            sb.AppendLine(skillData.name);

            //仇恨溢出提示
            //if (_character.camp == ECamp.Ally && _actionEnable)
            //{
            //    if (_character.target.ai.PreCalHatred(_character, _skillData.hatred))
            //    {
            //        sb.AppendLine("<color=red>>>将引起仇恨<<</color>");
            //    }
            //}
            if (skillData.distance == 1)
            {
                sb.AppendLine($"<color=yellow>近距离</color>");
            }
            else if (skillData.distance > 1)
            {
                sb.AppendLine($"<color=yellow>远距离</color>");
            }

            if (skillData.cost > 0)
            {
                sb.AppendLine($"<color=cyan>消耗:{skillData.cost / UIHPRoot.MPPerPoint}格</color>");
            }

            if (skillData.dmg > 0)
            {
                sb.AppendLine($"伤害:{skillData.dmg * 100}%");
            }
            if (skillData.dmgFire > 0)
            {
                sb.AppendLine($"火焰伤害:{skillData.dmgFire * 100}%");
            }
            if (skillData.timePower > 0)
            {
                sb.AppendLine($"蓄力时间:{skillData.timePower}S");
            }
            if (skillData.backswing > 0)
            {
                sb.AppendLine($"后摇时间:{skillData.backswing}S");
            }
            if (skillData.timeAtkStiff > 0)
            {
                sb.AppendLine($"攻击造成硬直:{skillData.timeAtkStiff}S");
            }
            if (skillData.dmgTenacity > 0)
            {
                sb.AppendLine($"削韧:{skillData.dmgTenacity}");
            }
           
            //if (_skillData.hatred > 0)
            //{
            //    sb.AppendLine($"仇恨:{_skillData.hatred}");
            //}
            //if (_skillData.quick)
            //{
            //    sb.AppendLine(">>速攻<<");
            //}
            sb.AppendLine();
            sb.AppendLine(skillData.tip);
            //UITip.Inst.Show(sb.ToString());
            UITip uiTip = UIMgr.Inst.ShowUI(UITable.EUITable.UITip) as UITip;
            uiTip.Refresh(sb.ToString());
            if (skillData.backswing > 0)
            {
                UIFight.Inst.ShowTimeTip(skillData.backswing);
            }
        }

        public void OnHoverOut()
        {
            //UITip.Inst.Hide();
            UIMgr.Inst.HideUI(UITable.EUITable.UITip);
            UIFight.Inst.HideTimeTip();
        }

        public void OnClick()
        {
            uiFightAction.OnSkillClick(this);
        }
        
        public Skill GetData()
        {
            return  _skill;
        }
    }
}