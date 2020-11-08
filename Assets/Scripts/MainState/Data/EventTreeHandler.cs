using System;
using SimpleJSON;

public class EventTreeHandler
{
    public Tree<EventBaseData> tree{private set; get;}

    TreeNode<EventBaseData> curNode;

    /// <summary>
    /// 选项触发
    /// </summary>
    /// <param name="index"></param>
    /// <returns>目标节点</returns>
    public TreeNode<EventBaseData> TriSelection(int index)
    {
        var targetNode = curNode.childs[index];
        curNode = targetNode;
        TriTreeNode(curNode);
        return curNode;
    }

    public void TriRoot()
    {
        curNode = tree.root;
        TriTreeNode(tree.root);
    }

    /// <summary>
    /// 触发节点
    /// </summary>
    /// <param name="root"></param>
    private void TriTreeNode(TreeNode<EventBaseData> node)
    {
        for (int i = 0; i < node.Data.jsonEvents.Count; i++)
        {
           var eventT = node.Data.jsonEvents[i];
           EventProcessor.Inst.FireEvent(eventT["event"], node.Data, eventT); 
        }
    }

    public void GenEventTree(EventBaseData rootEvent)
    {
        tree = new Tree<EventBaseData>(new TreeNode<EventBaseData>(rootEvent));
        //递归生产树
        AddChildForTreeNode(tree.root);
    }

    /// <summary>
    /// 递归生产树
    /// </summary>
    /// <param name="treeNode"></param>
    void AddChildForTreeNode(TreeNode<EventBaseData> treeNode)
    {
        if (treeNode.Data.jsonChilds == null || treeNode.Data.jsonChilds.Count == 0)
        {
            return;
        }

        for (int i = 0; i < treeNode.Data.jsonChilds.Count; i++)
        {
            int childId = treeNode.Data.jsonChilds[i].AsInt;
            treeNode.AddChild(new TreeNode<EventBaseData>(EventDataer.Inst.Get(childId)));
        }
        foreach (var child in treeNode.childs)
        {
            AddChildForTreeNode(child);   
        }
    }
}