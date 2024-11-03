using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataAccess;
using Core.Interfaces.Repository;
using Core.Entities;


namespace Infrastructure.Repository
{

    public class ApiDictionaryRepository : GenericRepository<dictionaryRoot>, IApiDictionaryRepository
    {
        public ApiDictionaryRepository(LeitnerBoxDbcontext context) : base(context)
        {
        }
        public  dictionaryRoot GetAllByQuestion(string question)
        {
            try
            {
                return _context.ApiDictionaryRoot.Where(x => x.word.Equals(question)).Include(a => a.phonetics).Include(b => b.meanings).ThenInclude(c => c.definitions)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<dictionaryRoot?> GetAllByQuestionId(int questionId)
        {
            try
            {
                return await _context.ApiDictionaryRoot.Where(x => x.BoxId==questionId).Include(a => a.phonetics).Include(b => b.meanings).ThenInclude(c => c.definitions)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
