using DSA_P1_KH.DataStructures.HashMap;
using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class HashMapTests
{
    public static void Run()
    {
        TestHelper.StartSection("HashMap Tests");

        var map = new MyHashMap<int, string>();

        // Tests Add through Put and Count.
        map.Put(1, "Task A");
        map.Put(2, "Task B");
        map.Put(3, "Task C");
        map.Put(12, "Collision Example");
        TestHelper.PrintResult("HashMap Add + Count", 4, map.Count);

        // Tests Get.
        TestHelper.PrintResult("HashMap Get Existing 1", "Task A", map.Get(1));
        TestHelper.PrintResult("HashMap Get Existing 12", "Collision Example", map.Get(12));
        TestHelper.PrintResult("HashMap Get Missing", null, map.Get(99));

        // Tests Update existing key.
        map.Put(2, "Updated Task B");
        TestHelper.PrintResult("HashMap Update Existing", "Updated Task B", map.Get(2));

        // Tests collision handling.
        TestHelper.PrintResult("HashMap Collision Keep First", "Updated Task B", map.Get(2));
        TestHelper.PrintResult("HashMap Collision Keep Second", "Collision Example", map.Get(12));

        // Tests FindBy.
        var found = map.FindBy(3, (pair, key) => pair.Key == key);
        TestHelper.PrintResult("HashMap FindBy Key", "Task C", found.Value);

        // Tests Filter.
        var filtered = map.Filter(pair => pair.Key > 2);
        TestHelper.PrintResult("HashMap Filter Count", 2, filtered.Count);

        // Tests Reduce.
        int totalLength = map.Reduce(0, (acc, pair) => acc + pair.Value.Length);
        TestHelper.PrintResult("HashMap Reduce", 43, totalLength);

        // Tests Remove.
        map.Remove(new KeyValuePair<int, string>(2, "Updated Task B"));
        TestHelper.PrintResult("HashMap Remove Count", 3, map.Count);
        TestHelper.PrintResult("HashMap Remove Missing Value", null, map.Get(2));

        // Tests Iterator contains all remaining keys.
        var iterated = new List<string>();
        var it = map.GetIterator();

        while (it.HasNext())
        {
            var pair = it.Next();
            iterated.Add($"{pair.Key}:{pair.Value}");
        }

        iterated.Sort();
        TestHelper.PrintResult("HashMap Iterator", "1:Task A 12:Collision Example 3:Task C",
            string.Join(" ", iterated));

        // Tests Iterator Reset.
        it.Reset();
        var resetItems = new List<string>();

        while (it.HasNext())
        {
            var pair = it.Next();
            resetItems.Add($"{pair.Key}:{pair.Value}");
        }

        resetItems.Sort();
        TestHelper.PrintResult("HashMap Iterator Reset", "1:Task A 12:Collision Example 3:Task C",
            string.Join(" ", resetItems));

        AnsiConsole.WriteLine();
        TestHelper.EndSection();
    }
}