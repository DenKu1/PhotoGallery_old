using System;
using System.Linq;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.DAL.Entities;

namespace PhotoGallery.BLL.Configuration.Automapper
{
    public class DtoProfile : Profile
    {
        public DtoProfile()
        {
            CreateMap<AlbumAddDTO, Album>()
                .ForMember(x => x.Created, opt => opt.MapFrom((_, _, _, context) => (DateTime)context.Items["creationTime"]))
                .ForMember(x => x.Updated, opt => opt.MapFrom((_, _, _, context) => (DateTime)context.Items["creationTime"]));            
            CreateMap<CommentAddDTO, Comment>();            
            CreateMap<PhotoAddDTO, Photo>()
                .ForMember(x => x.Created, opt => opt.MapFrom((_, _, _, context) => (DateTime)context.Items["creationTime"]));

            CreateMap<Comment, CommentDTO>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.UserName));            
            CreateMap<Album, AlbumDTO>();            
            CreateMap<Photo, PhotoDTO>()
                .ForMember(x => x.Likes, opt => opt.MapFrom(x => x.Likes.Count()))
                .ForMember(x => x.IsLiked, opt => opt.MapFrom((photo, photoDTO, _, context) =>
                    photo.Likes.Any(like => like.UserId == (int)context.Items["userId"])));            
            CreateMap<User, UserDTO>()
                .ForMember(x => x.Roles, opt => opt.MapFrom((_, _, _, context) => (string[])context.Items["roles"]));
            CreateMap<User, UserWithTokenDTO>()
                .ForMember(x => x.Roles, opt => opt.MapFrom((_, _, _, context) => (string[])context.Items["roles"]))
                .ForMember(x => x.Token, opt => opt.MapFrom((_, _, _, context) => (string)context.Items["token"]));
        }
    }
}