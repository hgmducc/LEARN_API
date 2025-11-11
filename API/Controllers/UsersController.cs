using System.Net;
using API.Models;
using API.Models.DTO;
using API.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/usersAuth")]
    [ApiController]
    public class UsersController : Controller
    {

        private readonly IUserRepository userRepository;
        protected APIResponse response;
        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.response = new();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await userRepository.Login(model);

            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {

                response.statusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("User or password is incorrect");
                return BadRequest(response);
            }

            response.statusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = loginResponse;
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            bool isUnique = userRepository.IsUniqueUser(model.UserName);
            if (!isUnique)
            {
                return BadRequest(
                    new
                    {
                        message = "Username already exists"
                    });
            }
            var user = await userRepository.Register(model);
            if (user == null)
            {
                return BadRequest(
                    new
                    {
                        message = "Error while registering"
                    });
            }
            return Ok();
        }
    }
}
