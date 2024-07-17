using domainNS = PTG.NextStep.Domain;

namespace PTG.NextStep.Service
{
    public class TaskQueueItem
    {
        public Guid TaskId { get; } = Guid.NewGuid();
        public Func<CancellationToken, Task> WorkItem { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public domainNS.TaskStatus Status { get; private set; }

        public TaskQueueItem(Func<CancellationToken, Task> workItem)
        {
            WorkItem = workItem;
            CancellationTokenSource = new CancellationTokenSource();
            Status = domainNS.TaskStatus.Pending;
        }

        public void SetStatus(domainNS.TaskStatus status)
        {
            Status = status;
        }
    }
}
