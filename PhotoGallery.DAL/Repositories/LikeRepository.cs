using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.DAL.Repositories
{
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(GalleryContext context) : base(context)
        {
        }
    }
}
