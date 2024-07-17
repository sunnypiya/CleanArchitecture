using domainNS = PTG.NextStep.Domain;

namespace PTG.NextStep.Service
{
    public interface ITaskQueue
    {
        void Enqueue(TaskQueueItem taskQueueItem);
        Task<TaskQueueItem> DequeueAsync(CancellationToken cancellationToken);
        TaskQueueItem GetTaskById(Guid taskId);
        IEnumerable<TaskQueueItem> GetAllTasks();
        void CompleteTask(Guid taskId);
    }
}
