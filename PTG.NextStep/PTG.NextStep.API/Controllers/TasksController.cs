using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTG.NextStep.Service;

namespace PTG.NextStep.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskQueue _deductionTaskQueue;

        public TasksController(ILogger<TasksController> logger,ITaskQueue deductionTaskQueue)
        {
            _logger = logger;
            _deductionTaskQueue = deductionTaskQueue;
        }
        [HttpGet("status/{taskId}")]
        public IActionResult CheckTaskStatus(Guid taskId)
        {
            try
            {
                var taskQueueItem = _deductionTaskQueue.GetTaskById(taskId);
                if (taskQueueItem != null)
                {
                    return Ok(new { TaskId = taskQueueItem.TaskId, Status = taskQueueItem.Status });
                }

                return NotFound(new { Message = "Task not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:CheckTaskStatus: {ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while checking task status."
                });
            }
        }
        [HttpPost("cancel/{taskId}")]
        public IActionResult CancelTask(Guid taskId)
        {
            try
            {
                var taskQueueItem = _deductionTaskQueue.GetTaskById(taskId);
                if (taskQueueItem != null && taskQueueItem.Status != Domain.TaskStatus.Completed)
                {
                    taskQueueItem.CancellationTokenSource.Cancel();
                    return Ok(new { Message = "Task cancelled successfully" });
                }

                return NotFound(new { Message = "Task not found/completed" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:CancelTask: {ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while cancelling task."
                });
            }
        }
        [HttpGet("status")]
        public IActionResult GetAllTaskStatuses()
        {
            try
            {
                var tasks = _deductionTaskQueue.GetAllTasks().Select(t => new { TaskId = t.TaskId, Status = t.Status });
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetAllTaskStatuses: {ex.Message}", ex);
                return BadRequest(new
                {
                    Error = "Error while getting task statuses."
                });
            }
        }
    }
}
