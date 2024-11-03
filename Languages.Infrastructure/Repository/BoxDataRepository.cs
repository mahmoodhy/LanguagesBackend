using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository
{


    public class BoxDataRepository : GenericRepository<BoxData>, IBoxDataRepository
    {
        public BoxDataRepository(LeitnerBoxDbcontext context) : base(context)
        {
        }
        public async Task<BoxData?> FindWordAsync(string word)
        {
            var Findingword = await _context.Box.Where(x => x.EnglishWord == word).FirstOrDefaultAsync();
            return Findingword;
        }

    }
}
