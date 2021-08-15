using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoleEntityCtl : MonoBehaviour
    {
        public int face; //朝向.向右为1,左为-1
        Character _character;
        SpriteRenderer _spriteRenderer;
        Animator _anim;
        public void Init(Character character)
        {
            this._character = character;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            //添加动画状态机
            _anim = GameUtil.GetOrAdd<Animator>(gameObject);
            if (_anim.runtimeAnimatorController == null)
            {
                GameResLoader.Load<RuntimeAnimatorController>("comm_character.controller", (a) => { _anim.runtimeAnimatorController = a; });
            }
            else
            {
                PlayAnim("idle");
            }
        }

        internal Vector3 GetPos()
        {
            return transform.position;
        }

        public float GetHeight()
        {
            return _spriteRenderer.bounds.size.y;
        }

        public void SetSprite(string name)
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{GameUtil.ToTitleCase(_character.roleData.model)}/{name}");
        }

        internal void SetPos(Vector3 pos)
        {
            transform.position = pos;
        }

        internal void Cache()
        {
           GoPool.Inst.Cache(gameObject);
        }

        public void PlayAnim(string animName)
        {
            if (_anim.runtimeAnimatorController)
            {
                _anim.Play(animName, 0, 0);
            }
        }
    }
}