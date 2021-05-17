using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;

        public PhotoService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(photoDTO.AlbumId);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != photoDTO.UserId)
            {
                throw new ValidationException("You don`t have permission to add photos to this album");
            }

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

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            return mapper.Map<PhotoDTO>(photo);
        }

        public async Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId)
        {
            var photos = await unitOfWork.Photos.Find(p => p.AlbumId == albumId);

            return mapper.Map<IEnumerable<PhotoDTO>>(photos);
        }

        public async Task RemovePhotoAsync(int photoId, int userId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            if (photo.Album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to delete this photo");
            }            

            var album = await unitOfWork.Albums.GetByIdAsync(photo.AlbumId);
            album.Updated = DateTime.Now;

            unitOfWork.Photos.Remove(photo);
            unitOfWork.Albums.Update(album);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoDTO.Id);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            if (photo.Album.UserId != photoDTO.UserId)
            {
                throw new ValidationException("You don`t have permission to update this photo");
            }

            photo.Name = photoDTO.Name;

            unitOfWork.Photos.Update(photo);
            await unitOfWork.SaveAsync();
        }

        public async Task LikePhotoAsync(int photoId, int userId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

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