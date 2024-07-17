using AutoMapper;
using Microsoft.Extensions.Logging;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Service
{
    public class TaskService : ITaskService
    {
        private readonly ILogger<DeductionService> _logger;
        private IDeductionRepository _deductionRepository;
        public TaskService(
            IDeductionRepository deductionRepository
            , ILogger<DeductionService> logger
            )
        {
            _logger = logger;
            _deductionRepository = deductionRepository;
        }
        public async Task ExecuteAsync(CreateRegisterRecordDTO createRegisterRecordDTO, CancellationToken cancellationToken)
        {
            try
            {
                var entities = await _deductionRepository.CreateRegisterRecord(createRegisterRecordDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while processing Register Record Task Message:{ex.Message}", ex);
            }
        }
    }
}
