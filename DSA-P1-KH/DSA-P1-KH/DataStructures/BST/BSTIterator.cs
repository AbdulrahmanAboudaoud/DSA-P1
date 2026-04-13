using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.DataStructures.BST;

public class BSTIterator<T> : IMyIterator<T>
{
    private readonly List<T> _items;
    private int _index = 0;

    public BSTIterator(IEnumerable<T> collection)
    {
        _items = new List<T>(collection);
    }

    public bool HasNext()
    {
        return _index < _items.Count;
    }

    public T Next()
    {
        return _items[_index++];
    }

    public void Reset()
    {
        _index = 0;
    }
}