namespace SOLID_and_Kiss_Principles.Interfaces;

/// <summary>
/// Task management interface - for assigning and creating tasks
/// ISP in action: separated from worker responsibilities
/// Notice how this is focused and specific
/// </summary>
public interface ITaskManager
{
    void AssignTask(string taskName, string assignee);
    void CreateSubTask(string parentTask, string subTask);
    void ReviewTask(string taskName);
}

/// <summary>
/// Worker interface - for actually doing the work
/// ISP: separate from management responsibilities
/// This way, managers don't need to implement work methods
/// </summary>
public interface IWorker
{
    void WorkOnTask(string taskName);
    void CompleteTask(string taskName);
    void ReportProgress(string taskName, int progressPercentage);
}

/// <summary>
/// Notification interface - for sending different types of notifications
/// Keep it simple - just what we need
/// </summary>
public interface INotificationService
{
    void SendTaskAssignment(string recipient, string taskName);
    void SendTaskCompletion(string recipient, string taskName);
}
