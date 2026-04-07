namespace DSA_P1_KH.DataStructures.BST;

public class MyBSTNode<T>
{
    public T Data;
    public MyBSTNode<T>? Left;
    public MyBSTNode<T>? Right;

    public MyBSTNode(T data)
    {
        Data = data;
        Left = null;
        Right = null;
    }
}