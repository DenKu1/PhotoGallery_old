using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PhotoGallery.DAL.Entities
{
    public class Photo
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }

        public DateTime Created { get; set; }

        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }

        public Photo()
        {
            Comments = new Collection<Comment>();
            Likes = new Collection<Like>();
        }
    }
}
