using System;
using System.Collections.Generic;

using PhotoGallery.DAL.Entities.Base;

namespace PhotoGallery.DAL.Entities
{
    public class Photo : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public DateTime Created { get; set; }

        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
