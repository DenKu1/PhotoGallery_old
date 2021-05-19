using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PhotoGallery.DAL.EF;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        GalleryContext context;

        public UserRepository(GalleryContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
