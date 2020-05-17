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
        
        private SkillData  _skillData;

        private Character _character;

        private bool _actionEnable;

        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
        }

        public void SetData(SkillData skillData, Character character, bool actionEnable)
        {
            _skillData = skillData;
            _character = character;
            _actionEnable = actionEnable;
            Refresh();
        }

        private void Refresh()
        {
            txtName.text = _skillData.name;
            if (_skillData.cost > 0)
            {
                txtCost.text = "COST:" +  _skillData.cost;
            }
            else
            {
                txtCost.text = "";
            }

            _btn.interactable = _actionEnable;
        }

        public void OnHoverIn()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(_skillData.name);
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
            if (_skillData.tenacityAtk > 0)
            {
                sb.AppendLine($"削韧:{_skillData.tenacityAtk}");
            }
            if (_skillData.cost > 0)
            {
                sb.AppendLine($"消耗:{_skillData.cost}");
            }
            if (_skillData.hatred > 0)
            {
                sb.AppendLine($"仇恨:{_skillData.hatred}");
            }
            if (_skillData.quick)
            {
                sb.AppendLine(">>速攻<<");
            }
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
            _character.OnActionSelected(_skillData);
        }
        
        public SkillData GetData()
        {
            return  _skillData;
        }
    }
}