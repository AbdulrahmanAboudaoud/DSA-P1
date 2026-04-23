using DSA_P1_KH.DataStructures.ArrayList;
using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class ArrayListTests
{
    public static void Run()
    {
        TestHelper.StartSection("ArrayList Tests");

        var list = new MyArrayList<int>();

        // Tests Add and Count.
        list.Add(10);
        list.Add(20);
        list.Add(30);
        TestHelper.PrintResult("ArrayList Add + Count", 3, list.Count);

        // Tests Remove.
        list.Remove(20);
        TestHelper.PrintResult("ArrayList Remove Count", 2, list.Count);
        TestHelper.PrintResult("ArrayList Remove Order", "10 30", TestHelper.EnumerableToString(list));

        // Tests FindBy.
        int found = list.FindBy(30, (x, key) => x == key);
        TestHelper.PrintResult("ArrayList FindBy", 30, found);

        // Tests Filter.
        var filtered = list.Filter(x => x > 10);
        TestHelper.PrintResult("ArrayList Filter", "30", TestHelper.EnumerableToString(filtered));

        // Tests Sort.
        list.Add(5);
        list.Sort((a, b) => a.CompareTo(b));
        TestHelper.PrintResult("ArrayList Sort", "5 10 30", TestHelper.EnumerableToString(list));

        // Tests Reduce.
        int sum = list.Reduce(0, (acc, x) => acc + x);
        TestHelper.PrintResult("ArrayList Reduce", 45, sum);

        // Tests Iterator.
        TestHelper.PrintResult("ArrayList Iterator", "5 10 30",
            TestHelper.IteratorToString(() => list.GetIterator()));

        // Tests Iterator Reset.
        var it = list.GetIterator();
        while (it.HasNext()) it.Next();
        it.Reset();

        var afterReset = new List<int>();
        while (it.HasNext())
            afterReset.Add(it.Next());

        TestHelper.PrintResult("ArrayList Iterator Reset", "5 10 30",
            string.Join(" ", afterReset));

        AnsiConsole.WriteLine();
        TestHelper.EndSection();
    }
}