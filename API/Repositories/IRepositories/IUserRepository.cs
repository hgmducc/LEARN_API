using API.Models.Domain;
using API.Models.DTO;

namespace API.Repositories.IRepositories
{
    public interface IUserRepository
    {
        //kiem tra username co bi trung khong, la duy nhat
        bool IsUniqueUser(string username);

        //dang nhap
        Task<LoginResponseDTO> Login(LoginResponseDTO loginResponseDTO );

        //dang ky
        Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto);

    }
}
