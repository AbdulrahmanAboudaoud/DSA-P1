using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class TestHelper
{
    private static Table? _table;
    private static int _testNumber = 1;

    // Start one table for a structure
    public static void StartSection(string title)
    {
        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new Rule($"[yellow bold]{title}[/]")
                .RuleStyle("grey")
                .Centered()
        );

        _table = new Table()
            .Border(TableBorder.Rounded)
            .Expand()
            .AddColumn("[grey]#[/]")
            .AddColumn("[grey]Test[/]")
            .AddColumn("[grey]Result[/]")
            .AddColumn("[grey]Expected[/]")
            .AddColumn("[grey]Actual[/]");

        _testNumber = 1;
    }

    // Add one row
    public static void PrintResult<T>(string testName, T expected, T actual)
    {
        if (_table == null)
            return;

        bool passed = Equals(expected, actual);

        string result = passed
            ? "[black on green] PASS [/]"
            : "[white on red] FAIL [/]";

        _table.AddRow(
            _testNumber.ToString(),
            testName,
            result,
            expected?.ToString() ?? "null",
            actual?.ToString() ?? "null"
        );

        _testNumber++;
    }

    // Finish table
    public static void EndSection()
    {
        if (_table != null)
        {
            AnsiConsole.Write(_table);
            AnsiConsole.WriteLine();
        }
    }

    public static string IteratorToString<T>(
        Func<DSA_P1_KH.DataStructures.Interfaces.IMyIterator<T>> iteratorFactory)
    {
        var it = iteratorFactory();
        var parts = new List<string>();

        while (it.HasNext())
            parts.Add(it.Next()?.ToString() ?? "null");

        return string.Join(" ", parts);
    }

    public static string EnumerableToString<T>(IEnumerable<T> items)
    {
        return string.Join(" ", items.Select(x => x?.ToString() ?? "null"));
    }
}