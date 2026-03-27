using DSA_P1_KH.Model;
using DSA_P1_KH.Service;
using Spectre.Console;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.View;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    private TaskFilterMode _statusFilterMode = TaskFilterMode.All;
    private TaskPriorityFilterMode _priorityFilterMode = TaskPriorityFilterMode.All;
    private TaskDateFilterMode _dateFilterMode = TaskDateFilterMode.All;

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
                    string.Compare(a.Description, b.Description, StringComparison.OrdinalIgnoreCase),
                _ => (a, b) => 0
            });
        }

        // APPLY FILTERING
        IMyCollection<TaskItem> filtered = collection;

        filtered = _statusFilterMode switch
        {
            TaskFilterMode.Todo => filtered.Filter(t => t.Status == TaskState.Todo),
            TaskFilterMode.InProgress => filtered.Filter(t => t.Status == TaskState.InProgress),
            TaskFilterMode.Done => filtered.Filter(t => t.Status == TaskState.Done),
            _ => filtered
        };

        filtered = _priorityFilterMode switch
        {
            TaskPriorityFilterMode.Low => filtered.Filter(t => t.Priority == TaskPriority.Low),
            TaskPriorityFilterMode.Medium => filtered.Filter(t => t.Priority == TaskPriority.Medium),
            TaskPriorityFilterMode.High => filtered.Filter(t => t.Priority == TaskPriority.High),
            _ => filtered
        };

        filtered = _dateFilterMode switch
        {
            TaskDateFilterMode.Today => filtered.Filter(t => t.CreationDate.Date == DateTime.Today),
            TaskDateFilterMode.ThisWeek => filtered.Filter(t =>
                t.CreationDate.Date >= DateTime.Today.AddDays(-7)),
            TaskDateFilterMode.Older => filtered.Filter(t =>
                t.CreationDate.Date < DateTime.Today.AddDays(-7)),
            _ => filtered
        };

        var todo = filtered.Filter(t => t.Status == TaskState.Todo);
        var progress = filtered.Filter(t => t.Status == TaskState.InProgress);
        var done = filtered.Filter(t => t.Status == TaskState.Done);

        int maxRows = Math.Max(todo.Count, Math.Max(progress.Count, done.Count));

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
            $"\n[grey]Status Filter:[/] [yellow]{_statusFilterMode}[/]   " +
            $"[grey]Priority Filter:[/] [yellow]{_priorityFilterMode}[/]   " +
            $"[grey]Date Filter:[/] [yellow]{_dateFilterMode}[/]   " +
            $"[grey]Sort:[/] [cyan]{_sortMode}[/]");
    }

    string FormatTask(TaskItem task)
    {
        string priorityColor = task.Priority switch
        {
            TaskPriority.High => "red",
            TaskPriority.Medium => "yellow",
            TaskPriority.Low => "grey",
            _ => "white"
        };

        return $"[blue]{task.Id}[/] {task.Description}\n" +
               $"[grey]{task.CreationDate:dd/MM HH:mm}[/]\n" +
               $"[{priorityColor}]{task.Priority}[/]";
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
                        "Change Task Priority",
                        "Change Task Description",
                        "Change Status Filter",
                        "Change Priority Filter",
                        "Change Date Filter",
                        "Change Sorting",
                        "Exit"
                    }));

            switch (option)
            {
                case "Add Task":

                    var description = AnsiConsole.Ask<string>(
                        "Enter [green]task description[/]:");

                    var priority = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriority>()
                            .Title("Select [green]priority[/]:")
                            .AddChoices(Enum.GetValues<TaskPriority>())
                    );

                    var duplicate = _service.FindByDescription(description);

                    if (duplicate != null)
                    {
                        AnsiConsole.MarkupLine(
                            $"[yellow]Warning: A task with this description already exists (ID: {duplicate.Id})[/]");

                        var confirmAdd = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Duplicate description detected. Add anyway?")
                                .AddChoices("Yes", "No")
                        ) == "Yes";

                        if (!confirmAdd)
                            break;
                    }

                    _service.AddTask(description, priority);
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

                case "Change Task Description":

                    var descId = AnsiConsole.Ask<int>(
                        "Enter task [blue]id[/]:");

                    var desc = AnsiConsole.Ask<string>(
                        "Enter new [green]description[/]:");

                    _service.ChangeTaskDescription(descId, desc);
                    break;

                case "Change Task Priority":

                    var prioId = AnsiConsole.Ask<int>(
                        "Enter task [blue]id[/]:");

                    var newPriority = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriority>()
                            .Title("Select new [green]priority[/]:")
                            .AddChoices(Enum.GetValues<TaskPriority>())
                    );

                    _service.ChangeTaskPriority(prioId, newPriority);
                    break;

                case "Change Status Filter":

                    _statusFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskFilterMode>()
                            .Title("Select [green]status filter[/]:")
                            .AddChoices(Enum.GetValues<TaskFilterMode>())
                    );

                    break;

                case "Change Priority Filter":

                    _priorityFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriorityFilterMode>()
                            .Title("Select [green]priority filter[/]:")
                            .AddChoices(Enum.GetValues<TaskPriorityFilterMode>())
                    );

                    break;

                case "Change Date Filter":

                    _dateFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskDateFilterMode>()
                            .Title("Select [green]date filter[/]:")
                            .AddChoices(Enum.GetValues<TaskDateFilterMode>())
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