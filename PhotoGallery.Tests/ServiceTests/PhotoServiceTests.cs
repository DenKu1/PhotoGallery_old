using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.BLL.Services;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.Tests.ServiceTests.Base;

namespace PhotoGallery.Tests.ServiceTests
{
    public class PhotoServiceTests : ServiceTestsBase
    {
        IMapper mapper;

        Mock<IServiceHelper> mockHelper;
        Mock<IUnitOfWork> mockUow;

        PhotoService service;

        public PhotoServiceTests()
        {
            mapper = CreateMapperProfile();

            mockUow = new Mock<IUnitOfWork>();
            mockHelper = new Mock<IServiceHelper>();

            service = new PhotoService(mapper, mockUow.Object, mockHelper.Object);
        }

        [Fact]
        public async Task AddPhotoAsync_Should_AddPhotoToUow()
        {
            var dto = new PhotoAddDTO { AlbumId = 1, Name = "name", Path = "path" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(dto.AlbumId)).ReturnsAsync(new Album());
            mockUow.Setup(uow => uow.Photos.AddAsync(It.Is<Photo>(p => p.Name == dto.Name))).Verifiable();

            await service.AddPhotoAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddPhotoAsync_Should_SaveUow()
        {
            var dto = new PhotoAddDTO { AlbumId = 1, Name = "name", Path = "path" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(dto.AlbumId)).ReturnsAsync(new Album());
            mockUow.Setup(uow => uow.Photos.AddAsync(It.IsAny<Photo>()));
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.AddPhotoAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddPhotoAsync_Should_UpdateAlbumInUow()
        {
            var photoDTO = new PhotoAddDTO { AlbumId = 1, Name = "name", Path = "path" };
            var album = new Album { UserId = 1, Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(photoDTO.AlbumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.Photos.AddAsync(It.IsAny<Photo>()));
            mockUow.Setup(uow => uow.Albums.Update(album)).Verifiable();

            await service.AddPhotoAsync(photoDTO);

            mockUow.Verify();
            album.Updated.Should().NotBe(default);
        }

        [Fact]
        public async Task AddPhotoAsync_Should_ReturnPhotoDTO()
        {
            var dto = new PhotoAddDTO { AlbumId = 1, Name = "name", Path = "path" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(dto.AlbumId)).ReturnsAsync(new Album());
            mockUow.Setup(uow => uow.Photos.AddAsync(It.IsAny<Photo>()));

            var result = await service.AddPhotoAsync(dto);

            result.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void AddPhotoAsync_WrongAlbumId_Should_ThrowException()
        {
            var dto = new PhotoAddDTO { AlbumId = 1, Name = "name", Path = "path" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(dto.AlbumId)).ReturnsAsync((Album)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Album>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.AddPhotoAsync(dto)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public void AddPhotoAsync_WrongUserId_Should_ThrowException()
        {
            var photoDTO = new PhotoAddDTO { UserId = 1, AlbumId = 1, Name = "name", Path = "path" };
            var album = new Album { UserId = 2, Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(photoDTO.AlbumId)).ReturnsAsync(album);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.AddPhotoAsync(photoDTO)).Should().Throw<PhotoGalleryNotAllowedException>();
        }

        [Fact]
        public async Task GetPhotoAsync_Should_ReturnPhotoDTO()
        {
            var photoId = 1;
            var photo = new Photo { Name = "name", Path = "path" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);

            var result = await service.GetPhotoAsync(photoId);

            result.Should().BeEquivalentTo(photo, options => options.ExcludingMissingMembers()
                .Excluding(p => p.Likes));
        }

        [Fact]
        public void GetPhotoAsync_WrongPhotoId_Should_ThrowException()
        {
            var photoId = 1;

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync((Photo)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Photo>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.GetPhotoAsync(photoId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetPhotosAsync_Should_ReturnPhotoDTOs()
        {
            var albumId = 1;
            var photos = new List<Photo> { new Photo { Name = "name", Path = "path" } };

            mockUow.Setup(uow => uow.Photos.FindAsync(It.IsAny<Expression<Func<Photo, bool>>>())).ReturnsAsync(photos);

            var result = await service.GetPhotosAsync(albumId);

            result.Should().BeEquivalentTo(photos, options => options.ExcludingMissingMembers()
                .Excluding(p => p.Likes));
        }

        [Fact]
        public async Task RemovePhotoAsync_Should_RemovePhotoFromUow()
        {
            var photoId = 1;
            var userId = 1;
            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Albums.GetByIdAsync(photo.AlbumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.Photos.Remove(photo)).Verifiable();

            await service.RemovePhotoAsync(photoId, userId);

            mockUow.Verify();
        }

        [Fact]
        public async Task RemovePhotoAsync_Should_UpdateAlbumInUow()
        {
            var photoId = 1;
            var userId = 1;
            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Albums.GetByIdAsync(photo.AlbumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.Albums.Update(album)).Verifiable();

            await service.RemovePhotoAsync(photoId, userId);

            mockUow.Verify();
            album.Updated.Should().NotBe(default);
        }       

        [Fact]
        public async Task RemovePhotoAsync_Should_SaveUow()
        {
            var photoId = 1;
            var userId = 1;
            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Albums.GetByIdAsync(photo.AlbumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.RemovePhotoAsync(photoId, userId);

            mockUow.Verify();
        }

        [Fact]
        public void RemovePhotoAsync_WrongPhotoId_Should_ThrowException()
        {
            var photoId = 1;
            var userId = 1;

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync((Photo)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Photo>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.RemovePhotoAsync(photoId, userId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        
        [Fact]
        public void RemovePhotoAsync_WrongUserId_Should_ThrowException()
        {
            var photoId = 1;
            var userId = 1;
            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.RemovePhotoAsync(photoId, userId)).Should().Throw<PhotoGalleryNotAllowedException>();
        }
        
        [Fact]
        public async Task UpdatePhotoAsync_Should_UpdatePhotoInUow()
        {
            var photoDTO = new PhotoUpdateDTO { Name = "name" };

            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoDTO.Id)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Photos.Update(photo)).Verifiable();

            await service.UpdatePhotoAsync(photoDTO);

            mockUow.Verify();
            photo.Name.Should().Be(photoDTO.Name);
        }

        [Fact]
        public async Task UpdatePhotoAsync_Should_SaveUow()
        {
            var photoDTO = new PhotoUpdateDTO { Name = "name" };

            var album = new Album { UserId = 1, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoDTO.Id)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.UpdatePhotoAsync(photoDTO);

            mockUow.Verify();
        }

        [Fact]
        public void UpdatePhotoAsync_WrongPhotoId_Should_ThrowException()
        {
            var photoDTO = new PhotoUpdateDTO { Id = 1, Name = "name" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoDTO.Id)).ReturnsAsync((Photo)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Photo>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.UpdatePhotoAsync(photoDTO)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public void UpdatePhotoAsync_WrongUserId_Should_ThrowException()
        {
            var photoDTO = new PhotoUpdateDTO { Id = 1, UserId = 1, Name = "name" };
            var album = new Album { UserId = 2, Name = "name", Description = "desc" };
            var photo = new Photo { Name = "name", Path = "path", Album = album };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoDTO.Id)).ReturnsAsync(photo);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.UpdatePhotoAsync(photoDTO)).Should().Throw<PhotoGalleryNotAllowedException>();
        }

        [Fact]
        public async Task LikePhotoAsync_NotLiked_Should_AddLikeToUow()
        {
            var photoId = 1;
            var userId = 1;

            var likes = new List<Like>();
            var photo = new Photo { Name = "name", Path = "path", Likes = likes };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Likes.AddAsync(It.Is<Like>(l => l.PhotoId == photoId && l.UserId == userId))).Verifiable();

            await service.LikePhotoAsync(photoId, userId);

            mockUow.Verify();
        }

        [Fact]
        public async Task LikePhotoAsync_AlreadyLiked_Should_RemoveFromUow()
        {
            var photoId = 1;
            var userId = 1;
           
            var likes = new List<Like> { new Like { UserId = userId, PhotoId = photoId } };     
            var photo = new Photo { Name = "name", Path = "path", Likes = likes };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Likes.Remove(It.Is<Like>(l => l.PhotoId == photoId && l.UserId == userId))).Verifiable();

            await service.LikePhotoAsync(photoId, userId);

            mockUow.Verify();
        }

        [Fact]
        public async Task LikePhotoAsync_Should_SaveUow()
        {
            var photoId = 1;
            var userId = 1;

            var likes = new List<Like>();
            var photo = new Photo { Name = "name", Path = "path", Likes = likes };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(photoId)).ReturnsAsync(photo);
            mockUow.Setup(uow => uow.Likes.AddAsync(It.IsAny<Like>()));
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.LikePhotoAsync(photoId, userId);

            mockUow.Verify();
        }
    }
}
