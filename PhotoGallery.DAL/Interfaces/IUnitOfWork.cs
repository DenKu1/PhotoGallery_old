using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using PhotoGallery.DAL.Entities;


namespace PhotoGallery.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<User> UserManager { get; }
        IUserRepository Users { get; }
        IGenericRepository<Album> Albums { get; }
        IGenericRepository<Comment> Comments { get; }
        IGenericRepository<Like> Likes { get; }
        IGenericRepository<Photo> Photos { get; }
        Task SaveAsync();
    }
}
