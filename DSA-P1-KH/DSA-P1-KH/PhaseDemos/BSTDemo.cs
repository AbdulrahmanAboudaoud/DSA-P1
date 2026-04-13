// ======================= BSTDemo.cs =======================
// Full demo that tests every BST feature

using DSA_P1_KH.DataStructures.BST;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.PhaseDemos;

public static class BSTDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== FULL BST DEMO ===\n");

        var tree = new MyBST<int>();


        // TEST 1: Add() → insert values into BST
        Console.WriteLine("TEST 1: Adding values");
        int[] values = { 10, 5, 15, 3, 7, 20 };

        foreach (var value in values)
        {
            Console.WriteLine($"Adding {value}");
            tree.Add(value);
        }

        Console.WriteLine($"Count after add: {tree.Count}\n");


        // TEST 2: Iterator traversal → verifies sorted in-order traversal
        Console.WriteLine("TEST 2: Iterator traversal (sorted order)");
        var it = tree.GetIterator();

        while (it.HasNext())
            Console.Write(it.Next() + " ");

        Console.WriteLine("\n");


        // TEST 3: Reset() → restart iterator from beginning
        Console.WriteLine("TEST 3: Iterator reset");
        it.Reset();

        while (it.HasNext())
            Console.Write(it.Next() + " ");

        Console.WriteLine("\n");


        // TEST 4: Contains() → search existing and missing values
        Console.WriteLine("TEST 4: Search / Contains");
        Console.WriteLine($"Contains 7 → {tree.Contains(7)}");
        Console.WriteLine($"Contains 99 → {tree.Contains(99)}\n");


        // TEST 5: Remove leaf node
        Console.WriteLine("TEST 5: Remove leaf node (7)");
        tree.Remove(7);

        PrintTree(tree);


        // TEST 6: Remove root node
        Console.WriteLine("\nTEST 6: Remove root node (10)");
        tree.Remove(10);

        PrintTree(tree);


        // TEST 7: Remove missing value
        Console.WriteLine("\nTEST 7: Remove missing value (99)");
        tree.Remove(99);

        PrintTree(tree);


        // TEST 8: FindBy() → find exact item
        Console.WriteLine("\nTEST 8: FindBy()");
        int found = tree.FindBy(15, (x, y) => x == y);
        Console.WriteLine($"Found: {found}\n");


        // TEST 9: Filter() → values > 10
        Console.WriteLine("TEST 9: Filter(x > 10)");
        var filtered = tree.Filter(x => x > 10);

        foreach (var item in filtered)
            Console.Write(item + " ");

        Console.WriteLine("\n");


        // TEST 10: Reduce() → sum all values
        Console.WriteLine("TEST 10: Reduce(sum)");
        int sum = tree.Reduce(0, (acc, x) => acc + x);
        Console.WriteLine($"Sum = {sum}\n");


        Console.WriteLine("BST Demo Finished.");
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }

    private static void PrintTree(MyBST<int> tree)
    {
        var it = tree.GetIterator();

        while (it.HasNext())
            Console.Write(it.Next() + " ");

        Console.WriteLine();
        Console.WriteLine($"Count: {tree.Count}");
    }
}