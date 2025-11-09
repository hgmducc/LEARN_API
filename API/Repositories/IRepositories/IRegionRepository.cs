using API.Models.Domain;

namespace API.Repositories.IRepositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsyns();

        Task<Region> GetRegionbyId(Guid id);

        Task<Region?> creatRegion(Region region);

        Task<Region?> UpdateRegionbyId(Guid id, Region updateRegion);

        Task<Region?> Delete(Guid id);
    }
}
