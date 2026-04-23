using Spectre.Console;

namespace DSA_P1_KH.Tests;

public static class TestRunner
{
    public static void RunAll()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("DSA P1 Tests")
                .Centered()
                .Color(Color.Green)
        );

        ArrayListTests.Run();
        LinkedListTests.Run();
        BSTTests.Run();
        HashMapTests.Run();

        AnsiConsole.Write(new Rule("[green]All tests finished[/]").RuleStyle("grey").Centered());
        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}