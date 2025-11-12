using API.Data;
using API.Models.Domain;
using API.Models.DTO;
using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // === LẤY TẤT CẢ === 
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
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
            var regions = await regionRepository.GetAllAsyns();

            ////map domain models to DTOs
            //var regionDTOs = new List<RegionDto>();

            ////duyệt từng phần tử trong danh sách regions
            //foreach (var region in regions)
            //{
            //    regionDTOs.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}

            var regionsDto = mapper.Map<List<RegionDto>>(regions);


            //return DTOs 
            return Ok(regionsDto);

        }


        // === LẤY THEO ID ===
        //lấy theo id https://localhost:port/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "KH")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // C1: sử dụng find() => chỉ dùng được khi tìm kiếm với khóa chinh 
            //var regions = dbContext.Regions.Find(id);
            //C2 : dùng LINQ => dùng trong mọi trường hợp, linh hoạt hơn

            var regions = await regionRepository.GetRegionbyId(id);
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

            return Ok(regionDTO);
        }


        //=== POST TẠO REGION MỚI ===
        [HttpPost]
        public async Task<IActionResult> CreatRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map DTO to domain model
            var rgion = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // sử dụng domain để tạo region
            await regionRepository.creatRegion(rgion);

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


        // === PUT CẬP NHẬT DỮ LIỆU ===
        //PUT : api/regions/{id} => truyền vào id để sửa region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionsDto updateRegionsDto)
        {

            // map dto sang domain model
            var updateRegion = new Region
            {
                Code = updateRegionsDto.Code,
                Name = updateRegionsDto.Name,
                RegionImageUrl = updateRegionsDto.RegionImageUrl
            };

            updateRegion = await regionRepository.UpdateRegionbyId(id, updateRegion);

            //kiểm tra region có tồn tại không
            if (updateRegion == null)
            {
                return NotFound();
            }


            // chuyển DomainModel sang DTO
            var regionDto = new RegionDto
            {
                Id = updateRegion.Id,
                Code = updateRegion.Code,
                Name = updateRegion.Name,
                RegionImageUrl = updateRegion.RegionImageUrl
            };

            return Ok(regionDto);
        }


        // === DELETE XÓA DỮ LIỆU ===
        //DELETE : api/regions/{id} => truyền vào id để xóa region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
           var regionModel = await regionRepository.Delete(id);

            if (regionModel == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

    }
}
