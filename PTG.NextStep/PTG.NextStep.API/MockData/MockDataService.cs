using PTG.NextStep.Domain;
using System.Text.Json;
using PTG.NextStep.Domain.Models;

namespace PTG.NextStep.API.MockData
{
    public class MockDataService
    {
        private readonly IWebHostEnvironment _env;

        public MockDataService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<List<MemberBasic>> GetMockDataAsync()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "MockData", "memberdata.json");
            List<MemberBasic> data = new List<MemberBasic>();
            if (File.Exists(filePath))
            {
                var json = await File.ReadAllTextAsync(filePath);
                data = JsonSerializer.Deserialize<List<MemberBasic>>(json);
            }
            return data;
            
        }
    }
}
