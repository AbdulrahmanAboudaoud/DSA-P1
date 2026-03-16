using DSA_P1_KH.Model;
using DSA_P1_KH.Repository;

namespace DSA_P1_KH.Service;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly List<TaskItem> _tasks;

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
        int newId = _tasks.Any()
            ? _tasks.Max(t => t.Id) + 1
            : 1;

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
        var task = _tasks.Find(t => t.Id == id);

        if (task != null)
        {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskStatus(int id, TaskState newStatus)
    {
        var task = _tasks.Find(t => t.Id == id);

        if (task != null)
        {
            task.Status = newStatus;
            _repository.SaveTasks(_tasks);
        }
    }
}