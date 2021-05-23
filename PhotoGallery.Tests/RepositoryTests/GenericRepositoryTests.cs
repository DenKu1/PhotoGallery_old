using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using FluentAssertions;
using Moq;
using Xunit;

using PhotoGallery.DAL.Repositories;

namespace PhotoGallery.Tests.Repository
{
    public class TestEntity
    {
        public string Name { get; set; }
    }

    public class GenericRepositoryTests
    {
        Mock<DbContext> mockDbContext;
        Mock<DbSet<TestEntity>> mockDbSet;

        GenericRepository<TestEntity> genericRepository;

        public GenericRepositoryTests()
        {           
            mockDbSet = new Mock<DbSet<TestEntity>>();

            mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(mockDbSet.Object);

            genericRepository = new GenericRepository<TestEntity>(mockDbContext.Object);
        }

        [Fact]
        public async Task AddAsync_Should_AddToContext()
        {
            var testEnity = new TestEntity { Name = "ABC" };
            
            mockDbSet.Setup(x => x.AddAsync(testEnity, It.IsAny<CancellationToken>())).Verifiable();

            await genericRepository.AddAsync(testEnity);

            mockDbSet.Verify();
        }

        [Fact]
        public async Task AddRangeAsync_Should_AddRangeToContext()
        {
            var testEnites = new List<TestEntity> { new TestEntity { Name = "ABC" } };
            
            mockDbSet.Setup(x => x.AddRangeAsync(testEnites, It.IsAny<CancellationToken>())).Verifiable();

            await genericRepository.AddRangeAsync(testEnites);

            mockDbSet.Verify();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTestEntityAsync()
        {
            int entityId = 1;
            var testEnity = new TestEntity { Name = "ABC" };

            mockDbSet.Setup(x => x.FindAsync(entityId)).Returns(new ValueTask<TestEntity>(testEnity)).Verifiable();

            var result = await genericRepository.GetByIdAsync(entityId);

            mockDbSet.Verify();
            result.Should().Be(testEnity);
        }

        [Fact]
        public void Remove_Should_RemoveFromContext()
        {
            var testEnity = new TestEntity { Name = "ABC" };
            mockDbSet.Setup(x => x.Remove(testEnity)).Verifiable();

            genericRepository.Remove(testEnity);

            mockDbSet.Verify();
        }

        [Fact]
        public void RemoveRange_ShouldRemoveRangeFromContext()
        {
            var testEnites = new List<TestEntity> { new TestEntity { Name = "ABC" } };
            mockDbSet.Setup(x => x.RemoveRange(testEnites)).Verifiable();

            genericRepository.RemoveRange(testEnites);

            mockDbSet.Verify();
        }

    }
}
