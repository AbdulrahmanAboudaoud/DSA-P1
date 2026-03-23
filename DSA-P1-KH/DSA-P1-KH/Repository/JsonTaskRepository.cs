using System.Text.Json;
using DSA_P1_KH.Model;
using DSA_P1_KH.DataStructures.ArrayList;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.Repository;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        var collection = new MyArrayList<TaskItem>();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

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