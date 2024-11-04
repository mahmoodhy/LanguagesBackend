using Core.Entities;
using Core.Interfaces.Repository;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository
{
    public class UserBoxStatisticRepository : GenericRepository<UserBoxStatistic>, IUserBoxStatisticRepository
    {
        public UserBoxStatisticRepository(LeitnerBoxDbcontext context) : base(context)
        {
        }
        public async Task<List<UserBoxStatistic>> GetWordLearnedStatisticsAsync(string username)
        {
            var statistic=await _context.UserBoxStatistics.Where(x=>x.UserName==username).ToListAsync();
            return statistic;
        }
    }
}