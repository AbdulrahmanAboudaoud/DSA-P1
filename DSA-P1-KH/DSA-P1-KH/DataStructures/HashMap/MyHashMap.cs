using DSA_P1_KH.DataStructures.Interfaces;
using System.Collections;

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

    private Entry[][] _buckets;
    private int _capacity = 10;

    public int Count { get; private set; }
    public bool Dirty { get; set; }

    // Initializes empty HashMap buckets.
    public MyHashMap()
    {
        _buckets = new Entry[_capacity][];
    }

    // Calculates bucket index for a given key.
    private int GetIndex(TKey key)
    {
        return Math.Abs(key!.GetHashCode()) % _capacity;
    }

    // Adds a new key-value pair into the HashMap.
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Put(item.Key, item.Value);
    }

    // Inserts or updates a key-value pair.
    public void Put(TKey key, TValue value)
    {
        int index = GetIndex(key);

        if (_buckets[index] == null)
        {
            _buckets[index] = new Entry[1];
            _buckets[index][0] = new Entry(key, value);
            Count++;
            Dirty = true;
            return;
        }

        var bucket = _buckets[index];

        for (int i = 0; i < bucket.Length; i++)
        {
            if (bucket[i].Key!.Equals(key))
            {
                bucket[i].Value = value;
                return;
            }
        }

        Entry[] newBucket = new Entry[bucket.Length + 1];

        for (int i = 0; i < bucket.Length; i++)
            newBucket[i] = bucket[i];

        newBucket[bucket.Length] = new Entry(key, value);

        _buckets[index] = newBucket;
        Count++;
        Dirty = true;
    }

    // Retrieves value by key.
    public TValue? Get(TKey key)
    {
        int index = GetIndex(key);
        var bucket = _buckets[index];

        if (bucket == null)
            return default;

        foreach (var entry in bucket)
        {
            if (entry.Key!.Equals(key))
                return entry.Value;
        }

        return default;
    }

    // Removes a key-value pair from the HashMap.
    public void Remove(KeyValuePair<TKey, TValue> item)
    {
        int index = GetIndex(item.Key);
        var bucket = _buckets[index];

        if (bucket == null)
            return;

        List<Entry> newBucket = new();

        foreach (var entry in bucket)
        {
            if (!entry.Key!.Equals(item.Key))
                newBucket.Add(entry);
        }

        if (newBucket.Count < bucket.Length)
        {
            _buckets[index] = newBucket.ToArray();
            Count--;
            Dirty = true;
        }
    }

    // Finds an entry matching a custom comparison rule.
    public KeyValuePair<TKey, TValue> FindBy<K>(K key, Func<KeyValuePair<TKey, TValue>, K, bool> comparer)
    {
        foreach (var item in this)
        {
            if (comparer(item, key))
                return item;
        }

        return default;
    }

    // Returns filtered HashMap containing matching entries.
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

    // HashMap has no natural sorting order.
    public void Sort(Comparison<KeyValuePair<TKey, TValue>> comparison)
    {
    }

    // Reduces all entries into one accumulated value.
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

    // Returns iterator for traversing HashMap.
    public IMyIterator<KeyValuePair<TKey, TValue>> GetIterator()
    {
        return new HashMapIterator<TKey, TValue>(this);
    }

    // Returns enumerator for foreach support.
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var bucket in _buckets)
        {
            if (bucket == null)
                continue;

            foreach (var entry in bucket)
            {
                yield return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}