using DSA_P1_KH.Model;

namespace DSA_P1_KH.Service;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();

    void AddTask(string description, TaskPriority priority);

    bool RemoveTask(int id);

    TaskItem GetTask(int id);

    TaskItem? GetTaskById(int id);

    bool ChangeTaskStatus(int id, TaskState newStatus, string user, UserRole role);

    void ChangeTaskDescription(int id, string newDescription, string user, UserRole role);

    void ChangeTaskPriority(int id, TaskPriority newPriority, string user, UserRole role);

    TaskItem? FindByDescription(string description);

    bool AddDependency(int taskId, int dependencyId);

    bool RemoveDependency(int taskId, int dependencyId);

    bool AssignTask(int taskId, string userName, UserRole role);
}