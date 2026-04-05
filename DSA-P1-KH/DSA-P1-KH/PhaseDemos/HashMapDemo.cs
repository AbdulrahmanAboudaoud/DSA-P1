using DSA_P1_KH.DataStructures.HashMap;

namespace DSA_P1_KH.PhaseDemos;

public static class HashMapDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== HashMap Demo (MyHashMap) ===\n");

        var map = new MyHashMap<int, string>();


        // ADD (Put)
        Console.WriteLine("Adding key-value pairs:");

        map.Put(1, "Task A");
        map.Put(2, "Task B");
        map.Put(3, "Task C");
        map.Put(12, "Collision Example");

        Console.WriteLine("Added: 1->A, 2->B, 3->C, 12->Collision\n");


        // GET
        Console.WriteLine("Retrieving values:");

        Console.WriteLine($"Key 1 -> {map.Get(1)}");
        Console.WriteLine($"Key 2 -> {map.Get(2)}");
        Console.WriteLine($"Key 3 -> {map.Get(3)}");
        Console.WriteLine($"Key 12 -> {map.Get(12)}");


        // UPDATE
        Console.WriteLine("\nUpdating key 2:");

        map.Put(2, "Updated Task B");

        Console.WriteLine($"Key 2 -> {map.Get(2)}");


        // NOT FOUND
        Console.WriteLine("\nTrying non-existing key:");

        var result = map.Get(99);

        Console.WriteLine(result == null
            ? "Key 99 not found"
            : result);

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }
}