using Microsoft.EntityFrameworkCore;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Database
{
    public class LookupRepository : ILookupRepository
    {
        private readonly ApplicationDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public LookupRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<PostingCodesDTO>> GetPostingCodesAsync()
        {
            return await _context.LuPostingCodes
                             .Select(c => new PostingCodesDTO { PostingCode = c.ShortName, LongName = c.LongName, PensionablePay = c.PensionablePay })
                             .ToListAsync();
        }
    }
}
