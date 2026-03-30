using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.DataStructures.LinkedList;

public class MyLinkedListIterator<T> : IMyIterator<T>
{
    private MyLinkedListNode<T>? _current;
    private MyLinkedListNode<T>? _head;

    public MyLinkedListIterator(MyLinkedListNode<T>? head)
    {
        _head = head;
        _current = null;
    }

    public bool HasNext()
    {
        if (_current == null)
            return _head != null;

        return _current.Next != null;
    }

    public T Next()
    {
        if (_current == null)
            _current = _head;
        else
            _current = _current!.Next;

        return _current!.Data;
    }

    public void Reset()
    {
        _current = null;
    }
}