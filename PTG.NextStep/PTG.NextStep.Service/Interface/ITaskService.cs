using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Service
{
    public interface ITaskService
    {
        Task ExecuteAsync(CreateRegisterRecordDTO createRegisterRecordDTO, CancellationToken cancellationToken);
    }
}
