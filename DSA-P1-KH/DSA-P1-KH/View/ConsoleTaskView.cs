using System;
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
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Clear();


        AnsiConsole.Write(
            new Rule("[yellow]Kanban Board[/]").RuleStyle("grey").Centered()
        );

        var collection = (IMyCollection<TaskItem>)tasks;

        // SORTING //

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

        // FILTERING //

        IMyCollection<TaskItem> filtered = collection;

        if (_statusFilterMode == TaskFilterMode.Todo)
            filtered = filtered.Filter(t => t.Status == TaskState.Todo);
        else if (_statusFilterMode == TaskFilterMode.InProgress)
            filtered = filtered.Filter(t => t.Status == TaskState.InProgress);
        else if (_statusFilterMode == TaskFilterMode.Done)
            filtered = filtered.Filter(t => t.Status == TaskState.Done);

        if (_priorityFilterMode == TaskPriorityFilterMode.Low)
            filtered = filtered.Filter(t => t.Priority == TaskPriority.Low);
        else if (_priorityFilterMode == TaskPriorityFilterMode.Medium)
            filtered = filtered.Filter(t => t.Priority == TaskPriority.Medium);
        else if (_priorityFilterMode == TaskPriorityFilterMode.High)
            filtered = filtered.Filter(t => t.Priority == TaskPriority.High);

        if (_dateFilterMode == TaskDateFilterMode.Today)
        {
            filtered = filtered.Filter(t => t.CreationDate.Date == DateTime.Today);
        }
        else if (_dateFilterMode == TaskDateFilterMode.ThisWeek)
        {
            filtered = filtered.Filter(t =>
                t.CreationDate.Date >= DateTime.Today.AddDays(-7));
        }
        else if (_dateFilterMode == TaskDateFilterMode.Older)
        {
            filtered = filtered.Filter(t =>
                t.CreationDate.Date < DateTime.Today.AddDays(-7));
        }

        // SPLIT INTO COLUMNS //

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
            string col1 = todoIt.HasNext() ? FormatTask(todoIt.Next()) : "";
            string col2 = progIt.HasNext() ? FormatTask(progIt.Next()) : "";
            string col3 = doneIt.HasNext() ? FormatTask(doneIt.Next()) : "";

            table.AddRow(col1 + "\n", col2 + "\n", col3 + "\n");
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine(
            $"\n[grey]Status:[/] [yellow]{_statusFilterMode}[/]   " +
            $"[grey]Priority:[/] [yellow]{_priorityFilterMode}[/]   " +
            $"[grey]Date:[/] [yellow]{_dateFilterMode}[/]   " +
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

        string statusColor = task.Status switch
        {
            TaskState.Todo => "red",
            TaskState.InProgress => "yellow",
            TaskState.Done => "green",
            _ => "white"
        };

        string deps = "";

        if (task.Dependencies != null && task.Dependencies.Length > 0)
        {
            deps = "Deps: ";
            for (int i = 0; i < task.Dependencies.Length; i++)
            {
                deps += task.Dependencies[i];
                if (i < task.Dependencies.Length - 1)
                    deps += ",";
            }
        }

        return
            $"[grey]#{task.Id} [/]{deps}\n" +                 // ID + deps
            $"[bold]{task.Description}[/]\n" +               // description
            $"[{priorityColor}]{task.Priority}[/]\n" +       // priority (only color)
            $"[dim]{task.CreationDate:dd/MM HH:mm}[/]\n" +   // date
            "[dim]────────────[/]";
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
                        "Add Dependency",
                        "Remove Dependency",
                        "Change Status Filter",
                        "Change Priority Filter",
                        "Change Date Filter",
                        "Change Sorting",
                        "Exit"
                    }));

            switch (option)
            {
                case "Add Task":
                    var description = AnsiConsole.Ask<string>("Enter task description:");

                    var priority = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriority>()
                            .Title("Select priority:")
                            .AddChoices(Enum.GetValues<TaskPriority>())
                    );

                    _service.AddTask(description, priority);
                    break;

                case "Remove Task":
                    var removeId = AnsiConsole.Ask<int>("Enter task id:");
                    if (!_service.RemoveTask(removeId))
                    {
                        AnsiConsole.MarkupLine("[red]Cannot remove task[/]");
                        Console.ReadKey();
                    }
                    break;

                case "Change Task Status":
                    var id = AnsiConsole.Ask<int>("Enter task id:");

                    var status = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskState>()
                            .Title("Select new status:")
                            .AddChoices(Enum.GetValues<TaskState>())
                    );

                    if (!_service.ChangeTaskStatus(id, status))
                    {
                        AnsiConsole.MarkupLine("[red]Cannot change status (dependencies not completed)[/]");
                        Console.ReadKey();
                    }
                    break;

                case "Change Task Description":
                    var descId = AnsiConsole.Ask<int>("Enter task id:");
                    var desc = AnsiConsole.Ask<string>("Enter new description:");
                    _service.ChangeTaskDescription(descId, desc);
                    break;

                case "Change Task Priority":
                    var prioId = AnsiConsole.Ask<int>("Enter task id:");

                    var newPriority = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriority>()
                            .Title("Select new priority:")
                            .AddChoices(Enum.GetValues<TaskPriority>())
                    );

                    _service.ChangeTaskPriority(prioId, newPriority);
                    break;

                case "Add Dependency":
                    var targetId = AnsiConsole.Ask<int>("Task id:");
                    var depId = AnsiConsole.Ask<int>("Dependency id:");

                    if (!_service.AddDependency(targetId, depId))
                    {
                        AnsiConsole.MarkupLine("[red]Cannot add dependency[/]");
                        Console.ReadKey();
                    }
                    break;

                case "Remove Dependency":
                    var tId = AnsiConsole.Ask<int>("Task id:");
                    var dId = AnsiConsole.Ask<int>("Dependency id:");

                    if (!_service.RemoveDependency(tId, dId))
                    {
                        AnsiConsole.MarkupLine("[red]Cannot remove dependency[/]");
                        Console.ReadKey();
                    }
                    break;

                case "Change Status Filter":
                    _statusFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskFilterMode>()
                            .Title("Select status filter:")
                            .AddChoices(Enum.GetValues<TaskFilterMode>())
                    );
                    break;

                case "Change Priority Filter":
                    _priorityFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriorityFilterMode>()
                            .Title("Select priority filter:")
                            .AddChoices(Enum.GetValues<TaskPriorityFilterMode>())
                    );
                    break;

                case "Change Date Filter":
                    _dateFilterMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskDateFilterMode>()
                            .Title("Select date filter:")
                            .AddChoices(Enum.GetValues<TaskDateFilterMode>())
                    );
                    break;

                case "Change Sorting":
                    _sortMode = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskSortMode>()
                            .Title("Select sorting:")
                            .AddChoices(Enum.GetValues<TaskSortMode>())
                    );
                    break;

                case "Exit":
                    return;
            }
        }
    }
}