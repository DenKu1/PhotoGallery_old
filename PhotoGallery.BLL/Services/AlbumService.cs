using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class AlbumService : IAlbumService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;
        IServiceHelper helper;

        public AlbumService(IMapper mapper, IUnitOfWork unitOfWork, IServiceHelper helper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.helper = helper;
        }

        public async Task<AlbumDTO> AddAlbumAsync(AlbumAddDTO albumDTO)
        {
            var album = mapper.Map<Album>(albumDTO, opt => opt.Items["creationTime"] = DateTime.Now);

            await unitOfWork.Albums.AddAsync(album);
            await unitOfWork.SaveAsync();

            return mapper.Map<AlbumDTO>(album);
        }

        public async Task<AlbumDTO> GetAlbumAsync(int albumId)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(albumId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(album);

            return mapper.Map<AlbumDTO>(album);
        }

        public async Task<IEnumerable<AlbumDTO>> GetAlbumsAsync(int userId)
        {
            var albums = await unitOfWork.Albums.Find(a => a.UserId == userId);

            return mapper.Map<IEnumerable<AlbumDTO>>(albums);
        }

        public async Task RemoveAlbumAsync(int albumId, int userId)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(albumId);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(album);
            helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(album.UserId, userId);

            unitOfWork.Albums.Remove(album);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAlbumAsync(AlbumUpdateDTO albumDTO)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(albumDTO.Id);

            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(album);
            helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(album.UserId, albumDTO.UserId);

            albumDTO.Name = albumDTO.Name;
            albumDTO.Description = albumDTO.Description;

            album.Updated = DateTime.Now;

            unitOfWork.Albums.Update(album);
            await unitOfWork.SaveAsync();
        }
    }
}