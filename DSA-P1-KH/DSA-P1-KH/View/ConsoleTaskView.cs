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
            new Rule("[yellow]Kanban Board[/]").RuleStyle("grey").Centered()
        );

        var todo = tasks.Where(t => t.Status == TaskState.Todo).ToList();
        var progress = tasks.Where(t => t.Status == TaskState.InProgress).ToList();
        var done = tasks.Where(t => t.Status == TaskState.Done).ToList();

        int maxRows = new[] { todo.Count, progress.Count, done.Count }.Max();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[red]To Do[/]")
            .AddColumn("[yellow]In Progress[/]")
            .AddColumn("[green]Done[/]");

        for (int i = 0; i < maxRows; i++)
        {
            string col1 = i < todo.Count ? FormatTask(todo[i]) : "";
            string col2 = i < progress.Count ? FormatTask(progress[i]) : "";
            string col3 = i < done.Count ? FormatTask(done[i]) : "";

            table.AddRow(col1, col2, col3);
        }

        AnsiConsole.Write(table);
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

                case "Exit":
                    return;
            }
        }
    }
}