using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PhotoGallery.BLL.DTO;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class PhotoService : Service, IPhotoService
    {
        public PhotoService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public async Task<PhotoDTO> AddPhotoAsync(PhotoAddDTO photoDTO, int userId)
        {
            if (photoDTO == null)
            {
                throw null;
            }

            var album = await _unit.Albums.GetByIdAsync(photoDTO.AlbumId);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to add photos to this album");
            }

            Photo photo = _mp.Map<Photo>(photoDTO);

            photo.Created = DateTime.Now;

            await _unit.Photos.AddAsync(photo);

            album.Updated = DateTime.Now;
            _unit.Albums.Update(album);

            await _unit.SaveAsync();

            return _mp.Map<PhotoDTO>(photo);
        }

        public async Task<PhotoDTO> GetPhotoAsync(int photoId)
        {
            var photo = await _unit.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            return _mp.Map<PhotoDTO>(photo);
        }

        public async Task<IEnumerable<PhotoDTO>> GetPhotosAsync(int albumId)
        {
            var album = await _unit.Albums.GetByIdAsync(albumId);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            var photos = album.Photos;

            return _mp.Map<IEnumerable<PhotoDTO>>(photos);
        }

        public async Task RemovePhotoAsync(int photoId, int userId)
        {
            var photo = await _unit.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            if (photo.Album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to delete this photo");
            }

            _unit.Photos.Remove(photo);

            var album = await _unit.Albums.GetByIdAsync(photo.AlbumId);
            album.Updated = DateTime.Now;

            _unit.Albums.Update(album);

            await _unit.SaveAsync();
        }

        public async Task UpdatePhotoAsync(PhotoUpdateDTO photoDTO, int userId)
        {
            if (photoDTO == null)
            {
                throw null;
            }

            var photo = await _unit.Photos.GetByIdAsync(photoDTO.Id);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            if (photo.Album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to update this photo");
            }

            photo.Name = photoDTO.Name;

            _unit.Photos.Update(photo);
            await _unit.SaveAsync();
        }

        public async Task LikePhotoAsync(int photoId, int userId)
        {
            if (await _unit.Photos.GetByIdAsync(photoId) == null)
            {
                throw new ValidationException("Photo was not found");
            }

            Like like = await _unit.Likes.SingleOrDefaultAsync(
                l => l.PhotoId == photoId && l.UserId == userId);

            if (like == null)
            {
                like = new Like { PhotoId = photoId, UserId = userId };
                await _unit.Likes.AddAsync(like);
            }
            else
            {
                _unit.Likes.Remove(like);
            }

            await _unit.SaveAsync();
        }
    }
}