using System;
using System.Collections.Generic;
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
    public class AlbumService : IAlbumService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;

        public AlbumService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        //Do not forget to update userId in controller!
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

            if (album == null)
            {
                throw new ValidationException("Album was not found"); //TODO: Change exception
            }

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

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to delete this album");
            }

            unitOfWork.Albums.Remove(album);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAlbumAsync(AlbumUpdateDTO albumDTO)
        {
            var album = await unitOfWork.Albums.GetByIdAsync(albumDTO.Id);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != albumDTO.UserId)
            {
                throw new ValidationException("You don`t have permission to update this album");
            }

            albumDTO.Name = albumDTO.Name;
            albumDTO.Description = albumDTO.Description;

            album.Updated = DateTime.Now;

            unitOfWork.Albums.Update(album);
            await unitOfWork.SaveAsync();
        }
    }
}