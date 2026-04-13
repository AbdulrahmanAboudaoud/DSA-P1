// ======================= HashMapDemo.cs =======================
// Full demo that tests every HashMap feature

using DSA_P1_KH.DataStructures.HashMap;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.PhaseDemos;

public static class HashMapDemo
{
    public static void Run()
    {
        Console.Clear();
        Console.WriteLine("=== FULL HASHMAP DEMO ===\n");

        var map = new MyHashMap<int, string>();


        // TEST 1: Add() → insert key-value pairs
        Console.WriteLine("TEST 1: Adding entries");

        map.Add(new KeyValuePair<int, string>(1, "Task A"));
        map.Add(new KeyValuePair<int, string>(2, "Task B"));
        map.Add(new KeyValuePair<int, string>(3, "Task C"));
        map.Add(new KeyValuePair<int, string>(12, "Collision Example"));

        Console.WriteLine($"Count after add: {map.Count}\n");


        // TEST 2: Get() → retrieve existing keys
        Console.WriteLine("TEST 2: Get existing keys");
        Console.WriteLine($"1 -> {map.Get(1)}");
        Console.WriteLine($"2 -> {map.Get(2)}");
        Console.WriteLine($"12 -> {map.Get(12)}\n");


        // TEST 3: Get() missing key
        Console.WriteLine("TEST 3: Get missing key");
        Console.WriteLine($"99 -> {map.Get(99)}\n");


        // TEST 4: Put() update existing key
        Console.WriteLine("TEST 4: Update existing key");
        map.Put(2, "Updated Task B");
        Console.WriteLine($"2 -> {map.Get(2)}\n");


        // TEST 5: Collision handling
        Console.WriteLine("TEST 5: Collision test");
        Console.WriteLine("Keys 2 and 12 should both exist:");
        Console.WriteLine($"2 -> {map.Get(2)}");
        Console.WriteLine($"12 -> {map.Get(12)}\n");


        // TEST 6: Iterator traversal
        Console.WriteLine("TEST 6: Iterator traversal");

        var it = map.GetIterator();

        while (it.HasNext())
        {
            var pair = it.Next();
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }

        Console.WriteLine();


        // TEST 7: Reset iterator
        Console.WriteLine("TEST 7: Iterator reset");
        it.Reset();

        while (it.HasNext())
        {
            var pair = it.Next();
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }

        Console.WriteLine();


        // TEST 8: Remove existing key
        Console.WriteLine("TEST 8: Remove existing key (2)");
        map.Remove(new KeyValuePair<int, string>(2, "Updated Task B"));

        PrintMap(map);


        // TEST 9: Remove missing key
        Console.WriteLine("\nTEST 9: Remove missing key (99)");
        map.Remove(new KeyValuePair<int, string>(99, "Missing"));

        PrintMap(map);


        // TEST 10: FindBy()
        Console.WriteLine("\nTEST 10: FindBy()");
        var found = map.FindBy(3, (pair, key) => pair.Key == key);
        Console.WriteLine($"{found.Key} -> {found.Value}\n");


        // TEST 11: Filter()
        Console.WriteLine("TEST 11: Filter(keys > 2)");
        var filtered = map.Filter(pair => pair.Key > 2);

        foreach (var pair in filtered)
            Console.WriteLine($"{pair.Key} -> {pair.Value}");

        Console.WriteLine();


        // TEST 12: Reduce()
        Console.WriteLine("TEST 12: Reduce(total string length)");
        int totalLength = map.Reduce(0, (acc, pair) => acc + pair.Value.Length);
        Console.WriteLine($"Total string length = {totalLength}\n");


        Console.WriteLine("HashMap Demo Finished.");
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }

    private static void PrintMap(MyHashMap<int, string> map)
    {
        var it = map.GetIterator();

        while (it.HasNext())
        {
            var pair = it.Next();
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }

        Console.WriteLine($"Count: {map.Count}");
    }
}