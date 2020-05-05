using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PhotoGallery.DAL.Entities
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public Album()
        {
            Photos = new Collection<Photo>();
        }
    }
}
