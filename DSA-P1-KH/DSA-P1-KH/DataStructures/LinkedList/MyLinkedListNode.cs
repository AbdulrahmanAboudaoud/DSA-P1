namespace DSA_P1_KH.DataStructures.LinkedList;

public class MyLinkedListNode<T>
{
    public T Data;
    public MyLinkedListNode<T>? Next;

    public MyLinkedListNode(T data)
    {
        Data = data;
        Next = null;
    }
}