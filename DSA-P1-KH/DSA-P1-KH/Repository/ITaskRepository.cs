using DSA_P1_KH.Model;

namespace DSA_P1_KH.Repository;

public interface ITaskRepository
{
    List<TaskItem> LoadTasks();
    void SaveTasks(List<TaskItem> tasks);
}