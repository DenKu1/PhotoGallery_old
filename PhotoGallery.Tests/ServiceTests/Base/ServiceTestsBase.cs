using AutoMapper;

using PhotoGallery.BLL.Configuration.Automapper;

namespace PhotoGallery.Tests.ServiceTests.Base
{
    public class ServiceTestsBase
    {
        protected Mapper CreateMapperProfile()
        {
            var myProfile = new DtoProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}
