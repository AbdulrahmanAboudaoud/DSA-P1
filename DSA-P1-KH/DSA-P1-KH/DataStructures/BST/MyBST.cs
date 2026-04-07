namespace DSA_P1_KH.DataStructures.BST;

public class MyBST<T> where T : IComparable<T>
{
    private MyBSTNode<T>? _root;


    // INSERT
    public void Insert(T value)
    {
        _root = InsertRecursive(_root, value);
    }

    private MyBSTNode<T> InsertRecursive(MyBSTNode<T>? node, T value)
    {
        if (node == null)
            return new MyBSTNode<T>(value);

        if (value.CompareTo(node.Data) < 0)
            node.Left = InsertRecursive(node.Left, value);
        else
            node.Right = InsertRecursive(node.Right, value);

        return node;
    }


    // SEARCH
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
        else
            return ContainsRecursive(node.Right, value);
    }


    // IN-ORDER (SORTED)
    public void PrintInOrder()
    {
        PrintInOrderRecursive(_root);
    }

    private void PrintInOrderRecursive(MyBSTNode<T>? node)
    {
        if (node == null)
            return;

        PrintInOrderRecursive(node.Left);
        Console.Write(node.Data + " ");
        PrintInOrderRecursive(node.Right);
    }


    // PRE-ORDER
    public void PrintPreOrder()
    {
        PrintPreOrderRecursive(_root);
    }

    private void PrintPreOrderRecursive(MyBSTNode<T>? node)
    {
        if (node == null)
            return;

        Console.Write(node.Data + " ");
        PrintPreOrderRecursive(node.Left);
        PrintPreOrderRecursive(node.Right);
    }


    // POST-ORDER
    public void PrintPostOrder()
    {
        PrintPostOrderRecursive(_root);
    }

    private void PrintPostOrderRecursive(MyBSTNode<T>? node)
    {
        if (node == null)
            return;

        PrintPostOrderRecursive(node.Left);
        PrintPostOrderRecursive(node.Right);
        Console.Write(node.Data + " ");
    }
}