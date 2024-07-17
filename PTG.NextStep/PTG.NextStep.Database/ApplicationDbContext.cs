using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PTG.NextStep.Tenant;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.Database
{
    public class ApplicationDbContext: DbContext, IUnitOfWork
    {
        private IConfiguration _configuration;
        private IMultiTenancyService _tenantService;

        public ApplicationDbContext(DbContextOptions options, IConfiguration configuration, IMultiTenancyService tenantService) :base(options)
        {
            _configuration = configuration;
            _tenantService = tenantService;
        }

        public DbSet<MemberBasic> MemberBasic { get; set; }
        public DbSet<MemberContact> MemberContact { get; set; }
        public DbSet<MemberServiceHistory> MemberServiceHistory { get; set; }
        public DbSet<MemberStatusHistory> MemberStatusHistory { get; set; }
        public DbSet<MemberDedEarnsHistory> MemberDedEarnsHistory { get; set; }
        public DbSet<MemberBeneficiary> MemberBeneficiary { get; set; }
        
        public DbSet<MemberEarningsSummary> MemberEarningsSummary { get; set; }
        public DbSet<MemberDRO> MemberDRO { get; set; }
        public DbSet<DERegister> DERegister { get; set; }
        public DbSet<MemberPowerofAttorney> MemberPowerofAtty { get; set; }
        public DbSet<MemberDependent> MemberDependent { get; set; }
        public DbSet<LuPostingCodes> LuPostingCodes { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
		public DbSet<DEImportResults> DEImportResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string tenant = _tenantService.Tenant;            
            string connectionString = _configuration.GetConnectionString(tenant);
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberStatusHistory>()
                .HasKey(op => new { op.Link, op.RetirementBoard, op.StatusEvent });
            modelBuilder.Entity<MemberDedEarnsHistory>()
                .HasKey(op => new { op.Link, op.RetirementBoard, op.PostingNumber, op.Unit });
            modelBuilder.Entity<MemberServiceHistory>()
                .HasKey(op => new { op.Link, op.RetirementBoard});
            modelBuilder.Entity<MemberEarningsSummary>()
               .HasKey(op => new { op.Link, op.Year });
            modelBuilder.Entity<MemberBeneficiary>()
               .HasKey(op => new { op.Link, op.BeneID });
            modelBuilder.Entity<DERegister>()
               .HasKey(op => new { op.PostingNumber,op.Unit, op.MemberLink,op.PostingCode });
            modelBuilder.Entity<MemberDependent>()
              .HasKey(op => new { op.Link, op.DepID});
            modelBuilder.Entity<LuPostingCodes>()
              .HasKey(op => new { op.ShortName, op.Unit });
              modelBuilder.Entity<DEImportResults>()
				.HasKey(op => new { op.PostingNumber, op.ResultsID });

		}

	}
}
