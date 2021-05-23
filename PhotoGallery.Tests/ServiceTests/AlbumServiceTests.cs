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
    public class AlbumServiceTests : ServiceTestsBase
    {
        IMapper mapper;

        Mock<IServiceHelper> mockHelper;
        Mock<IUnitOfWork> mockUow;        

        AlbumService service;               

        public AlbumServiceTests()
        {
            mapper = CreateMapperProfile();

            mockUow = new Mock<IUnitOfWork>();
            mockHelper = new Mock<IServiceHelper>();    

            service = new AlbumService(mapper, mockUow.Object, mockHelper.Object);
        }

        [Fact]
        public async Task AddAlbumAsync_Should_AddAlbumToUow()
        {
            var dto = new AlbumAddDTO { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.AddAsync(It.Is<Album>(a => a.Name == dto.Name))).Verifiable();

            await service.AddAlbumAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddAlbumAsync_Should_SaveUow()
        {
            var dto = new AlbumAddDTO { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.AddAsync(It.IsAny<Album>()));
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.AddAlbumAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddAlbumAsync_Should_ReturnAlbumDTO()
        {
            var dto = new AlbumAddDTO { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.AddAsync(It.IsAny<Album>()));

            var result = await service.AddAlbumAsync(dto);

            result.Should().BeEquivalentTo(dto);
            result.Created.Should().NotBe(default);
            result.Updated.Should().NotBe(default);
        }

        [Fact]
        public async Task GetAlbumAsync_Should_ReturnAlbumDTO()
        {
            var albumId = 1;
            var album = new Album { Name = "name", Description = "desc" };            

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync(album);

            var result = await service.GetAlbumAsync(albumId);

            result.Should().BeEquivalentTo(album, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetAlbumAsync_WrongAlbumId_Should_ThrowException()
        {
            var albumId = 1;

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync((Album)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Album>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.GetAlbumAsync(albumId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetAlbumsAsync_Should_ReturnAlbumDTOs()
        {
            var userId = 1;
            var albums = new List<Album> { new Album { Name = "name", Description = "desc" } };

            mockUow.Setup(uow => uow.Albums.FindAsync(It.IsAny<Expression<Func<Album, bool>>>())).ReturnsAsync(albums);

            var result = await service.GetAlbumsAsync(userId);

            result.Should().BeEquivalentTo(albums, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task RemoveAlbumAsync_Should_RemoveAlbumFromUow()
        {
            var albumId = 1;
            var userId = 1;
            var album = new Album { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.Albums.Remove(album)).Verifiable();

            await service.RemoveAlbumAsync(albumId, userId);

            mockUow.Verify();
        }

        [Fact]
        public async Task RemoveAlbumAsync_Should_SaveUow()
        {
            var albumId = 1;
            var userId = 1;
            var album = new Album { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.RemoveAlbumAsync(albumId, userId);

            mockUow.Verify();
        }

        [Fact]
        public void RemoveAlbumAsync_WrongAlbumId_Should_ThrowException()
        {
            var albumId = 1;
            var userId = 1;
            
            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync((Album)null);
            
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Album>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.RemoveAlbumAsync(albumId, userId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public void RemoveAlbumAsync_WrongUserId_Should_ThrowException()
        {
            var albumId = 1;
            var userId = 1;
            var album = new Album { UserId = 2 };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumId)).ReturnsAsync(album);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.RemoveAlbumAsync(albumId, userId)).Should().Throw<PhotoGalleryNotAllowedException>();
        }

        [Fact]
        public async Task UpdateAlbumAsync_Should_UpdateAlbumInUow()
        {
            var albumDTO = new AlbumUpdateDTO { Id = 1, Name = "newname", Description = "newdesc" };
            var album = new Album { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumDTO.Id)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.Albums.Update(album)).Verifiable();

            await service.UpdateAlbumAsync(albumDTO);

            mockUow.Verify();
            album.Name.Should().Be(albumDTO.Name);
            album.Description.Should().Be(albumDTO.Description);
            album.Updated.Should().NotBe(default);
        }

        [Fact]
        public async Task UpdateAlbumAsync_Should_SaveUow()
        {
            var albumDTO = new AlbumUpdateDTO { Id = 1, Name = "newname", Description = "newdesc" };
            var album = new Album { Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumDTO.Id)).ReturnsAsync(album);
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.UpdateAlbumAsync(albumDTO);

            mockUow.Verify();
        }

        [Fact]
        public void UpdateAlbumAsync_WrongAlbumId_Should_ThrowException()
        {
            var albumDTO = new AlbumUpdateDTO { Id = 1, Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumDTO.Id)).ReturnsAsync((Album)null);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Album>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.UpdateAlbumAsync(albumDTO)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public void UpdateAlbumAsync_WrongUserId_Should_ThrowException()
        {
            var albumDTO = new AlbumUpdateDTO { Id = 1, Name = "name", Description = "desc" };
            var album = new Album { Id = 2, Name = "name", Description = "desc" };

            mockUow.Setup(uow => uow.Albums.GetByIdAsync(albumDTO.Id)).ReturnsAsync(album);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.UpdateAlbumAsync(albumDTO)).Should().Throw<PhotoGalleryNotAllowedException>();
        }

    }
}
