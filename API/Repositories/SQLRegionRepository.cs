using API.Data;
using API.Models.Domain;
using API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        // Inject dbcontext

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region?> creatRegion(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> Delete(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                throw new InvalidOperationException($"Region with Id '{id}' not found.");
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsyns()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionbyId(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                throw new InvalidOperationException($"Region with Id '{id}' not found.");
            }
            return region;
        }
        
        public async Task<Region?> UpdateRegionbyId(Guid id, Region updateRegion)
        {
            //tìm region cần update
            var regionUD = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // kiểm tra xem rgion có tồn tại không 
            if(regionUD == null)
            {
                return null;
            }

            regionUD.Code = updateRegion.Code;
            regionUD.Name = updateRegion.Name;
            regionUD.RegionImageUrl = updateRegion.RegionImageUrl;  

            await dbContext.SaveChangesAsync();

            return updateRegion;
        }


    }
}
