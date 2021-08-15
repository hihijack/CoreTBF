using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIItemActionSelected : MonoBehaviour
    {
        public Image headIcon;
        public Text skillName;

        FightActionBase _action;
        
        public void SetData(FightActionBase action)
        {
            _action = action;
            Refresh();
        }

        private void Refresh()
        {
            GameUtil.SetSprite(headIcon, _action.Caster.roleData.headicon);
            skillName.text = _action.skill.GetBaseData().name;
        }
    }
}