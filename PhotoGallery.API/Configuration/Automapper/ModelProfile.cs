using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;

namespace PhotoGallery.API.Configuration.Automapper
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<AlbumAddModel, AlbumAddDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["userId"]));
            CreateMap<AlbumUpdateModel, AlbumUpdateDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["userId"]))
                .ForMember(x => x.Id, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["albumId"]));
            CreateMap<CommentAddModel, CommentAddDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["userId"]))
                .ForMember(x => x.PhotoId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["photoId"]));
            CreateMap<PhotoAddModel, PhotoAddDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["userId"]))
                .ForMember(x => x.AlbumId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["albumId"]));
            CreateMap<PhotoUpdateModel, PhotoUpdateDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["userId"]))
                .ForMember(x => x.Id, opt => opt.MapFrom((_, _, _, context) => (int)context.Items["photoId"]));
            CreateMap<UserLoginModel, UserLoginDTO>();
            CreateMap<UserRegisterModel, UserRegisterDTO>();

            CreateMap<AlbumDTO, AlbumModel>();
            CreateMap<CommentDTO, CommentModel>();
            CreateMap<PhotoDTO, PhotoModel>();
            CreateMap<UserDTO, UserModel>();
            CreateMap<UserWithTokenDTO, UserWithTokenModel>();
        }
    }
}
