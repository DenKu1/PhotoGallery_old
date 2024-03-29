﻿using System.Collections.Generic;
using System.Threading.Tasks;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;

namespace PhotoGallery.BLL.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO);
        Task<PhotoDTO> GetPhotoAsync(int photoId);
        Task<PhotoRecommendationsDTO> GetPhotoRecommendationsAsync(int photoId);
        Task<IEnumerable<PhotoDTO>> GetPhotosByTagsAsync(string[] tags, int limit);
        Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId, int userId);
        Task LikePhotoAsync(int photoId, int userId);
        Task RemovePhotoAsync(int photoId, int userId);
        Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO);
    }
}
