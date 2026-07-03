using System.Collections;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.DataStructures.LinkedList;

public class MyLinkedList<T> : IMyCollection<T>
{
    private MyLinkedListNode<T>? _head;
    private int _count;

    public int Count => _count;
    public bool Dirty { get; set; }
    public void Add(T item)
    {
        var newNode = new MyLinkedListNode<T>(item);

        if (_head == null)
        {
            _head = newNode;
        }
        else
        {
            var current = _head;
            while (current.Next != null)
                current = current.Next;

            current.Next = newNode;
        }

        _count++;
        Dirty = true;
    }

    public void Remove(T item)
    {
        if (_head == null)
            return;

        if (Equals(_head.Data, item))
        {
            _head = _head.Next;
            _count--;
            Dirty = true;
            return;
        }

        var current = _head;
        while (current.Next != null)
        {
            if (Equals(current.Next.Data, item))
            {
                current.Next = current.Next.Next;
                _count--;
                Dirty = true;
                return;
            }

            current = current.Next;
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        var current = _head;

        while (current != null)
        {
            if (comparer(current.Data, key))
                return current.Data;

            current = current.Next;
        }

        return default!; // not found
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyLinkedList<T>();
        var current = _head;

        while (current != null)
        {
            if (predicate(current.Data))
                result.Add(current.Data);

            current = current.Next;
        }

        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        if (_head == null)
            return;

        bool swapped;

        do
        {
            swapped = false;
            var current = _head;

            while (current.Next != null)
            {
                if (comparison(current.Data, current.Next.Data) > 0)
                {
                    var temp = current.Data;
                    current.Data = current.Next.Data;
                    current.Next.Data = temp;

                    swapped = true;
                }

                current = current.Next;
            }

        } while (swapped);

        Dirty = true;
    }

    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        return Reduce(default!, accumulator);
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;
        var current = _head;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }

        return result;
    }

    public IMyIterator<T> GetIterator()
    {
        return new MyLinkedListIterator<T>(_head);
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}