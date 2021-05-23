using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.API.Configuration.Automapper;

namespace PhotoGallery.Tests.ControllerTests.Base
{
    public class ControllerTestsBase
    {
        protected Mapper CreateMapperProfile()
        {
            var myProfile = new ModelProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        protected TController AddIdentity<TController>(TController controller, int userId) where TController : ControllerBase
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier.ToString(), userId.ToString())                                
            }, "TestAuthentication"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

    }
}
