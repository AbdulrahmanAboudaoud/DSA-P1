using DSA_P1_KH.Model;
using DSA_P1_KH.Service;
using Spectre.Console;

namespace DSA_P1_KH.View;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    void DisplayTasks(IEnumerable<TaskItem> tasks)
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new Rule("[yellow]ToDo List[/]").RuleStyle("grey").Centered()
        );

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold blue]Tasks Overview[/]")
            .AddColumn(new TableColumn("[green]ID[/]").Centered())
            .AddColumn(new TableColumn("[white]Description[/]"))
            .AddColumn(new TableColumn("[cyan]Status[/]").Centered());

        foreach (var task in tasks)
        {
            var status = task.Completed
                ? "[green]Done[/]"
                : "[red]Pending[/]";

            table.AddRow(
                task.Id.ToString(),
                task.Description,
                status
            );
        }

        AnsiConsole.Write(table);
    }

    public void Run()
    {
        while (true)
        {
            DisplayTasks(_service.GetAllTasks());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an option[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Add Task",
                        "Remove Task",
                        "Toggle Task State",
                        "Exit"
                    }));

            switch (option)
            {
                case "Add Task":
                    var description = AnsiConsole.Ask<string>(
                        "Enter [green]task description[/]:");
                    _service.AddTask(description);
                    break;

                case "Remove Task":
                    var removeId = AnsiConsole.Ask<int>(
                        "Enter task [red]id[/] to remove:");
                    _service.RemoveTask(removeId);
                    break;

                case "Toggle Task State":
                    var toggleId = AnsiConsole.Ask<int>(
                        "Enter task [blue]id[/] to toggle:");
                    _service.ToggleTaskCompletion(toggleId);
                    break;

                case "Exit":
                    return;
            }
        }
    }
}