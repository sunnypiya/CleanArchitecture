using PTG.NextStep.Domain;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PTG.NextStep.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using Nest;
using System.Linq;
using PTG.NextStep.Domain.DTO;

namespace PTG.NextStep.Database
{
	public class MemberRepository : IMemberRepository, IDisposable
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public IUnitOfWork UnitOfWork => _context;

		public MemberRepository(IConfiguration configuration, ApplicationDbContext context)
		{
			_configuration = configuration;
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}
		public async Task<IEnumerable<MemberBasic>> GetMembersAsync(Expression<Func<MemberBasic, bool>>? predicate = null)
		{
            if (predicate == null)
            {
                return await _context.MemberBasic.ToListAsync();
            }
            return await _context.MemberBasic.Where(predicate).ToListAsync();            
		}        
        public async Task<MemberBasic> GetMemberBasicByLinkAsync(decimal link)
		{
			return await _context.MemberBasic.Where(m => m.Link == link).FirstOrDefaultAsync();
		}

		public async Task<MemberBasic> GetMemberByEmployeeNumberAsync(string employeeNumber)
		{
			return await _context.MemberBasic.Where(m => m.EmployeeNumber == employeeNumber).FirstOrDefaultAsync();
		}

		public async Task<MemberContact> GetMemberContactByLinkAsync(decimal link)
		{
			return await _context.MemberContact.Where(m => m.Link == link).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<MemberStatusHistory>> GetMemberStatusHistoryByLinkAsync(decimal link)
		{
			return await _context.MemberStatusHistory.Where(m => m.Link == link).ToListAsync();
		}

		public async Task<IEnumerable<MemberServiceHistory>> GetMemberServiceHistoryByLinkAsync(decimal link)
		{
			return await _context.MemberServiceHistory.Where(m => m.Link == link).ToListAsync();
		}

		public async Task<IEnumerable<MemberDedEarnsHistory>> GetMemberDedEarnsHistoryByLinkAsync(decimal link)
		{
			return await _context.MemberDedEarnsHistory.Where(m => m.Link == link).ToListAsync();
		}

		public async Task SaveMemberBasicInfoAsync(MemberBasic memberBasic)
		{
			_context.Entry(memberBasic).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
		public async Task SaveMemberContactInfoAsync(MemberContact memberContact)
		{
			_context.Entry(memberContact).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task<bool> CreateMemberContact(MemberContact memberContact)
		{
			_context.MemberContact.Add(memberContact);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> AddMemberStatusHistory(MemberStatusHistory memberStatusHistory)
		{
			_context.MemberStatusHistory.Add(memberStatusHistory);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> AddMemberServiceHistory(MemberServiceHistory memberServiceHistory)
		{
			_context.MemberServiceHistory.Add(memberServiceHistory);
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<decimal> CheckDuplicateSSNLink(string SSN, string tableToCheck)
		{
			decimal existingSSNLink = 0;
			switch (tableToCheck)
			{
				case "MemberBeneficiary":
					var memberBeneficiary = await _context.MemberBeneficiary.Where(a => a.BeneSSN == SSN).FirstOrDefaultAsync();
					if (memberBeneficiary != null)
						existingSSNLink = memberBeneficiary.Link;
					break;
				case "MemberDRO":
					var memberDRO = await _context.MemberDRO.Where(a => a.DROSSN == SSN).FirstOrDefaultAsync();
					if (memberDRO != null)
						existingSSNLink = memberDRO.Link;
					break;

			}
			return existingSSNLink;
		}
		public async Task<string> GetDERegisterBySSN(string SSN)
		{
			string postingNumber = string.Empty;
			var deRegister = await _context.DERegister.Where(a => a.MemberSSN == SSN && a.DeductionPostingDate == null).FirstOrDefaultAsync();
			if (deRegister != null)
				postingNumber = deRegister.PostingNumber;
			return postingNumber;
		}

		public async Task<IEnumerable<MemberEarningsSummary>> GetEarningsSummaryByLinkAsync(decimal link)
		{
			return await _context.MemberEarningsSummary.Where(m => m.Link == link).ToListAsync();
		}

		public async Task<IEnumerable<MemberBeneficiary>> GetBeneficiaryInfoList(decimal link)
		{
			return await _context.MemberBeneficiary.Where(m => m.Link == link).OrderBy(x => x.BeneID).ToListAsync();
		}

		public async Task<MemberPowerofAttorney> GetPowerofAttorneyByLinkAsync(decimal link)
		{
			return await _context.MemberPowerofAtty.Where(m => m.Link == link).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<MemberDependent>> GetMemberDependentsInfoList(decimal link)
		{
			return await _context.MemberDependent.Where(m => m.Link == link).OrderBy(x => x.DepID).ToListAsync();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public async Task<MemberDRO> GetMemberDROInfoByLinkAsync(decimal link)
		{
			try
			{
				if (_context.MemberBasic.Where(m => m.DROFlag == "Y" && m.Link == link).Any())
				{
					return await _context.MemberDRO.Where(m => m.Link == link).FirstOrDefaultAsync();
				}
				return null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
