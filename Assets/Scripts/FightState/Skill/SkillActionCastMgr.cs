public class SkillActionCastMgr
{
    Tree<SkillActionTreeNode> mTreeCast;
    TreeNode<SkillActionTreeNode> mCurNode;

    private void SetRoot(FightSkillProcessorBase proc, ActionContent content)
    {
        TreeNode<SkillActionTreeNode> node = new TreeNode<SkillActionTreeNode>(new SkillActionTreeNode(proc, content));
        mTreeCast = new Tree<SkillActionTreeNode>(node);
        mCurNode = mTreeCast.root;
    }

    public void Clear()
    {
        mTreeCast = null;
        mCurNode = null;
    }

    public void AddChildToCurNode(FightSkillProcessorBase proc, ActionContent content)
    {
        if (mCurNode == null)
        {
            SetRoot(null, null);
        }
        var treeNode = new TreeNode<SkillActionTreeNode>(new SkillActionTreeNode(proc, content));
        mCurNode.AddChild(treeNode);
    }

    //取下个技能释放,并将head指向它
    //public ActionContent GetNextToCast()
    //{
    //    ActionContent content = null;
    //    TreeNode<SkillActionTreeNode> nextTreeNode = null;
    //    //先取子节点
    //    var child = mCurNode.GetChild(0);
    //    if (child != null)
    //    {
    //        nextTreeNode = child;
    //    }
    //    else
    //    {
    //        //无子节点，取同级下个节点
    //        var bother = mCurNode.GetNextBorther();
    //        if (bother != null)
    //        {
    //            nextTreeNode = bother;
    //        }
    //    }

    //    if (nextTreeNode != null)
    //    {
    //        content = nextTreeNode.Data.Content;
    //        //head指向下个节点
    //        mCurNode = nextTreeNode;
    //    }

    //    //无返回空
    //    return content;
    //}
}
