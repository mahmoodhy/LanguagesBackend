using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IUserBoxRepository : IGenericRepository<UserBox>
    {
        Task<List<UserBox>> TodayIsStartedBeforeList(string username);
        Task<bool> IsTodayFinishedAsync(string username);
        Task<List<UserBox>> GetWordsFromExistBoxes(string username, int count);
        Task<List<UserBox>> GetWordsForTodayAsync(int count, string username);
        Task<UserBox> GetOneRandomWordforTodayAsync(string username);
        Task<List<int>> GetTodayWordsRemainingIdsAsync(string username);
        Task<bool> IsTodayFinished(string username);
        Task<UserBox?> GetWordByBoxIdAsync(int wordBoxId);
        Task<UserBox?> FindWordAsync(string word,string UserName);

    }
}
