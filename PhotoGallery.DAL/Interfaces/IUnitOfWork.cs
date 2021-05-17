using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using PhotoGallery.DAL.Entities;


namespace PhotoGallery.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<User> UserManager { get; }
        IUserRepository Users { get; }
        IAlbumRepository Albums { get; }
        ICommentRepository Comments { get; }
        ILikeRepository Likes { get; }
        IPhotoRepository Photos { get; }        
        Task SaveAsync();
    }
}
