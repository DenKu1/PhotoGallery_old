using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PhotoGallery.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        //public override int Id { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public User()
        {
            Albums = new HashSet<Album>();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }
    }
}
