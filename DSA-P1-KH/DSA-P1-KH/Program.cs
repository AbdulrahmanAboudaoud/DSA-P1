using DSA_P1_KH.Repository;
using DSA_P1_KH.Service;
using DSA_P1_KH.View;
using DSA_P1_KH.PhaseDemos;
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
                new FigletText("DSA Project")
                    .Centered()
                    .Color(Color.Cyan)
            );

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select what you want to run[/]")
                    .AddChoices(new[]
                    {
                        "Dynamic Array Demo",
                        "Run Task Manager",
                        "Exit"
                    })
            );

            switch (option)
            {
                case "Dynamic Array Demo":
                    DynamicArrayDemo.Run();
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
        string filePath = Path.Combine(
             AppContext.BaseDirectory,
             "..",
             "..",
             "..",
             "tasks.json"
         );

        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);
        ITaskView view = new ConsoleTaskView(service);

        view.Run();
    }
}