using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Repository
{
    public class UserBoxViewRepository : GenericRepository<UserBoxView>, IUserBoxViewRepository
    {

        public UserBoxViewRepository(LeitnerBoxDbcontext context) : base(context)
        {

        }
       
        public async Task<UserBoxView?> GetUserBoxViewAsync(string word, string UserName)
        {
           
            var Findingword = await _context.userBoxView.Where(x => x.EnglishWord == word && x.UserName == UserName).FirstOrDefaultAsync();
            return Findingword;
        }
        public async Task<UserBoxView?> GetWordByBoxIdAsync(int wordBoxId)
        {
            var word = await _context.userBoxView.Where(x => x.BoxId == wordBoxId).FirstOrDefaultAsync();
            return word;
        }
    }
}
