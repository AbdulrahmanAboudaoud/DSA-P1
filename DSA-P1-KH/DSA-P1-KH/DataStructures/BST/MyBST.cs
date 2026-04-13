using DSA_P1_KH.DataStructures.Interfaces;
using System.Collections;

namespace DSA_P1_KH.DataStructures.BST;

public class MyBST<T> : IMyCollection<T> where T : IComparable<T>
{
    private MyBSTNode<T>? _root;

    public int Count { get; private set; }
    public bool Dirty { get; set; }

    // Adds a new item into the BST.
    public void Add(T item)
    {
        _root = InsertRecursive(_root, item);
        Count++;
        Dirty = true;
    }

    private MyBSTNode<T>? InsertRecursive(MyBSTNode<T>? node, T value)
    {
        if (node == null)
            return new MyBSTNode<T>(value);

        if (value.CompareTo(node.Data) < 0)
            node.Left = InsertRecursive(node.Left, value);
        else
            node.Right = InsertRecursive(node.Right, value);

        return node;
    }

    // Removes an item from the BST.
    public void Remove(T item)
    {
        _root = RemoveRecursive(_root, item);
        Dirty = true;
    }

    private MyBSTNode<T>? RemoveRecursive(MyBSTNode<T>? node, T value)
    {
        if (node == null)
            return null;

        int cmp = value.CompareTo(node.Data);

        if (cmp < 0)
        {
            node.Left = RemoveRecursive(node.Left, value);
        }
        else if (cmp > 0)
        {
            node.Right = RemoveRecursive(node.Right, value);
        }
        else
        {
            if (node.Left == null)
            {
                Count--;
                return node.Right;
            }

            if (node.Right == null)
            {
                Count--;
                return node.Left;
            }

            var minLargerNode = FindMin(node.Right);
            node.Data = minLargerNode.Data;
            node.Right = RemoveRecursive(node.Right, minLargerNode.Data);
        }

        return node;
    }

    private MyBSTNode<T> FindMin(MyBSTNode<T> node)
    {
        while (node.Left != null)
            node = node.Left;

        return node;
    }

    // Checks whether a value exists in the BST.
    public bool Contains(T value)
    {
        return ContainsRecursive(_root, value);
    }

    private bool ContainsRecursive(MyBSTNode<T>? node, T value)
    {
        if (node == null)
            return false;

        int cmp = value.CompareTo(node.Data);

        if (cmp == 0)
            return true;

        if (cmp < 0)
            return ContainsRecursive(node.Left, value);

        return ContainsRecursive(node.Right, value);
    }

    // Finds an item matching a custom comparison rule.
    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        foreach (var item in this)
        {
            if (comparer(item, key))
                return item;
        }

        return default!;
    }

    // Returns a filtered BST containing matching items.
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyBST<T>();

        foreach (var item in this)
        {
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    // BST is already naturally sorted.
    public void Sort(Comparison<T> comparison)
    {
    }

    // Reduces all items into one accumulated value.
    public R Reduce<R>(Func<R, T, R> accumulator)
    {
        return Reduce(default!, accumulator);
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R result = initial;

        foreach (var item in this)
            result = accumulator(result, item);

        return result;
    }

    // Returns iterator for traversing BST.
    public IMyIterator<T> GetIterator()
    {
        return new BSTIterator<T>(this);
    }

    // Returns enumerator for foreach support.
    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(_root).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Traverses BST in sorted in-order sequence.
    private IEnumerable<T> InOrderTraversal(MyBSTNode<T>? node)
    {
        if (node == null)
            yield break;

        foreach (var left in InOrderTraversal(node.Left))
            yield return left;

        yield return node.Data;

        foreach (var right in InOrderTraversal(node.Right))
            yield return right;
    }
}