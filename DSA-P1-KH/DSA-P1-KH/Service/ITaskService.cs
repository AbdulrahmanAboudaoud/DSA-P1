using DSA_P1_KH.Model;

namespace DSA_P1_KH.Service;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();
    void AddTask(string description);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);
}