using Core.Entities;
using Core.Interfaces.Repository;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repository
{
    public class AimeaningRepository : GenericRepository<Aimeaning>, IAimeaningRepository
    {
        public AimeaningRepository(LeitnerBoxDbcontext context) : base(context)
        {
        }
        public async Task<List<Aimeaning>> GetAIMeaningsByBoxIdAsync(int boxId)
        {
            var result=await _context.Aimeanings.AsNoTracking().Where(x=>x.BoxId==boxId).ToListAsync();
            return result;
        }
    }
}