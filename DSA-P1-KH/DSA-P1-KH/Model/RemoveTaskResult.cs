namespace DSA_P1_KH.Model;

public enum RemoveTaskResult
{
    Success,
    TaskNotFound,
    PermissionDenied,
    HasDependencies
}