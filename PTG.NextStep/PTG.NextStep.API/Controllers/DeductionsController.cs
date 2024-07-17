using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Nest;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;
using PTG.NextStep.Service;

namespace PTG.NextStep.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeductionsController : ControllerBase
    {
        private readonly ILogger<DeductionsController> _logger;
        private readonly IDeductionService _deductionService;
        private readonly IValidator<CreateRegisterRecordDTO> _validatorCreateRegister;
        private readonly IValidationDictionary _validationDictionary;
        private readonly ITaskQueue _deductionTaskQueue;

        public DeductionsController(ILogger<DeductionsController> logger
            , IDeductionService deductionService
            , IValidator<CreateRegisterRecordDTO> validatorCreateRegister
            , IValidationDictionary validationDictionary
            , ITaskQueue deductionTaskQueue
            )
        {
            _logger = logger;
            _deductionService = deductionService;
            _validatorCreateRegister = validatorCreateRegister;
            _validationDictionary = validationDictionary;
            _deductionTaskQueue = deductionTaskQueue;
        }

        /// <summary>
        /// This will fetch the data of Posting Numbers of Last months based on the duration months parameter provided.
        /// </summary>
        /// <param name="durationMonths"></param>
        /// <param name="excludeAllPosted"></param>
        /// <returns></returns>
        [HttpGet("postingnumbers/{durationMonths}/{excludeAllPosted}")]
        public async Task<IActionResult> GetRecentPostingNumbers(int durationMonths,bool excludeAllPosted=false)
        {
            try
            {
                int[] allowedValues = new int[] { 0, 6, 12, 24 };
                if (!Array.Exists(allowedValues, v => v == durationMonths))
                {
                    return BadRequest(new
                    {
                        Errors = $"The value {durationMonths} is not allowed. Allowed values are: {string.Join(", ", allowedValues)}."                        
                    });
                }
                var postingNumbers = await _deductionService.GetPostingNumbersByDate(durationMonths, excludeAllPosted);
                return Ok(postingNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetRecentPostingNumbers: {ex.Message}", ex);
                return BadRequest(new
                {
                    Errors = "Error while fetching Recent Posting Numbers.",
                    
                });
            }
        }
        /// <summary>
        /// This will fetch the data of Posting Numbers of Last months based on the duration months parameter provided.
        /// </summary>
        /// <param name="durationMonths"></param>
        /// <returns></returns>
        [HttpGet("earnings-deduction-postingnumbers/{durationMonths}")]
        public async Task<IActionResult> GetEarningsDeductionPostingNumbers(int durationMonths)
        {
            try
            {
                int[] allowedValues = new int[] { 0, 6, 12, 24 };
                if (!Array.Exists(allowedValues, v => v == durationMonths))
                {
                    return BadRequest(new
                    {
                        Errors = $"The value {durationMonths} is not allowed. Allowed values are: {string.Join(", ", allowedValues)}."
                    });
                }
                var postingNumbers = await _deductionService.GetEarningsDeductionsPostingNumbers(durationMonths);
                return Ok(postingNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetEarningsDeductionPostingNumbers: {ex.Message}", ex);
                return BadRequest(new
                {
                    Errors = "Error while fetching Earnings Deductions Posting Numbers",

                });
            }
        }
        /// <summary>
        /// Get Manual Entries by Posting Number
        /// </summary>
        /// <param name="postingNumber"></param>
        /// <returns></returns>
        [HttpGet("manual-entries/{postingNumber}")]
        public async Task<IActionResult> GetManualDeductionEntryRecords(string postingNumber)
        {
            try
            {                
                var postingNumbers = await _deductionService.GetDERegisterByPostingNumberAsync(postingNumber);
                return Ok(postingNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:GetManualDeductionEntryRecords: {ex.Message}", ex);
                return BadRequest(new
                {
                    Errors = "Error while fetching Manual Deduction Entry Records."
                });
            }
        }

		[HttpDelete("postingnumber/{postingnumber}")]
		public async Task<IActionResult> DeleteDeductionRegisterRecordsAsync(string postingNumber )
		{
			try
			{				
				var isSuccess = await _deductionService.DeleteDeductionRegisterRecordsAsync(postingNumber,_validationDictionary);
                if (isSuccess)
                {
                    var response = new { Message = "Posting Number Record Deleted Successfully." };
                    return new JsonResult(response)
                    {
                        ContentType = "application/json"
                    };
                }
                else
                {
                    return BadRequest(new
                    {
                        Errors = _validationDictionary.Errors.Values.ToArray(),
                        Warnings = _validationDictionary.Warnings.Values.ToArray()
                    });
                }
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in method:DeleteDeductionRegisterRecordsAsync: {ex.Message}", ex);

                return BadRequest(new
                {
                    Errors = "Error while deleting deduction register record."
                });
            }
		}

        [HttpPost("create-register-records-task")]
        public async Task<IActionResult> CreateRegisterRecord([FromBody]CreateRegisterRecordDTO registerRecordDTO)
        {
            try
            {
                var result = await _validatorCreateRegister.ValidateAsync(registerRecordDTO);
                if (!result.IsValid)
                {
                    return BadRequest(new
                    {
                        Errors = result.Errors.Where(e => e.Severity == Severity.Error)
                                .Select(e => e.ErrorMessage)
                                .ToArray(),
                        Warnings = result.Errors.Where(e => e.Severity == Severity.Warning)
                                .Select(e => e.ErrorMessage)
                                .ToArray()
                    });
                }
                bool isRegisterRecordExist = await _deductionService.GetRegisterRecordByPostingNumber(registerRecordDTO.PostingNumber);
                if (isRegisterRecordExist)
                {
                    return BadRequest(new
                    {
                        Errors = "Register record exists for the given posting number"
                    });
                }

                var taskQueueItem = new  TaskQueueItem(async token =>
                {
                    using (var scope = HttpContext.RequestServices.CreateScope())
                    {
                        var deductionTaskService = scope.ServiceProvider.GetRequiredService<ITaskService>();                        
                        await deductionTaskService.ExecuteAsync(registerRecordDTO, token);
                    }
                });
                _deductionTaskQueue.Enqueue(taskQueueItem);
                return Ok(new { message = "Create Register Records Task started. Please check the status of a task with taskId.",TaskId= taskQueueItem.TaskId });
                                            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method:CreateRegisterRecord: {ex.Message}", ex);
                return BadRequest(new
                {
                    Errors = "Error while Creating Register Records task."
                });
            }
        }        
    }
}
