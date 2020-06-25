using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO, int userId);
        Task<PhotoDTO> GetPhotoAsync(int photoId, int userId);
        Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId, int userId);
        Task LikePhotoAsync(int photoId, int userId);
        Task RemovePhotoAsync(int photoId, int userId);
        Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO, int userId);
    }
}
