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
        private readonly GalleryContext _context;

        public UserRepository(GalleryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
