using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoleEntityCtl : MonoBehaviour
    {
        public Sprite[] sprites;
        public int face; //朝向.向右为1,左为-1
        Character _character;
        Dictionary<string, Sprite> _dicSprites;
        SpriteRenderer _spriteRenderer;
        
        public void Init(Character character)
        {
            this._character = character;
            _dicSprites = new Dictionary<string, Sprite>();
            foreach (var sprite in sprites)
            {
                _dicSprites.Add(sprite.name, sprite);
            }

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
            if (_dicSprites.ContainsKey(name))
            {
                _spriteRenderer.sprite = _dicSprites[name];
            }
        }

        internal void SetPos(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}