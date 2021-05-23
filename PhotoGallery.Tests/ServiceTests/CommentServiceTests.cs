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
    public class CommentServiceTests : ServiceTestsBase
    {
        IMapper mapper;

        Mock<IServiceHelper> mockHelper;
        Mock<IUnitOfWork> mockUow;

        CommentService service;

        public CommentServiceTests()
        {
            mapper = CreateMapperProfile();

            mockUow = new Mock<IUnitOfWork>();
            mockHelper = new Mock<IServiceHelper>();

            service = new CommentService(mapper, mockUow.Object, mockHelper.Object);
        }

        [Fact]
        public async Task AddCommentAsync_Should_AddCommentToUow()
        {
            var dto = new CommentAddDTO { PhotoId = 1, Text = "text" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(dto.PhotoId)).ReturnsAsync(new Photo());
            mockUow.Setup(uow => uow.Comments.AddAsync(It.Is<Comment>(c => c.Text == dto.Text))).Verifiable();

            await service.AddCommentAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddCommentAsync_Should_SaveUow()
        {
            var dto = new CommentAddDTO { PhotoId = 1, Text = "text" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(dto.PhotoId)).ReturnsAsync(new Photo());
            mockUow.Setup(uow => uow.Comments.AddAsync(It.IsAny<Comment>()));
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.AddCommentAsync(dto);

            mockUow.Verify();
        }

        [Fact]
        public async Task AddCommentAsync_Should_ReturnCommentDTO()
        {
            var dto = new CommentAddDTO { PhotoId = 1, Text = "text" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(dto.PhotoId)).ReturnsAsync(new Photo());
            mockUow.Setup(uow => uow.Comments.AddAsync(It.IsAny<Comment>()));

            var result = await service.AddCommentAsync(dto);

            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void AddCommentAsync_WrongPhotoId_Should_ThrowException()
        {
            var dto = new CommentAddDTO { PhotoId = 1, Text = "text" };

            mockUow.Setup(uow => uow.Photos.GetByIdAsync(dto.PhotoId)).ReturnsAsync((Photo)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Photo>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.AddCommentAsync(dto)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetCommentAsync_Should_ReturnCommentDTO()
        {
            var commentId = 1;
            var comment = new Comment { Text = "text" };

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync(comment);

            var result = await service.GetCommentAsync(commentId);

            result.Should().BeEquivalentTo(comment, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void GetCommentAsync_WrongCommentId_Should_ThrowException()
        {
            var commentId = 1;

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync((Comment)null);
            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Comment>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.GetCommentAsync(commentId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public async Task GetCommentsAsync_Should_ReturnCommentDTOs()
        {
            var photoId = 1;
            var comments = new List<Comment> { new Comment { Text = "text" } };

            mockUow.Setup(uow => uow.Comments.FindAsync(It.IsAny<Expression<Func<Comment, bool>>>())).ReturnsAsync(comments);

            var result = await service.GetCommentsAsync(photoId);

            result.Should().BeEquivalentTo(comments, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task RemoveCommentAsync_Should_RemoveCommentFromUow()
        {
            var commentId = 1;
            var userId = 1;
            var comment = new Comment { Text = "text" };

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync(comment);
            mockUow.Setup(uow => uow.Comments.Remove(comment)).Verifiable();

            await service.RemoveCommentAsync(commentId, userId);

            mockUow.Verify();
        }

        [Fact]
        public async Task RemoveCommentAsync_Should_SaveUow()
        {
            var commentId = 1;
            var userId = 1;
            var comment = new Comment { Text = "text" };

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync(comment);
            mockUow.Setup(uow => uow.SaveAsync()).Verifiable();

            await service.RemoveCommentAsync(commentId, userId);

            mockUow.Verify();
        }

        [Fact]
        public void RemoveCommentAsync_WrongCommentId_Should_ThrowException()
        {
            var commentId = 1;
            var userId = 1;

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync((Comment)null);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(
                It.IsAny<Comment>())).Throws<PhotoGalleryNotFoundException>();

            service.Invoking(s => s.RemoveCommentAsync(commentId, userId)).Should().Throw<PhotoGalleryNotFoundException>();
        }

        [Fact]
        public void RemoveCommentAsync_WrongUserId_Should_ThrowException()
        {
            var commentId = 1;
            var userId = 1;
            var comment = new Comment { UserId = 2 };

            mockUow.Setup(uow => uow.Comments.GetByIdAsync(commentId)).ReturnsAsync(comment);

            mockHelper.Setup(helper => helper.ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(
                It.IsAny<int>(), It.IsAny<int>())).Throws<PhotoGalleryNotAllowedException>();

            service.Invoking(s => s.RemoveCommentAsync(commentId, userId)).Should().Throw<PhotoGalleryNotAllowedException>();
        }       

    }
}
