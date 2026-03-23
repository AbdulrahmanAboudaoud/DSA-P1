using DSA_P1_KH.DataStructures.ArrayList;

namespace DSA_P1_KH.PhaseDemos;

public static class DynamicArrayDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== Dynamic Array Demo (MyArrayList) ===\n");

        var list = new MyArrayList<int>();

        list.Add(5);
        list.Add(7);
        list.Add(9);
        list.Add(11);
        list.Add(13); // resize should happen

        Console.WriteLine("Items:");

        foreach (var x in list)
            Console.WriteLine(x);

        list.Remove(9);

        Console.WriteLine("\nAfter remove:");

        foreach (var x in list)
            Console.WriteLine(x);

        var filtered = list.Filter(x => x > 7);

        Console.WriteLine("\nFiltered (>7):");

        foreach (var x in filtered)
            Console.WriteLine(x);

        int sum = list.Reduce(0, (acc, x) => acc + x);
        Console.WriteLine($"\nSum = {sum}");

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }
}