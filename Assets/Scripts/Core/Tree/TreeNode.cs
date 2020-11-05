using System.Collections.Generic;

public class TreeNode<T>
{
    private T data;
    public List<TreeNode<T>> childs{private set; get;}

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
    }
}