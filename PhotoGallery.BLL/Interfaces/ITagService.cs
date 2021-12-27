using System.Collections.Generic;
using System.Threading.Tasks;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;

namespace PhotoGallery.BLL.Interfaces
{
    public interface ITagService
    {
        Task AddTagsToPhotoAsync(IEnumerable<string> tagNames, int photoId);
        Task<IEnumerable<string>> GetTagsForPhotoAsync(int photoId);
        Task RemoveTagFromPhotoAsync(string tagName, int photoId);
        
        Task AddTagsToUserAsync(IEnumerable<string> tagNames, int userId);
        Task<IEnumerable<string>> GetTagsForUserAsync(int userId);
        Task RemoveTagFromUserAsync(string tagName, int userId);
    }
}
