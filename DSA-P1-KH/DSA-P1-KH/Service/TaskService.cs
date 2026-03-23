using DSA_P1_KH.Model;
using DSA_P1_KH.Repository;
using DSA_P1_KH.DataStructures.Interfaces;

namespace DSA_P1_KH.Service;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }

    public IEnumerable<TaskItem> GetAllTasks()
    {
        return _tasks;
    }

    public void AddTask(string description)
    {
        int maxId = _tasks.Reduce(0, (max, t) => t.Id > max ? t.Id : max);
        int newId = maxId + 1;

        var newTask = new TaskItem
        {
            Id = newId,
            Description = description,
            Status = TaskState.Todo,
            CreationDate = DateTime.Now
        };

        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }

    public void RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);

        if (task != null)
        {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskStatus(int id, TaskState newStatus)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);

        if (task != null)
        {
            task.Status = newStatus;
            _repository.SaveTasks(_tasks);
        }
    }
}