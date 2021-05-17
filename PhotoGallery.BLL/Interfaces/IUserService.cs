using System.Collections.Generic;
using System.Threading.Tasks;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task CreateUserAsync(UserRegisterDTO data);
        Task<UserWithTokenDTO> LoginAsync(UserLoginDTO data);
        Task<UserDTO> GetUserAsync(int id);
        Task<UserDTO> GetUserByUserNameAsync(string userName);
        Task RemoveUserAsync(int id);
    }
}
