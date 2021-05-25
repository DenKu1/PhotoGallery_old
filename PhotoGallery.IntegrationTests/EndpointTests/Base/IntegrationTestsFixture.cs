using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using PhotoGallery.IntegrationTests.Utilities;

namespace PhotoGallery.IntegrationTests.EndpointTests.Base
{
    public class IntegrationTestsFixture
    {
        public HttpClient client;
        public PhotoGalleryWebApplicationFactory factory;

        public IntegrationTestsFixture()
        {            
            factory = new PhotoGalleryWebApplicationFactory();
            client = BuildAnonymousClient(factory);
        }

        private HttpClient BuildAnonymousClient(PhotoGalleryWebApplicationFactory factory)
        {
            return factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(
                services => services.AddMvc(
                    options =>
                    {
                        options.Filters.Add(new AllowAnonymousFilter());
                        options.Filters.Add(new FakeUserFilter());
                    }))).CreateClient();
        }
    }    
}
