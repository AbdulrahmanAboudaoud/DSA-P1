using DSA_P1_KH.Repository;
using DSA_P1_KH.Service;
using DSA_P1_KH.View;

namespace DSA_P1_KH;

class Program
{
    public static void Main()
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
