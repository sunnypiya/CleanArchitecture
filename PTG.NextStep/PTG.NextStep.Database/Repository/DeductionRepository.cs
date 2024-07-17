using PTG.NextStep.Domain;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PTG.NextStep.Domain.Models;
using Microsoft.Extensions.Configuration;
using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Database
{
    public class DeductionRepository : IDeductionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public IUnitOfWork UnitOfWork => _context;

        public DeductionRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration;
        }

        public async Task<List<string>> GetPostingNumbersByDateAsync(DateOnly fromDate, bool excludeAllPosted = false)
        {
            return await _context.DERegister
                .Where(x=> (x.DeductionPostingDate == null || x.DeductionPostingDate >= fromDate) && (!excludeAllPosted || (excludeAllPosted && x.PostedFlag != CommonConstants.Yes) ))
                .Select(a =>a.PostingNumber)
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<EarningsDeductionsPostingNumberDTO>> GetEarningsDeductionsPostingNumbersAsync(DateOnly fromDate)
        {
            return await _context.MemberDedEarnsHistory
                .Where(x => (x.DeductionPostingDate == null || x.DeductionPostingDate >= fromDate) && x.Unit != CommonConstants.XFR)
                .Select(c => new EarningsDeductionsPostingNumberDTO { PostingNumber =c.PostingNumber, DeductionPostingDate=c.DeductionPostingDate })
                .Distinct()
                .OrderBy(a=>a.DeductionPostingDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<DERegister>> GetDERegisterByPostingNumberAsync(string postingNumber)
        {
            return await _context.DERegister.Where(a => a.PostingNumber == postingNumber).OrderBy(a => a.MemberName).ToListAsync();
        }

        public async Task DeleteDeductionRegisterRecordsAsync(string postingNumber)
        {
            try
            {
                _context.AuditTrail.RemoveRange(_context.AuditTrail
                    .Where(a => a.Screen == CommonConstants.DERegisterScreen
                             && _context.DERegister
                                .Where(d => d.PostingNumber == postingNumber)
                                .Select(d => d.AuditLink)
                                .Contains(a.CrossAuditLink.Value)));

                _context.DERegister.RemoveRange(_context.DERegister.Where(s => s.PostingNumber == postingNumber));

                _context.DEImportResults.RemoveRange(_context.DEImportResults.Where(d => d.PostingNumber == postingNumber));

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> CreateRegisterRecord(CreateRegisterRecordDTO createRegisterRecordDTO)
        {
            var activeMembers = await (from mb in _context.MemberBasic
                                 join ms in _context.MemberStatusHistory
                                 on mb.Link equals ms.Link
                                 where ms.StatusDate == _context.MemberStatusHistory
                                                         .Where(m => m.Link == mb.Link)
                                                         .Select(m => m.StatusDate)
                                                         .Max()
                                 select new { mb.Link,ms.StatusEvent }
                                ).ToListAsync();

            //Filter Active Members
            var activeMembersLinks = activeMembers.Where(a => a.StatusEvent == "REHIR" || a.StatusEvent == "HIRED"
                                                        || a.StatusEvent == "BUYBK" || a.StatusEvent == "MAKUP"
                                                        || a.StatusEvent == "ACTLOA" || a.StatusEvent == "MILLV"
                                                        || a.StatusEvent == "MILRT" || a.StatusEvent == "WCOMP"
                                                        || a.StatusEvent == "OFFWC"

            ).Select(a=>a.Link).ToList();
            
            var earningHistories = await (from memberDedEarnsHistory in _context.MemberDedEarnsHistory
                                              join memberBasic in _context.MemberBasic on memberDedEarnsHistory.Link equals memberBasic.Link
                                              where activeMembersLinks.Contains(memberDedEarnsHistory.Link)
                                                  && memberDedEarnsHistory.PostingNumber == createRegisterRecordDTO.PostingNumberToPullFrom
                                                  && memberDedEarnsHistory.DeductionPostingDate == createRegisterRecordDTO.DeductionPostingDate
                                              orderby memberDedEarnsHistory.PostingCode
                                              select new { memberDedEarnsHistory, memberBasic }
                                        ).ToListAsync();

            foreach (var e in earningHistories)
            {
                var registerRecord = _context.DERegister.Where
                    (a => a.PostingNumber == createRegisterRecordDTO.PostingNumber
                    && a.Unit == (createRegisterRecordDTO.AllActivities ? e.memberBasic.CurrentUnit : e.memberDedEarnsHistory.Unit)
                    && a.MemberLink == e.memberBasic.Link
                    && a.PostingCode == e.memberDedEarnsHistory.PostingCode
                    ).FirstOrDefault();

                if (registerRecord == null)
                {
                    DERegister dERegister = new DERegister();
                    dERegister.DeductionAmount = e.memberDedEarnsHistory.DeductionAmount;
                    dERegister.ContributionRate = createRegisterRecordDTO.AllActivities ? (!string.IsNullOrEmpty(e.memberBasic.ContributionRate) ? decimal.Parse(e.memberBasic.ContributionRate) : 0) : e.memberDedEarnsHistory.ContributionRate;
                    dERegister.DeductionIncrementalAmount = e.memberDedEarnsHistory.DeductionIncrementalAmount;
                    dERegister.PostingCode = e.memberDedEarnsHistory.PostingCode;
                    dERegister.Unit = createRegisterRecordDTO.AllActivities ? e.memberBasic.CurrentUnit : e.memberDedEarnsHistory.Unit;
                    dERegister.MemberLink = e.memberBasic.Link;
                    dERegister.PostingNumber = createRegisterRecordDTO.PostingNumber;
                    dERegister.MemberSSN = e.memberBasic.SSN;
                    dERegister.MemberName = e.memberBasic.FullName;
                    dERegister.MemberSSNLastFour = e.memberBasic.SSNLastFour;
                    dERegister.OriginalDeductionAmount = e.memberDedEarnsHistory.DeductionAmount;
                    dERegister.OrigDeductionIncrementalAmt = e.memberDedEarnsHistory.DeductionIncrementalAmount;
                    dERegister.DeductionPostingDate = e.memberDedEarnsHistory.DeductionPostingDate;
                    dERegister.Modified = CommonConstants.No;
                    dERegister.PostedFlag = CommonConstants.No;

                    _context.DERegister.Add(dERegister);                    
                }
            }
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> SaveDERegisterAsync(DERegister dERegister)
        {
            _context.DERegister.Add(dERegister);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> GetRegisterRecordByPostingNumberAsync(string postingNumber)
        {
            return await _context.DERegister.Where(a => a.PostingNumber == postingNumber).FirstOrDefaultAsync() !=null;
        }
    }
}
