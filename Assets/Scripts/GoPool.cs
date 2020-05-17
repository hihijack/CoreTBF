using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GoPool
    {
        private GameObject _goRoot;
        
        private Dictionary<string, Queue<GameObject>> _dicCached;
        
        #region 单例

        private static GoPool _inst;
        
        public static GoPool Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new GoPool();
                }

                return _inst;
            }
        }

        private GoPool()
        {
            _goRoot = new GameObject("_GoPool");
            _goRoot.transform.position = Vector3.left * 10000;
            _dicCached = new Dictionary<string, Queue<GameObject>>();
        }
        
        #endregion

        public GameObject Pop(string key)
        {
            if (_dicCached.ContainsKey(key) && _dicCached[key].Count > 0)
            {
                return _dicCached[key].Dequeue();
            }
            else
            {
                return null;
            }
        }

        public void Cache(string key, GameObject go)
        {
            go.transform.parent = _goRoot.transform;
            go.transform.localPosition = Vector3.zero;

            if (string.IsNullOrEmpty(key))
            {
                key = go.name;
            }
            
            if (_dicCached.ContainsKey(key))
            {
                _dicCached[key].Enqueue(go);
            }
            else
            {
                var q = new Queue<GameObject>();
                q.Enqueue(go);
                _dicCached.Add(key, q);
            }
        }
        
        public void Cache(GameObject go)
        {
            string key = go.name;
            Cache(key, go);
        }
    }
}