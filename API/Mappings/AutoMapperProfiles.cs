using API.Models.Domain;
using API.Models.DTO;
using AutoMapper;

namespace API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // tạo auto mapper giữa Region và RegionDto

            CreateMap<Region, RegionDto>().ReverseMap();
        }
    }
}
