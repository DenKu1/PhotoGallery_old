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
    public class CommentControllerTests : ControllerTestsBase
    {
        int userId = 1;

        IMapper mapper;

        Mock<ICommentService> mockService;

        CommentController controller;

        public CommentControllerTests()
        {
            mapper = CreateMapperProfile();
            mockService = new Mock<ICommentService>();

            controller = new CommentController(mapper, mockService.Object);
            AddIdentity(controller, userId);
        }

        [Fact]
        public void GetComments_Should_ReturnCommentModels()
        {
            var photoId = 1;
            var commentDTOs = new List<CommentDTO> { new CommentDTO { Text = "text" } };          

            mockService.Setup(s => s.GetCommentsAsync(photoId)).ReturnsAsync(commentDTOs).Verifiable();

            controller.GetComments(userId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commentDTOs);
            mockService.Verify();
        }

        [Fact]
        public void GetComment_Should_ReturnCommentModel()
        {
            var commentId = 1;
            var commentDTO = new CommentDTO { Text = "text" };

            mockService.Setup(s => s.GetCommentAsync(commentId)).ReturnsAsync(commentDTO).Verifiable();

            controller.GetComment(commentId).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commentDTO);
            mockService.Verify();
        }

        [Fact]
        public void PostComment_Should_ReturnCommentModel()
        {
            var photoId = 1;
            var commentModel = new CommentAddModel { Text = "text" };
            var commentDTO = new CommentDTO { Text = "text" };

            mockService.Setup(s => s.AddCommentAsync(It.Is<CommentAddDTO>(dto =>
                dto.Text == commentModel.Text))).ReturnsAsync(commentDTO).Verifiable();

            controller.PostComment(photoId, commentModel).Result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commentModel);
            mockService.Verify();
        }

        [Fact]
        public void DeleteAlbum_Should_ReturnOk()
        {
            var commentId = 1;

            mockService.Setup(s => s.RemoveCommentAsync(commentId, userId)).Verifiable();

            controller.DeleteComment(commentId).Result.Should().BeOfType<OkResult>();
            mockService.Verify();
        }

    }
}
