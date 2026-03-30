using DSA_P1_KH.DataStructures.LinkedList;

namespace DSA_P1_KH.PhaseDemos;

public static class LinkedListDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== Linked List Demo (MyLinkedList) ===\n");

        var list = new MyLinkedList<int>();

        // ADD
        Console.WriteLine("Adding items:");
        list.Add(5);
        list.Add(7);
        list.Add(9);
        list.Add(11);
        list.Add(13);

        Print(list);

        // REMOVE
        Console.WriteLine("\nRemoving 9:");
        list.Remove(9);

        Print(list);

        // FILTER
        Console.WriteLine("\nFilter (>7):");
        var filtered = list.Filter(x => x > 7);

        Print(filtered);
 
        // REDUCE
        int sum = list.Reduce(0, (acc, x) => acc + x);
        Console.WriteLine($"\nSum = {sum}");
 
        // SORT
        Console.WriteLine("\nSorting descending:");
        list.Sort((a, b) => b.CompareTo(a));

        Print(list);

 
        // ITERATOR DEMO
        Console.WriteLine("\nIterator traversal:");

        var it = list.GetIterator();
        while (it.HasNext())
        {
            Console.Write(it.Next() + " ");
        }

        Console.WriteLine("\n");

        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
    }

    private static void Print(IEnumerable<int> collection)
    {
        foreach (var x in collection)
            Console.WriteLine(x);
    }
}