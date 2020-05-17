using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightItemCharacter : MonoBehaviour
    {
        public Image headIcon;
        public Text txtStiffTime;
        public Text txtHatred;
        public Image imgState;
        public RectTransform rtPointer;
        public Character character;

        //原指针高度
        float oriPointerHeight;

        void Start()
        {
            oriPointerHeight = rtPointer.sizeDelta.y;
        }

        public void SetData(Character character)
        {
            this.character = character;
            var sprite = Resources.Load<Sprite>($"Sprites/{character.roleData.headicon}");
            headIcon.sprite = sprite;
        }

        private void Update()
        {
            if (character != null)
            {
                txtStiffTime.text = character.mTimeStiff.ToString("0.0") + "S";
                if (character.camp == ECamp.Ally)
                {
                    //仇恨值
                    txtHatred.text = character.target.ai.GetHatred(character).ToString();
                }
                imgState.gameObject.SetActive(true);

                if (character.State == ECharacterState.Def)
                {
                    GameUtil.SetSprite(imgState,"shield");
                }
                else if (character.State == ECharacterState.Wait)
                {
                    GameUtil.SetSprite(imgState,"wait");
                }
                else if (character.State == ECharacterState.Power)
                {
                    GameUtil.SetSprite(imgState, "power");
                }
                else
                {
                    imgState.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 添加指针高度
        /// </summary>
        /// <param name="offset"></param>
        internal void AddPointHeight(int offset)
        {
            var t = rtPointer.sizeDelta;
            t.y = oriPointerHeight + offset;
            rtPointer.sizeDelta = t;
        }
    }
}