using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using PhotoGallery.API.Models.Out;
using PhotoGallery.IntegrationTests.EndpointTests.Base;
using PhotoGallery.IntegrationTests.Utilities;

namespace PhotoGallery.IntegrationTests.EndpointTests
{
    public class CommentIntergationTests : IClassFixture<IntegrationTestsFixture>
    {
        IntegrationTestsFixture fixture;

        public CommentIntergationTests(IntegrationTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetComments_Should_ReturnCommentModels()
        {
            var photoId = 1;
            var requestUri = $"api/photos/{photoId}/comments";

            var response = await fixture.client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var comments = await response.DeserializeAsync<IEnumerable<CommentModel>>();

            comments.Count().Should().Be(3);
        }

        [Fact]
        public async Task GetComment_Should_ReturnAlbumModel()
        {
            var commentId = 1;
            var requestUri = $"api/comments/{commentId}";

            var response = await fixture.client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var comment = await response.DeserializeAsync<AlbumModel>();

            comment.Id.Should().Be(commentId);
        }
    }
}
