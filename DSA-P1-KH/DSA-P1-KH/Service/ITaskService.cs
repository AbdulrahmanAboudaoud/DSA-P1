using DSA_P1_KH.Model;

namespace DSA_P1_KH.Service;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();

    void AddTask(string description, TaskPriority priority);

    bool RemoveTask(int id);

    TaskItem GetTask(int id);

    TaskItem? GetTaskById(int id);

    bool ChangeTaskStatus(int id, TaskState newStatus);

    void ChangeTaskDescription(int id, string newDescription);

    void ChangeTaskPriority(int id, TaskPriority newPriority);

    TaskItem? FindByDescription(string description);

    bool AddDependency(int taskId, int dependencyId);

    bool RemoveDependency(int taskId, int dependencyId);
}