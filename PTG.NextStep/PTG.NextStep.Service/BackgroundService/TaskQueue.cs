using domainNS = PTG.NextStep.Domain;
using System.Collections.Concurrent;
using PTG.NextStep.Domain;

namespace PTG.NextStep.Service
{
    public class TaskQueue : ITaskQueue
    {
        private readonly ConcurrentQueue<TaskQueueItem> _workItems = new();
        private readonly ConcurrentDictionary<Guid, TaskQueueItem> _taskDictionary = new();
        private readonly SemaphoreSlim _signal = new(0);        

        public void Enqueue(TaskQueueItem taskQueueItem)
        {
            if (taskQueueItem == null)
            {
                throw new ArgumentNullException(nameof(taskQueueItem));
            }

            //var taskId = Guid.NewGuid();
            //await _taskRepository.AddTaskAsync(taskId, domainNS.TaskStatus.Pending);

            _workItems.Enqueue(taskQueueItem);
            _taskDictionary[taskQueueItem.TaskId] = taskQueueItem;
            _signal.Release();
            //return taskId;
        }

        public async Task<TaskQueueItem> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }

        public TaskQueueItem GetTaskById(Guid taskId)
        {
            _taskDictionary.TryGetValue(taskId, out var taskQueueItem);
            return taskQueueItem;
        }

        public IEnumerable<TaskQueueItem> GetAllTasks()
        {
            return _taskDictionary.Values.ToList();
        }
        public void CompleteTask(Guid taskId)
        {
            _taskDictionary.TryRemove(taskId, out _);
        }
    }

}
