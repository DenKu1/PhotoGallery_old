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

namespace PhotoGallery.Tests.Repository
{
    public class TestEntity
    {
        public string Name { get; set; }
    }

    public class GenericRepositoryTests
    {
        CancellationToken cancellationToken;
        ValueTask<TestEntity> valueTask;

        Mock<DbContext> mockDbContext;
        Mock<DbSet<TestEntity>> mockDbSet;

        GenericRepository<TestEntity> genericRepository;

        public GenericRepositoryTests()
        {
            cancellationToken = new CancellationToken(false);
            valueTask = new ValueTask<TestEntity>();


            mockDbSet = new Mock<DbSet<TestEntity>>();

            mockDbContext = new Mock<DbContext>();
            mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(mockDbSet.Object);

            genericRepository = new GenericRepository<TestEntity>(mockDbContext.Object);
        }

        [Fact]
        public void AddAsync_should_add_to_context()
        {
            var testEnity = new TestEntity { Name = "ABC" };
            mockDbSet.Setup(x => x.AddAsync(testEnity, It.IsAny<CancellationToken>())).Verifiable();

            genericRepository.AddAsync(testEnity);

            mockDbSet.Verify();
        }

        [Fact]
        public void AddRangeAsync_should_add_range_to_context()
        {
            var testEnites = new List<TestEntity> { new TestEntity { Name = "ABC" } };
            mockDbSet.Setup(x => x.AddRangeAsync(testEnites, It.IsAny<CancellationToken>())).Verifiable();

            genericRepository.AddRangeAsync(testEnites);

            mockDbSet.Verify();
        }

        [Fact]
        public void Find_should_add_return_list()
        {
            var mockQuariable = new Mock<IQueryable<TestEntity>>();

            var testEnites = new List<TestEntity> { new TestEntity { Name = "ABC" } };
            mockDbSet.Setup(x => x.Where(It.IsAny<Func<TestEntity, bool>>())).Returns(mockQuariable.Object).Verifiable();

            var result = genericRepository.Find(x => x.Name == "ABC");

            mockDbSet.Verify();
            Assert.Equal(testEnites, result.Result);
        }

        [Fact]
        public void GetByIdAsync_should_return_entity()
        {
            int entityId = 1;
            var testEnity = new TestEntity { Name = "ABC" };

            mockDbSet.Setup(x => x.FindAsync(entityId)).Returns(new ValueTask<TestEntity>(testEnity)).Verifiable();

            var result = genericRepository.GetByIdAsync(entityId);

            mockDbSet.Verify();
            Assert.Equal(testEnity, result.Result);
        }

        [Fact]
        public void Remove_should_remove_from_context()
        {
            var testEnity = new TestEntity { Name = "ABC" };
            mockDbSet.Setup(x => x.Remove(testEnity)).Verifiable();

            genericRepository.Remove(testEnity);

            mockDbSet.Verify();
        }

        [Fact]
        public void RemoveRange_should_add_to_context()
        {
            var testEnites = new List<TestEntity> { new TestEntity { Name = "ABC" } };
            mockDbSet.Setup(x => x.RemoveRange(testEnites)).Verifiable();

            genericRepository.RemoveRange(testEnites);

            mockDbSet.Verify();
        }

    }
}
