using System;
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
        private readonly GalleryContext _context;

        private AlbumRepository _albumRepository;
        private CommentRepository _commentRepository;
        private LikeRepository _likeRepository;
        private PhotoRepository _photoRepository;
        private UserRepository _userRepository;

        public UnitOfWork(GalleryContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public UserManager<User> UserManager { get; }

        public IAlbumRepository Albums =>
            _albumRepository ??= new AlbumRepository(_context);

        public ICommentRepository Comments =>
            _commentRepository ??= new CommentRepository(_context);

        public ILikeRepository Likes =>
            _likeRepository ??= new LikeRepository(_context);

        public IPhotoRepository Photos =>
           _photoRepository ??= new PhotoRepository(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
