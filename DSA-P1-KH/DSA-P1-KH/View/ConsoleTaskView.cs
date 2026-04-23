using System;
using DSA_P1_KH.Model;
using DSA_P1_KH.Service;
using Spectre.Console;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.View;

public class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    private readonly UserRole _role;
    private readonly string _user;

    private TaskFilterMode _statusFilterMode = TaskFilterMode.All;
    private TaskPriorityFilterMode _priorityFilterMode = TaskPriorityFilterMode.All;
    private TaskDateFilterMode _dateFilterMode = TaskDateFilterMode.All;

    private TaskSortMode _sortMode = TaskSortMode.None;

    public ConsoleTaskView(ITaskService service, UserRole role, string user)
    {
        _service = service;
        _role = role;
        _user = user;
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
            filtered = filtered.Filter(t => t.CreationDate.Date == DateTime.Today);
        else if (_dateFilterMode == TaskDateFilterMode.ThisWeek)
            filtered = filtered.Filter(t => t.CreationDate.Date >= DateTime.Today.AddDays(-7));
        else if (_dateFilterMode == TaskDateFilterMode.Older)
            filtered = filtered.Filter(t => t.CreationDate.Date < DateTime.Today.AddDays(-7));

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
            $"\n[grey]User:[/] [cyan]{_user}[/] ([yellow]{_role}[/])   " +
            $"[grey]Status:[/] [yellow]{_statusFilterMode}[/]   " +
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

        string assigned = string.IsNullOrEmpty(task.AssignedTo)
            ? ""
            : $"\n[dim]@{task.AssignedTo}[/]";

        return
            $"[grey]#{task.Id} [/]{deps}\n" +
            $"[bold]{task.Description}[/]{assigned}\n" +
            $"[{priorityColor}]{task.Priority}[/]\n" +
            $"[dim]{task.CreationDate:dd/MM HH:mm}[/]\n" +
            "[white]────────────[/]";
    }

    bool CanModify(TaskItem? task)
    {
        return task != null &&
               (_role == UserRole.ProjectManager || task.AssignedTo == _user);
    }

    public void Run()
    {
        while (true)
        {
            DisplayTasks(_service.GetAllTasks());

            var choices = new List<string>
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
                "Change Sorting"
            };

            if (_role == UserRole.ProjectManager)
                choices.Add("Assign Task");

            choices.Add("Exit");

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select an option[/]")
                    .AddChoices(choices)
            );

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

                    var result = _service.RemoveTask(removeId, _user, _role);

                    switch (result)
                    {
                        case RemoveTaskResult.TaskNotFound:
                            AnsiConsole.MarkupLine("[red]Task not found.[/]");
                            Console.ReadKey();
                            break;

                        case RemoveTaskResult.PermissionDenied:
                            AnsiConsole.MarkupLine("[red]Not allowed: only assigned user or manager can delete this task.[/]");
                            Console.ReadKey();
                            break;

                        case RemoveTaskResult.HasDependencies:
                            AnsiConsole.MarkupLine("[red]Cannot remove task: other tasks depend on it.[/]");
                            Console.ReadKey();
                            break;
                    }

                    break;

                case "Change Task Status":
                    var id = AnsiConsole.Ask<int>("Enter task id:");
                    var taskStatus = _service.GetTaskById(id);

                    if (!CanModify(taskStatus))
                    {
                        AnsiConsole.MarkupLine("[red]Not allowed: only assigned user or manager[/]");
                        Console.ReadKey();
                        break;
                    }

                    var status = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskState>()
                            .Title("Select new status:")
                            .AddChoices(Enum.GetValues<TaskState>())
                    );

                    if (!_service.ChangeTaskStatus(id, status, _user, _role))
                    {
                        AnsiConsole.MarkupLine("[red]Dependencies not completed[/]");
                        Console.ReadKey();
                    }
                    break;

                case "Change Task Description":
                    var descId = AnsiConsole.Ask<int>("Enter task id:");
                    var taskDesc = _service.GetTaskById(descId);

                    if (!CanModify(taskDesc))
                    {
                        AnsiConsole.MarkupLine("[red]Not allowed: only assigned user or manager[/]");
                        Console.ReadKey();
                        break;
                    }

                    var desc = AnsiConsole.Ask<string>("Enter new description:");
                    _service.ChangeTaskDescription(descId, desc, _user, _role);
                    break;

                case "Change Task Priority":
                    var prioId = AnsiConsole.Ask<int>("Enter task id:");
                    var taskPrio = _service.GetTaskById(prioId);

                    if (!CanModify(taskPrio))
                    {
                        AnsiConsole.MarkupLine("[red]Not allowed: only assigned user or manager[/]");
                        Console.ReadKey();
                        break;
                    }

                    var newPriority = AnsiConsole.Prompt(
                        new SelectionPrompt<TaskPriority>()
                            .Title("Select new priority:")
                            .AddChoices(Enum.GetValues<TaskPriority>())
                    );

                    _service.ChangeTaskPriority(prioId, newPriority, _user, _role);
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

                case "Assign Task":
                    var assignId = AnsiConsole.Ask<int>("Task id:");
                    var assignUser = AnsiConsole.Ask<string>("Assign to:");

                    var assignResult = _service.AssignTask(assignId, assignUser, _role);

                    switch (assignResult)
                    {
                        case AssignTaskResult.TaskNotFound:
                            AnsiConsole.MarkupLine($"[red]Task with ID {assignId} does not exist.[/]");
                            Console.ReadKey();
                            break;

                        case AssignTaskResult.PermissionDenied:
                            AnsiConsole.MarkupLine("[red]Only manager can assign tasks.[/]");
                            Console.ReadKey();
                            break;
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