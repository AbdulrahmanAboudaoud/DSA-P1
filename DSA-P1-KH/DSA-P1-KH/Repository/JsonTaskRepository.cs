using System.Text.Json;
using DSA_P1_KH.Model;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.Repository;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly Func<IMyCollection<TaskItem>> _collectionFactory;

    public JsonTaskRepository(string filePath, Func<IMyCollection<TaskItem>> collectionFactory)
    {
        _filePath = filePath;
        _collectionFactory = collectionFactory;
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        IMyCollection<TaskItem> collection = _collectionFactory();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return collection;

        var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

        if (tasks != null)
        {
            foreach (var task in tasks)
                collection.Add(task);
        }

        return collection;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        var list = new List<TaskItem>();

        foreach (var task in tasks)
            list.Add(task);

        string json = JsonSerializer.Serialize(
            list,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(_filePath, json);
    }
}