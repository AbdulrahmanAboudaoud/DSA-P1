namespace DSA_P1_KH.DataStructures.HashMap;

public class MyHashMap<TKey, TValue>
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

    public MyHashMap()
    {
        _buckets = new Entry[_capacity][];
    }

    private int GetIndex(TKey key)
    {
        return Math.Abs(key.GetHashCode()) % _capacity;
    }

    public void Put(TKey key, TValue value)
    {
        int index = GetIndex(key);

        if (_buckets[index] == null)
        {
            _buckets[index] = new Entry[1];
            _buckets[index][0] = new Entry(key, value);
            return;
        }

        var bucket = _buckets[index];

        // check if exists
        for (int i = 0; i < bucket.Length; i++)
        {
            if (bucket[i].Key!.Equals(key))
            {
                bucket[i].Value = value;
                return;
            }
        }

        // add new entry
        Entry[] newBucket = new Entry[bucket.Length + 1];

        for (int i = 0; i < bucket.Length; i++)
            newBucket[i] = bucket[i];

        newBucket[bucket.Length] = new Entry(key, value);

        _buckets[index] = newBucket;
    }

    public TValue? Get(TKey key)
    {
        int index = GetIndex(key);

        var bucket = _buckets[index];
        if (bucket == null)
            return default;

        for (int i = 0; i < bucket.Length; i++)
        {
            if (bucket[i].Key!.Equals(key))
                return bucket[i].Value;
        }

        return default;
    }
}