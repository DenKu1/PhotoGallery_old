using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;
using PhotoGallery.IntegrationTests.Utilities;

namespace PhotoGallery.IntegrationTests.EndpointTests.Base
{
    public class IntegrationTestsFixture
    {
        public HttpClient client;
        public PhotoGalleryWebApplicationFactory factory;

        public UserWithTokenModel user;

        public IntegrationTestsFixture()
        {            
            factory = new PhotoGalleryWebApplicationFactory();
            client = factory.CreateClient();

            user = LoginAsync().Result;
            SetJwtTokenHeader(client, user.Token);
        }

        private async Task<UserWithTokenModel> LoginAsync()
        {
            var userData = new UserLoginModel 
            { 
                UserName = TestUser.UserName,
                Password = TestUser.Password
            };

            var response = await client.PostAsync("api/login", userData.Serealize());

            return await response.DeserializeAsync<UserWithTokenModel>();
        }

        private void SetJwtTokenHeader(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }    
}
