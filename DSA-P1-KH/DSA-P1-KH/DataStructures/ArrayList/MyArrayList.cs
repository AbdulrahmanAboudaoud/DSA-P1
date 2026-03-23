using System.Collections;
using DSA_P1_KH.DataStructures.Interfaces;
namespace DSA_P1_KH.DataStructures.ArrayList;

public class MyArrayList<T> : IMyCollection<T>, IEnumerable<T>
{
    private T[] _data;
    private int _count;

    public bool Dirty { get; set; }

    public MyArrayList(int capacity = 4)
    {
        _data = new T[capacity];
        _count = 0;
        Dirty = false;
    }

    public int Count => _count;

    public void Add(T item)
    {
        if (_count == _data.Length)
            Resize();

        _data[_count++] = item;
        Dirty = true;
    }

    public void Remove(T item)
    {
        int index = Array.IndexOf(_data, item, 0, _count);

        if (index == -1)
            return;

        for (int i = index; i < _count - 1; i++)
            _data[i] = _data[i + 1];

        _count--;
        Dirty = true;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < _count; i++)
        {
            if (comparer(_data[i], key))
                return _data[i];
        }

        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyArrayList<T>();

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_data[i]))
                result.Add(_data[i]);
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        Array.Sort(_data, 0, _count, Comparer<T>.Create(comparison));
        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        R result = default!;

        for (int i = 0; i < _count; i++)
            result = accumulator(result, _data[i]);

        return result;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        for (int i = 0; i < _count; i++)
            result = accumulator(result, _data[i]);

        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new MyArrayIterator<T>(_data, _count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _data[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Resize()
    {
        T[] newArr = new T[_data.Length * 2];

        for (int i = 0; i < _data.Length; i++)
            newArr[i] = _data[i];

        _data = newArr;
    }
}