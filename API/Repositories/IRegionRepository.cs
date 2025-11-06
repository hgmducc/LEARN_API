
using API.Models.Domain;

namespace API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsyns();
    }
}
