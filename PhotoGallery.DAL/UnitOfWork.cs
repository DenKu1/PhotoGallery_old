using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.DAL.Repositories;

namespace PhotoGallery.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        GalleryContext galleryContext;

        AlbumRepository albumRepository;
        CommentRepository commentRepository;
        LikeRepository likeRepository;
        PhotoRepository photoRepository;
        UserRepository userRepository;

        public UserManager<User> UserManager { get; }

        public IAlbumRepository Albums =>
            albumRepository ??= new AlbumRepository(galleryContext);

        public ICommentRepository Comments =>
            commentRepository ??= new CommentRepository(galleryContext);

        public ILikeRepository Likes =>
            likeRepository ??= new LikeRepository(galleryContext);

        public IPhotoRepository Photos =>
            photoRepository ??= new PhotoRepository(galleryContext);

        public IUserRepository Users =>
            userRepository ??= new UserRepository(galleryContext);

        public UnitOfWork(GalleryContext context, UserManager<User> userManager)
        {
            galleryContext = context;
            UserManager = userManager;
        }

        public async Task SaveAsync()
        {
            await galleryContext.SaveChangesAsync();
        }
    }
}
