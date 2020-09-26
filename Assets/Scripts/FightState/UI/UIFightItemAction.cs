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

        private SkillBaseData  _skillData;

        private Character _character;

        private bool _actionEnable;

        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        public void SetData(SkillBaseData skillData, Character character, bool actionEnable)
        {
            _skillData = skillData;
            _character = character;
            _actionEnable = actionEnable;
            Refresh();
        }

        private void Refresh()
        {
            string nameDesc = _skillData.name;
            int cost = _skillData.cost;
            //发动蓄力技能
            if (_character.mSkillPowering == _skillData)
            {
                nameDesc = "!发动-" + _skillData.name;
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
            sb.AppendLine(_skillData.name);

            //仇恨溢出提示
            //if (_character.camp == ECamp.Ally && _actionEnable)
            //{
            //    if (_character.target.ai.PreCalHatred(_character, _skillData.hatred))
            //    {
            //        sb.AppendLine("<color=red>>>将引起仇恨<<</color>");
            //    }
            //}
            
            if (_skillData.cost > 0)
            {
                sb.AppendLine($"<color=cyan>消耗:{_skillData.cost / UIHPRoot.MPPerPoint}格</color>");
            }

            if (_skillData.dmg > 0)
            {
                sb.AppendLine($"伤害:{_skillData.dmg * 100}%");
            }
            if (_skillData.dmgFire > 0)
            {
                sb.AppendLine($"火焰伤害:{_skillData.dmgFire * 100}%");
            }
            if (_skillData.timePower > 0)
            {
                sb.AppendLine($"蓄力时间:{_skillData.timePower}S");
            }
            if (_skillData.backswing > 0)
            {
                sb.AppendLine($"后摇时间:{_skillData.backswing}S");
            }
            if (_skillData.timeAtkStiff > 0)
            {
                sb.AppendLine($"攻击造成硬直:{_skillData.timeAtkStiff}S");
            }
            if (_skillData.dmgTenacity > 0)
            {
                sb.AppendLine($"削韧:{_skillData.dmgTenacity}");
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
            sb.AppendLine(_skillData.tip);
            UITip.Inst.Show(sb.ToString());
            if (_skillData.backswing > 0)
            {
                UIMgr.Inst.uiFight.ShowTimeTip(_skillData.backswing);
            }
        }

        public void OnHoverOut()
        {
            UITip.Inst.Hide();
            UIMgr.Inst.uiFight.HideTimeTip();
        }

        public void OnClick()
        {
            uiFightAction.OnSkillClick(this);
        }
        
        public SkillBaseData GetData()
        {
            return  _skillData;
        }
    }
}