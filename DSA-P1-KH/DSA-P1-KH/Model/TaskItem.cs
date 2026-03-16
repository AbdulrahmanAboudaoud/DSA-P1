namespace DSA_P1_KH.Model;

public class TaskItem
{
    public int Id { get; set; }

    public string Description { get; set; } = "";

    public TaskState Status { get; set; } = TaskState.Todo;

    public DateTime CreationDate { get; set; } = DateTime.Now;
}