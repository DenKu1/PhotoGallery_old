using AutoMapper;
using PhotoGallery.BLL.DTO;
using PhotoGallery.DAL.Entities;

namespace PhotoGallery.BLL.Configs
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Album, AlbumDTO>();
            CreateMap<AlbumAddDTO, Album>();

            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentAddDTO, Comment>();

            CreateMap<Photo, PhotoDTO>().ForMember(p => p.Likes, opt => opt.Ignore());
            CreateMap<PhotoAddDTO, Photo>();

            CreateMap<User, UserDTO>();
        }
    }
}