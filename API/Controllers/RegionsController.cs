using API.Data;
using API.Models.Domain;
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

            //return DTOs
            return Ok(regions);

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
            if (regions == null) return NotFound(
                new {
                    Status = 404,
                    success = false,
                    message = "không tìm thấy id"
                });
            else { return Ok(regions); }
            
        }
    }
}
