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
    public class AlbumIntergationTests : IClassFixture<IntegrationTestsFixture>
    {
        IntegrationTestsFixture fixture;

        public AlbumIntergationTests(IntegrationTestsFixture fixture)
        {
            this.fixture = fixture;     
        }

        [Fact]
        public async Task GetAlbums_Should_ReturnAlbumModels()
        {
            var requestUri = $"api/users/{fixture.user.Id}/albums";

            var response = await fixture.client.GetAsync(requestUri);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var albums = await response.DeserializeAsync<IEnumerable<AlbumModel>>();          

            albums.Count().Should().Be(2);
        }

        [Fact]
        public async Task GetAlbum_Should_ReturnAlbumModel()
        {
            var albumId = 1;
            var requestUri = $"api/albums/{albumId}";

            var response = await fixture.client.GetAsync(requestUri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var album = await response.DeserializeAsync<AlbumModel>();

            album.Id.Should().Be(albumId);
        }
    }    
}
