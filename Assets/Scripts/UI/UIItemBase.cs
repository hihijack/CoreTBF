using UnityEngine;
using System.Collections;
using DefaultNamespace;

public class UIItemBase : MonoBehaviour
{
    public virtual void Refresh()
    {

    }

    public static T Create<T>(Transform parent, GameObject pfb) where T : UIItemBase
    {
        var go = GameUtil.PopOrInst(pfb);
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<T>();
        return t;
    }

    public virtual void Cache()
    {
        GoPool.Inst.Cache(gameObject);
    }

    public virtual void OnAwake() { }

    private void Awake()
    {
        OnAwake();
    }
}
