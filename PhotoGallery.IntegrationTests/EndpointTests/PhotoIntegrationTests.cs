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
    public class PhotoIntergationTests : IClassFixture<IntegrationTestsFixture>
    {
        IntegrationTestsFixture fixture;

        public PhotoIntergationTests(IntegrationTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetPhotos_Should_ReturnPhotoModels()
        {
            var albumId = 1;
            var requestUri = $"api/albums/{albumId}/photos";

            var response = await fixture.client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photos = await response.DeserializeAsync<IEnumerable<PhotoModel>>();

            photos.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetPhoto_Should_ReturnAlbumModel()
        {
            var photoId = 1;
            var requestUri = $"api/photos/{photoId}";

            var response = await fixture.client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var photo = await response.DeserializeAsync<PhotoModel>();

            photo.Id.Should().Be(photoId);
        }
    }
}
