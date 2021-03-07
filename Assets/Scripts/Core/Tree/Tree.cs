using System;
using System.Collections.Generic;

public class Tree<T>
{
    public TreeNode<T> root{private set; get;}

    public Tree(TreeNode<T> root)
    {
       this.root = root;
    }

    /// <summary>
    /// ����,����cb����
    /// </summary>
    /// <param name="proc"></param>
    public void Iterator(Action<TreeNode<T>> proc)
    {
        ProcNode(root, proc);
    }

    //�����ڵ㼰�亢�ӽڵ�
    void ProcNode(TreeNode<T> node, Action<TreeNode<T>> cb) 
    {
        cb(node);
        if (node.childs.Count > 0)
        {
            foreach (var item in node.childs)
            {
                ProcNode(item, cb);
            }
        }
    }
}