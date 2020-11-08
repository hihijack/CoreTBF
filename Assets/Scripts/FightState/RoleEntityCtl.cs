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
        
        public void Init(Character character)
        {
            this._character = character;
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
    }
}