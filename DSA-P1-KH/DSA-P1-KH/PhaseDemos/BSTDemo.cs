using DSA_P1_KH.DataStructures.BST;

namespace DSA_P1_KH.PhaseDemos;

public static class BSTDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== Binary Search Tree Demo ===\n");

        var tree = new MyBST<int>();

    
        // INSERT
        Console.WriteLine("Inserting values:");

        int[] values = { 10, 5, 15, 3, 7, 20 };

        for (int i = 0; i < values.Length; i++)
        {
            Console.WriteLine($"Insert {values[i]}");
            tree.Insert(values[i]);
        }

    
        // TRAVERSALS
        Console.WriteLine("\nInOrder (Sorted):");
        tree.PrintInOrder();

        Console.WriteLine("\n\nPreOrder:");
        tree.PrintPreOrder();

        Console.WriteLine("\n\nPostOrder:");
        tree.PrintPostOrder();

    
        // SEARCH
        Console.WriteLine("\n\nSearching:");

        Console.WriteLine($"Contains 7 → {tree.Contains(7)}");
        Console.WriteLine($"Contains 99 → {tree.Contains(99)}");

        Console.WriteLine("\n\nTree Structure:");
        Console.WriteLine("        10");
        Console.WriteLine("       /  \\");
        Console.WriteLine("      5    15");
        Console.WriteLine("     / \\     \\");
        Console.WriteLine("    3   7     20");

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }
}