using PTG.NextStep.Domain;
using PTG.NextStep.Service;
using domainNS = PTG.NextStep.Domain;


public class NSBackgroundService : BackgroundService
    {
        private readonly ILogger<NSBackgroundService> _logger;
        private readonly ITaskQueue _taskQueue;
        private readonly MiddlewarePipeline _middlewarePipeline;
        private readonly IHubService _hubService;

    public NSBackgroundService(ILogger<NSBackgroundService> logger
            , ITaskQueue taskQueue
            , MiddlewarePipeline middlewarePipeline
            ,IHubService hubService)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            _middlewarePipeline = middlewarePipeline;
            _hubService = hubService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                var taskQueueItem = await _taskQueue.DequeueAsync(stoppingToken);
                var taskCancellationToken = taskQueueItem.CancellationTokenSource.Token;

                taskQueueItem.SetStatus(domainNS.TaskStatus.InProgress);
                _ = Task.Run(async () =>
                {
                    try
                    {                        
                        await _middlewarePipeline.InvokeAsync(stoppingToken, taskQueueItem.WorkItem);

                        await _hubService.SendMessageAsync(taskQueueItem.TaskId.ToString(), $"Task Completed with TaskID:{taskQueueItem.TaskId.ToString()}"); // Send notification to client from signalR

                        taskQueueItem.SetStatus(domainNS.TaskStatus.Completed);
                    }
                    catch (OperationCanceledException)
                    {
                        taskQueueItem.SetStatus(domainNS.TaskStatus.Canceled);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(taskQueueItem.WorkItem));
                        taskQueueItem.SetStatus(domainNS.TaskStatus.Failed);
                    }
                    finally
                    {
                        //_taskQueue.CompleteTask(taskQueueItem.TaskId); // Remove task from queue after completion
                    }
                }, taskCancellationToken);

            }
        }
    }

