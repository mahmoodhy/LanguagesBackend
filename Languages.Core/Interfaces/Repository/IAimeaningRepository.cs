using Core.Entities;
namespace Core.Interfaces.Repository
{
    public interface IAimeaningRepository : IGenericRepository<Aimeaning>
    {
        Task<List<Aimeaning>> GetAIMeaningsByBoxIdAsync(int boxId);
    }
}