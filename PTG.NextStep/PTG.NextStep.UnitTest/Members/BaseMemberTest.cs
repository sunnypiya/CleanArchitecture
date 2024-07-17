using Microsoft.EntityFrameworkCore;
using PTG.NextStep.API.MockData;
using PTG.NextStep.Database;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.UnitTest.Members
{
    public class BaseMemberTest
    {
        [Fact]
        public async Task GetMembers_ReturnsMembers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            // Seed the database with test data
            using (var context = new ApplicationDbContext(options, null, null))
            {
                context.MemberBasic.Add(new MemberBasic { Link = 4440, EmployeeNumber = "4440" }); // Need to revisit this
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options, null, null))
            {
                // Create an instance of ProductRepository with the DbContext
                //var repository = new MemberRepository(context);

                //// Act
                //var members = await repository.GetMembersAsync();

                //// Assert
                //Assert.NotNull(members);
                //Assert.Equal("4440", members.FirstOrDefault().EmployeeNumber);
                
            }
        }
        
    }
}
