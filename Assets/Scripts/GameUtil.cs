using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using Sirenix.Utilities;
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

    public static void SetSprite(this Image image, string spriteName)
    {
        var sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
        image.sprite = sprite;
    }

    internal static GameObject FindChild(GameObject gameObject, string path)
    {
        return gameObject.transform.Find(path).gameObject;
    }

    public static void SetSpriteNativeSize(this Image img, float width, float height)
    {

        float asspt = img.sprite.rect.width / img.sprite.rect.height;
        if (width > 0)
        {
            img.rectTransform.sizeDelta = new Vector2(width, width / asspt);
        }
        else if (height > 0)
        {
            img.rectTransform.sizeDelta = new Vector2(height * asspt, height);
        }
    }

    public static void SetImageAlpha(this Image img, float alpha)
    {
        var c = img.color;
        c.a = alpha;
        img.color = c;
    }

    public static void SetSprite(Image image, string path, string spriteName)
    {
        var sprite = Resources.Load<Sprite>($"{path}/{spriteName}");
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

    public static Rect LimitRectIn(Rect ori, Rect limit)
    {
        Rect r = ori;
        if (ori.yMin < limit.yMin)
        {
            r = r.AddPosition(0, limit.yMin - ori.yMin);
        }
        if (ori.yMax > limit.yMax)
        {
            r = r.AddPosition(0, limit.yMax - ori.yMax);
        }
        if (ori.xMin < limit.xMin)
        {
            r = r.AddPosition(limit.xMin - ori.xMin, 0);
        }
        if (ori.xMax > limit.xMax)
        {
            r = r.AddPosition(limit.xMax - ori.xMax, 0);
        }
        return r;
    }

    public static string ToTitleCase(string s)
    {
        return s.Substring(0, 1).ToUpper() + s.Substring(1); 
    }
    
    /// <summary>
    /// 从给定范围内选择N个不重复随机数
    /// </summary>
    /// <returns>The random numbers set.</returns>
    /// <param name="min">Minimum.</param>
    /// <param name="max">Max.</param>
    /// <param name="count">Count.</param>
	public static HashSet<int> GetRandomNumbersSet(int min, int max, int count)
    {
	    if (count > (max - min + 1) || max < min) {
			return null;  
        } 
        HashSet<int> set = new HashSet<int>();
        while (set.Count < count)
        {
            set.Add(UnityEngine.Random.Range(min, max + 1));
        }
		return set;
    }

    /// <summary>
    /// 列表转成a,b,c 字符串
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    public static string GetStringLst(List<int> lst) 
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lst.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(",");
            }
            sb.Append(lst[i]);
        }
        return sb.ToString();
    }


    /// <summary>
    /// 列表转成a,b,c 字符串
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    public static string GetStringLst(List<string> lst)
    {
        if (lst == null)
        {
            return "";
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lst.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(",");
            }
            sb.Append(lst[i]);
        }
        return sb.ToString();
    }

    public static List<T> RandomSortList<T>(List<T> ListT)
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
}