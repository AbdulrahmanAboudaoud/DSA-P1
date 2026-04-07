using DSA_P1_KH.Repository;
using DSA_P1_KH.Service;
using DSA_P1_KH.View;
using DSA_P1_KH.PhaseDemos;
using DSA_P1_KH.Model;
using Spectre.Console;

namespace DSA_P1_KH;

class Program
{
    static void Main()
    {
        while (true)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(
                new FigletText("DSA Project 1")
                    .Centered()
                    .Color(Color.Cyan)
            );

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select what you want to run[/]")
                    .AddChoices(new[]
                    {
                        "Dynamic Array Demo",
                        "Linked List Demo",
                        "HashMap Demo",
                        "Binary Search Tree Demo",
                        "Run Task Manager",
                        "Exit"
                    })
            );

            switch (option)
            {
                case "Dynamic Array Demo":
                    DynamicArrayDemo.Run();
                    break;

                case "Linked List Demo":
                    LinkedListDemo.Run();
                    break;

                case "HashMap Demo":
                    HashMapDemo.Run();
                    break;

                case "Binary Search Tree Demo":
                    BSTDemo.Run();
                    break;

                case "Run Task Manager":
                    RunTaskApp();
                    break;

                case "Exit":
                    return;
            }
        }
    }

    static void RunTaskApp()
    {
        // role selection
        var roleOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select your role[/]")
                .AddChoices("Project Manager", "Worker")
        );

        UserRole role = roleOption == "Project Manager"
            ? UserRole.ProjectManager
            : UserRole.Worker;

        // user name
        string userName = AnsiConsole.Ask<string>("Enter your [green]name[/]:");

        string filePath = Path.Combine(
             AppContext.BaseDirectory,
             "..",
             "..",
             "..",
             "tasks.json"
         );

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);

        // pass role + user to view
        ITaskView view = new ConsoleTaskView(service, role, userName);

        view.Run();
    }
}