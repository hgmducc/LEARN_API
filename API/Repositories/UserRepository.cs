using API.Data;
using API.Models.Domain;
using API.Models.DTO;
using API.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {

        // the vao ket noi db 
        private readonly NZWalksDbContext dbContext;
        private string secretKey;
        public UserRepository(NZWalksDbContext dbContext, IConfiguration configuration)
        {
           this.dbContext = dbContext;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        // === KIEM TRA USERNAME CO BI TRUNG KHONG ===
        public bool IsUniqueUser(string username)
        {
            // kiểm tra, tìm kiếm username trong bảng LocalUsers
             var checkUser = dbContext.LocalUsers.FirstOrDefault(x => x.UserName == username);
            //null nếu không tìm thấy
            if(checkUser == null)
            {
                return true; // không trùng, tên mới, duy nhất
            }

            return false; //trùng tên trong database

        }

        // === DANG NHAP ===
        public Task<LoginResponseDTO> Login(LoginResponseDTO loginResponseDTO)
        {
            // lấy thông tin user từ database dựa trên username và password

            var user = dbContext.LocalUsers.FirstOrDefault(x => x.UserName == loginResponseDTO.User.UserName 
            && x.Password == loginResponseDTO.User.Password);

            // kiểm tra user có tồn tại không
            if(user == null)
            {
                return null;
            }

            // nếu tồn tại user thì sinh token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginRequestDto loginRequestDto = new LoginRequestDto()
            {
                Token = tokenHandler.WriteToken(token),
                UserName = user
            };

            return loginRequestDto

        }


        // === DANG KY ===
        public async Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto)
        {
            // tạo đối tượng user mới từ DTO
            LocalUser user = new()
            {
                UserName = registerationRequestDto.UserName,
                name = registerationRequestDto.name,
                Password = registerationRequestDto.Password,
                role = registerationRequestDto.role
            };

            // thêm user vào database
            await dbContext.LocalUsers.AddAsync(user);
            await dbContext.SaveChangesAsync(); 

            user.Password = ""; // xóa mật khẩu trước khi trả về

            return user;
        }
    }
}
