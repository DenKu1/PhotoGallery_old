using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly GalleryContext galleryContext;
        UserRepository userRepository;

        IGenericRepository<Album> albumRepository;
        IGenericRepository<Comment> commentRepository;
        IGenericRepository<Like> likeRepository;
        IGenericRepository<Photo> photoRepository;
        IGenericRepository<Tag> tagRepository;

        public UserManager<User> UserManager { get; }

        public IUserRepository Users => userRepository ??= new UserRepository(galleryContext);

        public IGenericRepository<Album> Albums => albumRepository ??= new GenericRepository<Album>(galleryContext);
        public IGenericRepository<Comment> Comments => commentRepository ??= new GenericRepository<Comment>(galleryContext);
        public IGenericRepository<Like> Likes => likeRepository ??= new GenericRepository<Like>(galleryContext);
        public IGenericRepository<Photo> Photos => photoRepository ??= new GenericRepository<Photo>(galleryContext);
        public IGenericRepository<Tag> Tags => tagRepository ??= new GenericRepository<Tag>(galleryContext);

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
