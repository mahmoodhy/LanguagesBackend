using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IUserBoxViewRepository : IGenericRepository<UserBoxView>
    {
        
        Task<UserBoxView?> GetUserBoxViewAsync(string word,string UserName);
        Task<UserBoxView?> GetWordByBoxIdAsync(int wordBoxId);

    }
}
