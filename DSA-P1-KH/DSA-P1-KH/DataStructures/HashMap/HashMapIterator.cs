using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.DataStructures.HashMap;

public class HashMapIterator<TKey, TValue> : IMyIterator<KeyValuePair<TKey, TValue>>
{
    private readonly List<KeyValuePair<TKey, TValue>> _items;
    private int _index = 0;

    public HashMapIterator(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        _items = new List<KeyValuePair<TKey, TValue>>(collection);
    }

    public bool HasNext()
    {
        return _index < _items.Count;
    }

    public KeyValuePair<TKey, TValue> Next()
    {
        return _items[_index++];
    }

    public void Reset()
    {
        _index = 0;
    }
}