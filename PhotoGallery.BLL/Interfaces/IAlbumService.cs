using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IAlbumService
    {
        Task<AlbumDTO> AddAlbumAsync(AlbumAddDTO albumDTO);
        Task<AlbumDTO> GetAlbumAsync(int albumId);
        Task<IEnumerable<AlbumDTO>> GetAlbumsAsync(int userId);
        Task RemoveAlbumAsync(int albumId, int userId);
        Task UpdateAlbumAsync(AlbumUpdateDTO albumDTO, int userId);
    }
}
