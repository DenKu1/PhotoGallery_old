using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IUserService : IService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task CreateUserAsync(UserRegisterDTO data);
        Task<(string, UserDTO)> LoginAsync(UserLoginDTO data);
        Task<UserDTO> GetUserByUserNameAsync(string userName);
        Task DeleteUserAsync(int id);
    }
}
