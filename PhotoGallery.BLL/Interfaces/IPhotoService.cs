using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO, int userId);
        Task<PhotoDTO> GetPhotoAsync(int photoId);
        Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId);
        Task LikePhotoAsync(int photoId, int userId);
        Task RemovePhotoAsync(int photoId, int userId);
        Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO, int userId);
    }
}
