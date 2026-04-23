using DSA_P1_KH.Repository;
using DSA_P1_KH.Service;
using DSA_P1_KH.View;
using DSA_P1_KH.PhaseDemos;
using DSA_P1_KH.Model;
using DSA_P1_KH.DataStructures.Interfaces;
using DSA_P1_KH.DataStructures.ArrayList;
using DSA_P1_KH.DataStructures.LinkedList;
using DSA_P1_KH.DataStructures.BST;
using DSA_P1_KH.DataStructures.HashMap;
using DSA_P1_KH.Tests;
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
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Run Task Manager",
                        "Run Data Structure Tests",
                        "Dynamic Array Demo",
                        "Linked List Demo",
                        "HashMap Demo",
                        "Binary Search Tree Demo",
                        "Exit"
                    })
            );

            switch (option)
            {
                case "Run Task Manager":
                    RunTaskApp();
                    break;

                case "Run Data Structure Tests":
                    TestRunner.RunAll();
                    break;

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

                case "Exit":
                    return;
            }
        }
    }

    static void RunTaskApp()
    {
        var roleOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select your role[/]")
                .AddChoices("Project Manager", "Worker")
        );

        UserRole role = roleOption == "Project Manager"
            ? UserRole.ProjectManager
            : UserRole.Worker;

        string userName = AnsiConsole.Ask<string>("Enter your [green]name[/]:");

        var structureOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select data structure for Task Manager[/]")
                .AddChoices("ArrayList", "LinkedList", "BST", "HashMap")
        );

        DataStructureType structureType = structureOption switch
        {
            "ArrayList" => DataStructureType.ArrayList,
            "LinkedList" => DataStructureType.LinkedList,
            "BST" => DataStructureType.BST,
            "HashMap" => DataStructureType.HashMap,
            _ => DataStructureType.ArrayList
        };

        Func<IMyCollection<TaskItem>> collectionFactory = structureType switch
        {
            DataStructureType.ArrayList => () => new MyArrayList<TaskItem>(),
            DataStructureType.LinkedList => () => new MyLinkedList<TaskItem>(),
            DataStructureType.BST => () => new MyBST<TaskItem>(),
            DataStructureType.HashMap => () => new TaskHashMapCollection(),
            _ => () => new MyArrayList<TaskItem>()
        };

        string filePath = Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "tasks.json"
        );

        ITaskRepository repository = new JsonTaskRepository(filePath, collectionFactory);
        ITaskService service = new TaskService(repository);
        ITaskView view = new ConsoleTaskView(service, role, userName);

        AnsiConsole.MarkupLine(
            $"[green]Task Manager started using:[/] [yellow]{structureOption}[/]"
        );
        Thread.Sleep(1000);

        view.Run();
    }
}