using System;
using System.Collections.Generic;

using PhotoGallery.DAL.Entities.Base;

namespace PhotoGallery.DAL.Entities
{
    public class Album : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
