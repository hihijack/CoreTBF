﻿using DefaultNamespace;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFightItemCharacter : UIItemBase
    {
        public Image headIcon;
        public Text txtStiffTime;
        public Text txtHatred;
        public Image imgState;
        public Image imgPointer;
        public Character character;

        public GameObject goShield;
        public Text txtShield;
        //原指针高度
        float oriPointerHeight;
        RectTransform rtPointer;

        UIImageEx _imgExHeadIcon;

        private void Awake()
        {
            _imgExHeadIcon = headIcon.GetComponent<UIImageEx>();
        }

        void Start()
        {
            rtPointer = imgPointer.rectTransform;
            oriPointerHeight = rtPointer.sizeDelta.y;
        }

        public void SetData(Character character)
        {
            this.character = character;
            var sprite = Resources.Load<Sprite>($"Sprites/{character.roleData.headicon}");
            headIcon.sprite = sprite;
            if (character.camp == ECamp.Enemy)
            {
                imgPointer.color = Color.red;
            }
            else
            {
                imgPointer.color = Color.yellow;
            }
        }

        private void Update()
        {
            if (character != null)
            {
                if (!character.IsAlive())
                {
                    //死亡,移除
                    gameObject.transform.localPosition = Vector3.right * 1000;
                    return;
                }
                txtStiffTime.text = character.mTimeStiff.ToString("0.0") + "S";
                //if (character.camp == ECamp.Ally)
                //{
                //    //仇恨值
                //    txtHatred.text = character.target.ai.GetHatred(character).ToString();
                //}
                imgState.gameObject.SetActive(true);

                 if (character.State == ECharacterState.Wait)
                {
                    GameUtil.SetSprite(imgState,"wait");
                }
                else if (character.State == ECharacterState.Power)
                {
                    GameUtil.SetSprite(imgState, "power");
                }
                else if (character.State == ECharacterState.Def)
                {
                    GameUtil.SetSprite(imgState, "shield");
                }
                else if (character.State == ECharacterState.Dying)
                {
                    GameUtil.SetSprite(imgState, "Sprites/Buffs", "Icons8_28");
                }
                else if (character.State == ECharacterState.Dead)
                {
                    GameUtil.SetSprite(imgState, "Sprites/Buffs", "Icon.1_36");
                }
                else
                {
                    imgState.gameObject.SetActive(false);
                }

                if ( UIFightActionRoot.Inst != null && UIFightActionRoot.Inst.State == EUIState.Showing 
                    && character.camp == ECamp.Ally && character.IsEnableAction && character.IsInReady() && !FightState.Inst.characterMgr.HasActed(character))
                {
                    //可行动
                    _imgExHeadIcon.SetOutLineEnable(true);
                }
                else
                {
                    _imgExHeadIcon.SetOutLineEnable(false);
                }

                if (character.propData.shield > 0)
                {
                    goShield.SetActive(true);
                    txtShield.text = "x" + character.propData.shield.ToString();
                }
                else
                {
                    goShield.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 当头像点击
        /// </summary>
        public void OnBtnClick_HeadIcon()
        {
            if (UIFightActionRoot.Inst != null && UIFightActionRoot.Inst.State == EUIState.Showing)
            {
                UIFightActionRoot.Inst.OnSelectACharacter(character);
            }
        }

        /// <summary>
        /// 添加指针高度
        /// </summary>
        /// <param name="offset"></param>
        internal void AddPointHeight(int offset)
        {
            if (rtPointer == null)
            {
                return;
            }
            var t = rtPointer.sizeDelta;
            t.y = oriPointerHeight + offset;
            rtPointer.sizeDelta = t;
        }

        internal void SetIsHatredTarget(bool v)
        {
            if (v)
            {
                headIcon.color = Color.red;
            }
            else
            {
                headIcon.color = Color.white;
            }
        }
    }
}