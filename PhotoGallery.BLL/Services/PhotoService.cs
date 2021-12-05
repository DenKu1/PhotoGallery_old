using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;
        IServiceHelper helper;

        public PhotoService(IMapper mapper, IUnitOfWork unitOfWork, IServiceHelper helper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.helper = helper;
        }

        public async Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(photoDTO.AlbumId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(album);
            helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(album.UserId, photoDTO.UserId);

            var creationTime = DateTime.Now;

            var photo = mapper.Map<Photo>(photoDTO, opt => opt.Items["creationTime"] = creationTime);
            album.Updated = creationTime;

            await unitOfWork.Photos.AddAsync(photo);
            unitOfWork.Albums.Update(album);
            await unitOfWork.SaveAsync();

            return mapper.Map<PhotoDTO>(photo);
        }

        public async Task<PhotoDTO> GetPhotoAsync(int photoId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);

            return mapper.Map<PhotoDTO>(photo);
        }

        public async Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId, int userId)
        {
            var photos = await unitOfWork.Photos.FindAsync(p => p.AlbumId == albumId);

            return mapper.Map<IEnumerable<PhotoDTO>>(photos, opt => opt.Items["userId"] = userId);
        }

        public async Task RemovePhotoAsync(int photoId, int userId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);
            helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(photo.Album.UserId, userId);         

            var album = await unitOfWork.Albums.GetByIdAsync(photo.AlbumId);
            album.Updated = DateTime.Now;

            unitOfWork.Photos.Remove(photo);
            unitOfWork.Albums.Update(album);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoDTO.Id);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);
            helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(photo.Album.UserId, photoDTO.UserId);

            mapper.Map(photoDTO, photo);

            unitOfWork.Photos.Update(photo);
            await unitOfWork.SaveAsync();
        }

        public async Task LikePhotoAsync(int photoId, int userId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);

            var like = photo.Likes.FirstOrDefault(l => l.UserId == userId);

            if (like == null)
            {
                await unitOfWork.Likes.AddAsync(new Like
                { 
                    PhotoId = photoId,
                    UserId = userId
                });
            }
            else
            {
                unitOfWork.Likes.Remove(like);
            }

            await unitOfWork.SaveAsync();
        }
    }
}