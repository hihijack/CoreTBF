using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public GameObject PopOrInst(string key, string path)
    {
        var go = Pop(key);
        if (go == null)
        {
            var pfb = Resources.Load<GameObject>(Path.Combine(path, key));
            go = GameObject.Instantiate<GameObject>(pfb);
            go.name = key;
        }
        return go;
    }

    public void Cache(string key, GameObject go)
    {
        go.transform.SetParent(_goRoot.transform, false);
        go.transform.localPosition = Vector3.zero;

        if (string.IsNullOrEmpty(key))
        {
            key = go.name;
        }

        if (_dicCached.ContainsKey(key))
        {
            if (!_dicCached[key].Contains(go))
            {
                //防止重复进队
                _dicCached[key].Enqueue(go);
            }
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