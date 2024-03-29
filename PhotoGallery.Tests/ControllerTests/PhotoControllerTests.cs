﻿using System;
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
    public class PhotoControllerTests : ControllerTestsBase
    {
        int userId = 1;

        IMapper mapper;

        Mock<IPhotoService> mockPhotoService;
        Mock<ITagService> mockTagService;

        PhotoController controller;

        public PhotoControllerTests()
        {
            mapper = CreateMapperProfile();
            mockPhotoService = new Mock<IPhotoService>();
            mockTagService = new Mock<ITagService>();

            controller = new PhotoController(mapper, mockPhotoService.Object, mockTagService.Object);
            AddIdentity(controller, userId);
        }

        [Fact]
        public void GetPhotos_Should_ReturnPhotoModels()
        {
            var albumId = 1;
            var photoDTOs = new List<PhotoDTO> { new PhotoDTO { Name = "name", Path = "path", Tags = Array.Empty<string>() } };

            mockPhotoService.Setup(s => s.GetPhotosAsync(userId, albumId)).ReturnsAsync(photoDTOs).Verifiable();

            controller.GetPhotos(userId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(photoDTOs);
            mockPhotoService.Verify();
        }

        [Fact]
        public void GetPhoto_Should_ReturnPhotoModel()
        {
            var photoId = 1;
            var photoDTO = new PhotoDTO { Name = "name", Path = "path", Tags = Array.Empty<string>() };

            mockPhotoService.Setup(s => s.GetPhotoAsync(photoId)).ReturnsAsync(photoDTO).Verifiable();

            controller.GetPhoto(photoId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(photoDTO);
            mockPhotoService.Verify();
        }

        [Fact]
        public void PostPhoto_Should_ReturnPhotoModel()
        {
            var albumId = 1;
            var photoModel = new PhotoAddModel { Name = "name", Path = "path" };
            var photoDTO = new PhotoDTO { Name = "name", Path = "path" };

            mockPhotoService.Setup(s => s.AddPhotoAsync(It.Is<PhotoAddDTO>(dto =>
                dto.Name == photoModel.Name && dto.Path == photoModel.Path))).ReturnsAsync(photoDTO).Verifiable();

            controller.PostPhoto(albumId, photoModel).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(photoModel);
            mockPhotoService.Verify();
        }

        [Fact]
        public void LikePhoto_Should_ReturnOk()
        {
            var photoId = 1;

            mockPhotoService.Setup(s => s.LikePhotoAsync(photoId, userId)).Verifiable();

            controller.LikePhoto(photoId).Result.Should().BeOfType<OkResult>();
            mockPhotoService.Verify();
        }

        [Fact]
        public void PutPhoto_Should_ReturnOk()
        {
            var photoId = 1;
            var photoModel = new PhotoUpdateModel { Name = "name" };

            mockPhotoService.Setup(s => s.UpdatePhotoAsync(It.Is<PhotoUpdateDTO>(dto =>
                dto.Name == photoModel.Name))).Verifiable();

            controller.PutPhoto(photoId, photoModel).Result.Should().BeOfType<OkResult>();
            mockPhotoService.Verify();
        }

        [Fact]
        public void DeletePhoto_Should_ReturnOk()
        {
            var photoId = 1;

            mockPhotoService.Setup(s => s.RemovePhotoAsync(photoId, userId)).Verifiable();

            controller.DeletePhoto(photoId).Result.Should().BeOfType<OkResult>();
            mockPhotoService.Verify();
        }

    }
}
