using System;
using System.Collections.Generic;

public class TreeNode<T>
{
    private T data;

    public TreeNode<T> parent { private set; get;}

    public List<TreeNode<T>> childs{private set; get;}
    /// <summary>
    /// 层数
    /// </summary>
    public int layer { private set; get; } 

    /// <summary>
    /// 序号。从0开始
    /// </summary>
    public int index { private set; get; }

      //构造器
    public TreeNode(T v)
    {
        data = v;
        childs = new List<TreeNode<T>>();
    }

     //数据域属性
    public T Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }

    public void AddChild(TreeNode<T> child)
    {
        childs.Add(child);
        child.layer = layer + 1;
        child.index = childs.Count - 1;
        child.parent = this;
    }

    /// <summary>
    /// 是否叶子节点
    /// </summary>
    /// <returns></returns>
    internal bool IsLeafNode()
    {
        return childs == null || childs.Count == 0;
    }
}