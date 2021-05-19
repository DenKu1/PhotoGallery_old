using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using PhotoGallery.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.EF;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoGallery.Tests.Repository
{
    public class UserRepositoryTests
    {
        //CancellationToken cancellationToken;
        //ValueTask<TestEntity> valueTask;

        Mock<GalleryContext> mockDbContext;
        Mock<DbSet<User>> mockDbSet;

        UserRepository userRepository;

        public UserRepositoryTests()
        {
            //cancellationToken = new CancellationToken(false);
            //valueTask = new ValueTask<TestEntity>();


            mockDbSet = new Mock<DbSet<User>>();

            var mockOptions = new Mock<DbContextOptions<GalleryContext>>();
            mockDbContext = new Mock<GalleryContext>(mockOptions.Object);
            mockDbContext.SetupGet(x => x.Users).Returns(mockDbSet.Object);

            userRepository = new UserRepository(mockDbContext.Object);
        }

        [Fact]
        public void GetAllAsync_should_return_list()
        {
            var users = new List<User> { new User { UserName = "ABC" } };
            var mockTask = new Mock<Task<List<User>>>();
            mockTask.SetupGet(x => x.Result).Returns(users);

            mockDbSet.Setup(x => x.ToListAsync(It.IsAny<CancellationToken>())).Returns(mockTask.Object).Verifiable();

            var result = userRepository.GetAllAsync();

            mockDbSet.Verify();
            Assert.Equal(users, result.Result);
        }
    }
}