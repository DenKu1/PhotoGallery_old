using System.Collections.Generic;
using PhotoGallery.DAL.Entities.Base;

namespace PhotoGallery.DAL.Entities
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
