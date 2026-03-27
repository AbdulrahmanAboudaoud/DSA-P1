using DSA_P1_KH.Model;

namespace DSA_P1_KH.Service;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();

    void AddTask(string description, TaskPriority priority);

    bool RemoveTask(int id);

    void ChangeTaskStatus(int id, TaskState newStatus);

    void ChangeTaskDescription(int id, string newDescription);

    void ChangeTaskPriority(int id, TaskPriority newPriority);

    TaskItem? FindByDescription(string description);
}