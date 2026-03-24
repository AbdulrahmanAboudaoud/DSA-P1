using DSA_P1_KH.Model;
using DSA_P1_KH.Service;
using Spectre.Console;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.View;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    private TaskFilterMode _filterMode = TaskFilterMode.All;
    private TaskSortMode _sortMode = TaskSortMode.None;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    void DisplayTasks(IEnumerable<TaskItem> tasks)
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new Rule("[yellow]Kanban Board[/]").RuleStyle("grey").Centered()
        );

        var collection = (IMyCollection<TaskItem>)tasks;

        // APPLY SORTING
        if (_sortMode != TaskSortMode.None)
        {
            collection.Sort(_sortMode switch
            {
                TaskSortMode.Id => (a, b) => a.Id.CompareTo(b.Id),
                TaskSortMode.CreationDate => (a, b) => a.CreationDate.CompareTo(b.CreationDate),
                TaskSortMode.Description => (a, b) =>
                    string.Compare(a.Description, b.Description, true),
                _ => (a, b) => 0
            });
        }

        // APPLY FILTERING
        IMyCollection<TaskItem> filtered = _filterMode switch
        {
            TaskFilterMode.Todo => collection.Filter(t => t.Status == TaskState.Todo),
            TaskFilterMode.InProgress => collection.Filter(t => t.Status == TaskState.InProgress),
            TaskFilterMode.Done => collection.Filter(t => t.Status == TaskState.Done),
            _ => collection
        };

        var todo = filtered.Filter(t => t.Status == TaskState.Todo);
        var progress = filtered.Filter(t => t.Status == TaskState.InProgress);
        var done = filtered.Filter(t => t.Status == TaskState.Done);

        int maxRows = Math.Max(todo.Count,
                        Math.Max(progress.Count, done.Count));

        var todoIt = todo.GetIterator();
        var progIt = progress.GetIterator();
        var doneIt = done.GetIterator();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[red]To Do[/]")
            .AddColumn("[yellow]In Progress[/]")
            .AddColumn("[green]Done[/]");

        for (int i = 0; i < maxRows; i++)
        {
            string col1 = todoIt.HasNext()
                ? FormatTask(todoIt.Next())
                : "";

            string col2 = progIt.HasNext()
                ? FormatTask(progIt.Next())
                : "";

            string col3 = doneIt.HasNext()
                ? FormatTask(doneIt.Next())
                : "";

            table.AddRow(col1, col2, col3);
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[grey]Filter:[/] [yellow]{_filterMode}[/]   " +
            $"[grey]Sort:[/] [cyan]{_sortMode}[/]");
    }

    string FormatTask(TaskItem task)
    {
        return $"[blue]{task.Id}[/] {task.Description}\n" +
               $"[grey]{task.CreationDate:dd/MM HH:mm}[/]";
    }

    public void Run()
    {
        while (true)
        {
            DisplayTasks(_service.GetAllTasks());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an option[/]")
                    .AddChoices(new[]
                    {
                        "Add Task",
                        "Remove Task",
                        "Change Task Status",
                        "Edit Task",
                        "Change Filter",
                        "Change Sorting",
                        "Exit"
                    }));

            switch (option)
            {
                case "Add Task":

                    var description = AnsiConsole.Ask<string>(
                        "Enter [green]task description[/]:");

                    var duplicate = _service.FindByDescription(description);

                    if (duplicate != null)
                    {
                        AnsiConsole.MarkupLine($"[yellow]Warning: A task with this description already exists (ID: {duplicate.Id})[/]");
                        var confirmAdd = AnsiConsole.Confirm("Do you want to add an identical task under a new ID?");
                        if (!confirmAdd)
                            break;
                    }

                    _service.AddTask(description);
                    break;

                case "Remove Task":

                    var removeId = AnsiConsole.Ask<int>(
                        "Enter task [red]id[/] to remove:");

                    if (!_service.RemoveTask(removeId))
                    {
                        AnsiConsole.MarkupLine("[red]Error: No such task - Press any key to continue[/]");
                        System.Console.ReadKey();
                    }
                    break;

                case "Change Task Status":

                    var id = AnsiConsole.Ask<int>(
                        "Enter task [blue]id[/]:");

                    var status = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskState>()
                            .Title("Select new [green]status[/]:")
                            .AddChoices(Enum.GetValues<TaskState>())
                    );

                    _service.ChangeTaskStatus(id, status);
                    break;

                case "Edit Task":

                    var descId = AnsiConsole.Ask<int>(
                        "Enter task [blue]id[/]:");

                    var desc = AnsiConsole.Ask<string>(
                        "Enter new [green]description[/]:");

                    _service.ChangeTaskDescription(descId, desc);
                    break;

                case "Change Filter":

                    _filterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskFilterMode>()
                            .Title("Select [green]filter[/]:")
                            .AddChoices(Enum.GetValues<TaskFilterMode>())
                    );

                    break;

                case "Change Sorting":

                    _sortMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskSortMode>()
                            .Title("Select [green]sorting[/]:")
                            .AddChoices(Enum.GetValues<TaskSortMode>())
                    );

                    break;

                case "Exit":
                    return;
            }
        }
    }
}