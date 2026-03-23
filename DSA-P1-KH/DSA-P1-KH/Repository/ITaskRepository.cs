using DSA_P1_KH.DataStructures.Interfaces;
using DSA_P1_KH.Model;

namespace DSA_P1_KH.Repository;

public interface ITaskRepository
{
    IMyCollection<TaskItem> LoadTasks();
    void SaveTasks(IMyCollection<TaskItem> tasks);
}