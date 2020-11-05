using System.Collections.Generic;

public class Tree<T>
{
    public TreeNode<T> root{private set; get;}

    public Tree(TreeNode<T> root)
    {
       this.root = root;
    }
}