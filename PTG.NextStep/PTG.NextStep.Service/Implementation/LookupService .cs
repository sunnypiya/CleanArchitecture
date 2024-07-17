using AutoMapper;
using Microsoft.Extensions.Logging;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Service
{
    public class LookupService : ILookupService
    {
        private ILookupRepository _lookupRepository;
        private IMapper _mapper;
        private readonly ILogger<LookupService> _logger;
        public LookupService(ILookupRepository lookupRepository
            , IMapper mapper
            , ILogger<LookupService> logger)
        {
            _lookupRepository = lookupRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<PostingCodesDTO>> GetPostingCodesAsync()
        {
            try
            {
                return await _lookupRepository.GetPostingCodesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching Posting Codes : {ex.Message}", ex);
                throw;
            }
        }
    }
}
