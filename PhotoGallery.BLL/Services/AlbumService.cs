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
    public class AlbumService : Service, IAlbumService
    {
        public AlbumService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        //Do not forget to update userId in controller!
        public async Task<AlbumDTO> AddAlbumAsync(AlbumAddDTO albumDTO)
        {
            if (albumDTO == null)
            {
                throw null;
            }

            Album album = _mp.Map<Album>(albumDTO);

            var curDateTime = DateTime.Now;

            album.Created = curDateTime;
            album.Updated = curDateTime;

            await _unit.Albums.AddAsync(album);
            await _unit.SaveAsync();

            return _mp.Map<AlbumDTO>(album);
        }

        public async Task<AlbumDTO> GetAlbumAsync(int albumId)
        {
            var album = await _unit.Albums.GetByIdAsync(albumId);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            return _mp.Map<AlbumDTO>(album);
        }

        public async Task<IEnumerable<AlbumDTO>> GetAlbumsAsync(int userId)
        {
            var user = await _unit.UserManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            var albums = user.Albums;

            return _mp.Map<IEnumerable<AlbumDTO>>(albums);
        }

        public async Task RemoveAlbumAsync(int albumId, int userId)
        {
            var album = await _unit.Albums.GetByIdAsync(albumId);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to delete this album");
            }

            _unit.Albums.Remove(album);
            await _unit.SaveAsync();
        }

        public async Task UpdateAlbumAsync(AlbumUpdateDTO albumDTO, int userId)
        {
            if (albumDTO == null)
            {
                throw null;
            }

            if (albumDTO.Name == null && albumDTO.Description == null)
            {
                return;
            }

            var album = await _unit.Albums.GetByIdAsync(albumDTO.Id);

            if (album == null)
            {
                throw new ValidationException("Album was not found");
            }

            if (album.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to update this album");
            }

            if (albumDTO.Name != null)
            {
                album.Name = albumDTO.Name;
            }

            if (albumDTO.Description != null)
            {
                album.Description = albumDTO.Description != "" ? albumDTO.Description : null;
            }

            album.Updated = DateTime.Now;

            _unit.Albums.Update(album);
            await _unit.SaveAsync();
        }
    }
}