using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.Tests.ControllerTests.Base;
using PhotoGallery.API.Controllers;
using PhotoGallery.API.Models.In;

namespace PhotoGallery.Tests.ControllerTests
{
    public class AlbumControllerTests : ControllerTestsBase
    {      
        int userId = 1;

        IMapper mapper;

        Mock<IAlbumService> mockService;

        AlbumController controller;

        public AlbumControllerTests()
        {
            mapper = CreateMapperProfile();
            mockService = new Mock<IAlbumService>();

            controller = new AlbumController(mapper, mockService.Object);
            AddIdentity(controller, userId);
        }

        [Fact]
        public void GetAlbums_Should_ReturnAlbumModels()
        {
            var albumDTOs = new List<AlbumDTO> { new AlbumDTO { Name = "name", Description = "desc" } };

            mockService.Setup(s => s.GetAlbumsAsync(userId)).ReturnsAsync(albumDTOs).Verifiable();

            controller.GetAlbums(userId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(albumDTOs);
            mockService.Verify();
        }

        [Fact]
        public void GetAlbum_Should_ReturnAlbumModel()
        {
            var albumId = 1;
            var albumDTO = new AlbumDTO { Name = "name", Description = "desc" };

            mockService.Setup(s => s.GetAlbumAsync(albumId)).ReturnsAsync(albumDTO).Verifiable();

            controller.GetAlbum(albumId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(albumDTO);
            mockService.Verify();
        }

        [Fact]
        public void PostAlbum_Should_ReturnAlbumModel()
        {
            var albumModel = new AlbumAddModel { Name = "name", Description = "desc" };
            var albumDTO = new AlbumDTO { Name = "name", Description = "desc" };

            mockService.Setup(s => s.AddAlbumAsync(It.Is<AlbumAddDTO>(dto =>
                dto.Name == albumModel.Name && dto.Description == albumModel.Description))).ReturnsAsync(albumDTO).Verifiable();

            controller.PostAlbum(albumModel).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(albumModel);
            mockService.Verify();
        }

        [Fact]
        public void PutAlbum_Should_ReturnOk()
        {
            var albumId = 1;
            var albumModel = new AlbumUpdateModel { Name = "name", Description = "desc" };

            mockService.Setup(s => s.UpdateAlbumAsync(It.Is<AlbumUpdateDTO>(dto =>
                dto.Name == albumModel.Name && dto.Description == albumModel.Description))).Verifiable();

            controller.PutAlbum(albumId, albumModel).Result.Should().BeOfType<OkResult>();
            mockService.Verify();
        }

        [Fact]
        public void DeleteAlbum_Should_ReturnOk()
        {
            var albumId = 1;            

            mockService.Setup(s => s.RemoveAlbumAsync(albumId, userId)).Verifiable();

            controller.DeleteAlbum(albumId).Result.Should().BeOfType<OkResult>();
            mockService.Verify();
        }

    }
}
