using Core.Entities;
namespace Core.Interfaces.Repository
{
    public interface IUserBoxStatisticRepository : IGenericRepository<UserBoxStatistic>
    {
        Task<List<UserBoxStatistic>> GetWordLearnedStatisticsAsync(string username);
    }
}