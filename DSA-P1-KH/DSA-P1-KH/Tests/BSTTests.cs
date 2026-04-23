using DSA_P1_KH.DataStructures.BST;
using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class BSTTests
{
    public static void Run()
    {
        TestHelper.StartSection("BST Tests");

        var tree = new MyBST<int>();

        // Tests Add and Count.
        tree.Add(10);
        tree.Add(5);
        tree.Add(15);
        tree.Add(3);
        tree.Add(7);
        tree.Add(20);
        TestHelper.PrintResult("BST Add + Count", 6, tree.Count);

        // Tests Iterator sorted traversal.
        TestHelper.PrintResult("BST Iterator Sorted", "3 5 7 10 15 20",
            TestHelper.IteratorToString(() => tree.GetIterator()));

        // Tests Contains existing and missing values.
        TestHelper.PrintResult("BST Contains Existing", true, tree.Contains(7));
        TestHelper.PrintResult("BST Contains Missing", false, tree.Contains(99));

        // Tests Remove leaf node.
        tree.Remove(7);
        TestHelper.PrintResult("BST Remove Leaf Count", 5, tree.Count);
        TestHelper.PrintResult("BST Remove Leaf Order", "3 5 10 15 20",
            TestHelper.EnumerableToString(tree));

        // Tests Remove root node.
        tree.Remove(10);
        TestHelper.PrintResult("BST Remove Root Count", 4, tree.Count);
        TestHelper.PrintResult("BST Remove Root Order", "3 5 15 20",
            TestHelper.EnumerableToString(tree));

        // Tests FindBy.
        int found = tree.FindBy(15, (x, key) => x == key);
        TestHelper.PrintResult("BST FindBy", 15, found);

        // Tests Filter.
        var filtered = tree.Filter(x => x > 10);
        TestHelper.PrintResult("BST Filter", "15 20", TestHelper.EnumerableToString(filtered));

        // Tests Reduce.
        int sum = tree.Reduce(0, (acc, x) => acc + x);
        TestHelper.PrintResult("BST Reduce", 43, sum);

        // Tests Iterator Reset.
        var it = tree.GetIterator();
        while (it.HasNext()) it.Next();
        it.Reset();

        var afterReset = new List<int>();
        while (it.HasNext())
            afterReset.Add(it.Next());

        TestHelper.PrintResult("BST Iterator Reset", "3 5 15 20",
            string.Join(" ", afterReset));

        AnsiConsole.WriteLine();
        TestHelper.EndSection();
    }
}