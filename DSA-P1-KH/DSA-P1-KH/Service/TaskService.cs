using DSA_P1_KH.Repository;
using DSA_P1_KH.DataStructures.Interfaces;
using DSA_P1_KH.Model;

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

    public IEnumerable<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string description, TaskPriority priority)
    {
        int maxId = _tasks.Reduce(0, (max, t) => t.Id > max ? t.Id : max);
        int newId = maxId + 1;

        _tasks.Add(new TaskItem
        {
            Id = newId,
            Description = description,
            Priority = priority,
            Status = TaskState.Todo,
            CreationDate = DateTime.Now
        });

        _repository.SaveTasks(_tasks);
    }

    public bool RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        foreach (var t in _tasks)
        {
            if (t.Dependencies != null && Contains(t.Dependencies, id))
                return false;
        }

        _tasks.Remove(task);
        _repository.SaveTasks(_tasks);
        return true;
    }

    public bool ChangeTaskStatus(int id, TaskState newStatus)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task == null) return false;

        if (newStatus == TaskState.Done && task.Dependencies != null)
        {
            for (int i = 0; i < task.Dependencies.Length; i++)
            {
                var dep = _tasks.FindBy(task.Dependencies[i], (t, key) => t.Id == key);

                if (dep == null || dep.Status != TaskState.Done)
                    return false;
            }
        }

        task.Status = newStatus;
        _repository.SaveTasks(_tasks);
        return true;
    }

    public void ChangeTaskDescription(int id, string newDescription)
    {
        var task = GetTaskById(id);
        if (task == null) return;

        task.Description = newDescription;
        _repository.SaveTasks(_tasks);
    }

    public void ChangeTaskPriority(int id, TaskPriority newPriority)
    {
        var task = GetTaskById(id);
        if (task == null) return;

        task.Priority = newPriority;
        _repository.SaveTasks(_tasks);
    }

    public TaskItem GetTask(int id)
    {
        var task = GetTaskById(id);
        if (task == null)
            throw new KeyNotFoundException($"Task {id} not found");

        return task;
    }

    public TaskItem? GetTaskById(int id)
        => _tasks.FindBy(id, (t, key) => t.Id == key);

    public TaskItem? FindByDescription(string description)
        => _tasks.FindBy(description, (t, key) =>
            string.Equals(t.Description, key, StringComparison.Ordinal));

    // DEPENDENCIES //

    public bool AddDependency(int taskId, int dependencyId)
    {
        var task = GetTaskById(taskId);
        var dependency = GetTaskById(dependencyId);

        if (task == null || dependency == null)
            return false;

        if (taskId == dependencyId)
            return false;

        if (HasCircularDependency(taskId, dependencyId))
            return false;

        if (task.Dependencies.Length == 0)
        {
            task.Dependencies = new int[] { dependencyId };
        }
        else
        {
            if (Contains(task.Dependencies, dependencyId))
                return false;

            task.Dependencies = AddToArray(task.Dependencies, dependencyId);
        }

        _repository.SaveTasks(_tasks);
        return true;
    }

    public bool RemoveDependency(int taskId, int dependencyId)
    {
        var task = GetTaskById(taskId);
        if (task == null || task.Dependencies.Length == 0)
            return false;

        if (!Contains(task.Dependencies, dependencyId))
            return false;

        task.Dependencies = RemoveFromArray(task.Dependencies, dependencyId);

        _repository.SaveTasks(_tasks);
        return true;
    }

    // HELPERS //

    private bool Contains(int[] arr, int value)
    {
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] == value)
                return true;

        return false;
    }

    private int[] AddToArray(int[] arr, int value)
    {
        int[] newArr = new int[arr.Length + 1];

        for (int i = 0; i < arr.Length; i++)
            newArr[i] = arr[i];

        newArr[arr.Length] = value;

        return newArr;
    }

    private int[] RemoveFromArray(int[] arr, int value)
    {
        int count = 0;

        for (int i = 0; i < arr.Length; i++)
            if (arr[i] != value)
                count++;

        int[] newArr = new int[count];
        int index = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != value)
            {
                newArr[index++] = arr[i];
            }
        }

        return newArr;
    }

    private bool HasCircularDependency(int startId, int targetId)
    {
        if (startId == targetId)
            return true;

        var task = GetTaskById(targetId);
        if (task == null || task.Dependencies.Length == 0)
            return false;

        for (int i = 0; i < task.Dependencies.Length; i++)
        {
            if (HasCircularDependency(startId, task.Dependencies[i]))
                return true;
        }

        return false;
    }
}