using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            // tạo 1 chuỗi gồm các tên học sinh 
            string[] name = new string[] { "Duc", "Duy", "Mạnh" };

            return Ok(name);
        }
    }
}
