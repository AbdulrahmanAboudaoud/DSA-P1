using DSA_P1_KH.DataStructures.LinkedList;
using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class LinkedListTests
{
    public static void Run()
    {
        TestHelper.StartSection("LinkedList Tests");

        var list = new MyLinkedList<int>();

        // Tests Add and Count.
        list.Add(10);
        list.Add(20);
        list.Add(30);
        TestHelper.PrintResult("LinkedList Add + Count", 3, list.Count);

        // Tests Remove.
        list.Remove(20);
        TestHelper.PrintResult("LinkedList Remove Count", 2, list.Count);
        TestHelper.PrintResult("LinkedList Remove Order", "10 30", TestHelper.EnumerableToString(list));

        // Tests FindBy.
        int found = list.FindBy(30, (x, key) => x == key);
        TestHelper.PrintResult("LinkedList FindBy", 30, found);

        // Tests Filter.
        var filtered = list.Filter(x => x > 10);
        TestHelper.PrintResult("LinkedList Filter", "30", TestHelper.EnumerableToString(filtered));

        // Tests Sort.
        list.Add(5);
        list.Sort((a, b) => a.CompareTo(b));
        TestHelper.PrintResult("LinkedList Sort", "5 10 30", TestHelper.EnumerableToString(list));

        // Tests Reduce.
        int sum = list.Reduce(0, (acc, x) => acc + x);
        TestHelper.PrintResult("LinkedList Reduce", 45, sum);

        // Tests Iterator.
        TestHelper.PrintResult("LinkedList Iterator", "5 10 30",
            TestHelper.IteratorToString(() => list.GetIterator()));

        // Tests Iterator Reset.
        var it = list.GetIterator();
        while (it.HasNext()) it.Next();
        it.Reset();

        var afterReset = new List<int>();
        while (it.HasNext())
            afterReset.Add(it.Next());

        TestHelper.PrintResult("LinkedList Iterator Reset", "5 10 30",
            string.Join(" ", afterReset));

        AnsiConsole.WriteLine();
        TestHelper.EndSection();
    }
}