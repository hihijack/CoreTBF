using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public static class GameUtil
{
    public static T GetOrAdd<T>(GameObject go)  where T : Component
    {
        var t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }

        return t;
    }

    public static void SetSprite(Image image, string spriteName)
    {
        var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
        image.sprite = sprite;
    }

    public static void CacheChildren(GameObject go)
    {
        for (int i = go.transform.childCount - 1; i >= 0; i--)
        {
            GoPool.Inst.Cache(go.transform.GetChild(i).gameObject);
        } 
    }

    public static GameObject PopOrInst(GameObject pfb)
    {
        var go = GoPool.Inst.Pop(pfb.name);
        if (go == null)
        {
            go = Object.Instantiate(pfb);
            go.name = pfb.name;
        }
        return go;
    }
    
    public static T GetComponentInChildByPath<T> (GameObject gobjParent, string path) where T : Component{
        Component r = null;
        if (gobjParent == null){
            return null;
        }
        var tf = gobjParent.transform.Find(path);
        if (tf != null){
            r = tf.GetComponent<T>();
        }
        return (T) r;
    }
}