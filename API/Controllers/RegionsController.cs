using API.Data;
using API.Models.Domain;
using API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // lấy tất cả bảng
        [HttpGet]
        public IActionResult GetAll()
        {
            /*  hardcode dữ hiệu
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Ha Noi",
                    Code = "HAN",
                    RegionImageUrl = "https://hanoi"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Thanh Pho HCM",
                    Code = "HCM",
                    RegionImageUrl = ""
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Da Nang",
                    Code = "DAN",
                    RegionImageUrl = "https://danang"
                },

            };
            */

            // kết nối DB dùng DbContext
            //tạo biến region và lấy danh sách region có trong bảng region
            //Get data from database - domain models
            var regions = dbContext.Regions.ToList();

            //map domain models to DTOs
            var regionDTOs = new List<RegionDto>();

            //duyệt từng phần tử trong danh sách regions
            foreach (var region in regions)
            {
                regionDTOs.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }


            //return DTOs 
            return Ok(regionDTOs);

        }

        //lấy theo id https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            // C1: sử dụng find() => chỉ dùng được khi tìm kiếm với khóa chinh 
            //var regions = dbContext.Regions.Find(id);


            //C2 : dùng LINQ => dùng trong mọi trường hợp, linh hoạt hơn

            var regions = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            //kiểm tra kết quả trả về
            if (regions == null)
            {
                return NotFound(
                new
                {
                    Status = 404,
                    success = false,
                    message = "không tìm thấy id"
                });
            }


            var regionDTO = new RegionDto
            {
                Id = regions.Id,
                Code = regions.Code,
                Name = regions.Name,
                RegionImageUrl = regions.RegionImageUrl
            };

            if (regionDTO == null) return NotFound(
                new
                {
                    Status = 404,
                    success = false,
                    message = "không tìm thấy id"
                });
            else { return Ok(regions); }
        }


        //Post tạo region mới

        [HttpPost]
        public IActionResult CreatRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map DTO to domain model
            var rgion = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // sử dụng domain để tạo region
            dbContext.Regions.Add(rgion);

            dbContext.SaveChanges();

            // map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = rgion.Id,
                Code = rgion.Code,
                Name = rgion.Name,
                RegionImageUrl = rgion.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


    }
}
