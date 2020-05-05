using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IUserService : IService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task CreateUserAsync(UserRegisterDTO data);
        Task<string> LoginAsync(UserLoginDTO data);
        Task<UserDTO> GetUserProfileAsync(string userId);
        //Task UpdateProfile(UserDTO user);
        Task DeleteUserAsync(int id);
    }
}
