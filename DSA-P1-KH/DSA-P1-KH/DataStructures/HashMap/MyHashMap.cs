using System.Collections;
using DSA_P1_KH.DataStructures.Interfaces;
using DSA_P1_KH.DataStructures.LinkedList;

namespace DSA_P1_KH.DataStructures.HashMap;

public class MyHashMap<TKey, TValue> : IMyCollection<KeyValuePair<TKey, TValue>>
{
    private class Entry
    {
        public TKey Key;
        public TValue Value;

        public Entry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    private readonly MyLinkedList<Entry>?[] _buckets;
    private readonly int _capacity = 16;

    public int Count { get; private set; }
    public bool Dirty { get; set; }

    public MyHashMap()
    {
        _buckets = new MyLinkedList<Entry>?[_capacity];
    }

    private int GetIndex(TKey key)
    {
        return Math.Abs(key!.GetHashCode()) % _capacity;
    }

    private Entry? FindEntry(TKey key)
    {
        int index = GetIndex(key);
        var chain = _buckets[index];

        if (chain == null)
            return null;

        foreach (var entry in chain)
        {
            if (entry.Key!.Equals(key))
                return entry;
        }

        return null;
    }

    public void Put(TKey key, TValue value)
    {
        var existing = FindEntry(key);

        if (existing != null)
        {
            existing.Value = value;
            Dirty = true;
            return;
        }

        int index = GetIndex(key);

        if (_buckets[index] == null)
            _buckets[index] = new MyLinkedList<Entry>();

        _buckets[index]!.Add(new Entry(key, value));
        Count++;
        Dirty = true;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Put(item.Key, item.Value);
    }

    public TValue? Get(TKey key)
    {
        var entry = FindEntry(key);
        return entry == null ? default : entry.Value;
    }

    public void Remove(KeyValuePair<TKey, TValue> item)
    {
        var entry = FindEntry(item.Key);

        if (entry == null)
            return;

        _buckets[GetIndex(item.Key)]!.Remove(entry);
        Count--;
        Dirty = true;
    }

    public KeyValuePair<TKey, TValue> FindBy<K>(K key, Func<KeyValuePair<TKey, TValue>, K, bool> comparer)
    {
        foreach (var item in this)
        {
            if (comparer(item, key))
                return item;
        }

        return default;
    }

    public IMyCollection<KeyValuePair<TKey, TValue>> Filter(Func<KeyValuePair<TKey, TValue>, bool> predicate)
    {
        var result = new MyHashMap<TKey, TValue>();

        foreach (var item in this)
        {
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public void Sort(Comparison<KeyValuePair<TKey, TValue>> comparison)
    {
    }

    public R Reduce<R>(Func<R, KeyValuePair<TKey, TValue>, R> accumulator)
    {
        return Reduce(default!, accumulator);
    }

    public R Reduce<R>(R initial, Func<R, KeyValuePair<TKey, TValue>, R> accumulator)
    {
        R result = initial;

        foreach (var item in this)
            result = accumulator(result, item);

        return result;
    }

    public IMyIterator<KeyValuePair<TKey, TValue>> GetIterator()
    {
        return new HashMapIterator<TKey, TValue>(this);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var chain in _buckets)
        {
            if (chain == null)
                continue;

            foreach (var entry in chain)
                yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}