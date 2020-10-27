public class Node<T>
{
    private T data; //数据域
    
    //构造器
    public Node(T v)
    {
        data = v;
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
}