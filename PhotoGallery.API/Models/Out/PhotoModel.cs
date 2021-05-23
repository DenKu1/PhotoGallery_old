using System;

namespace PhotoGallery.API.Models.Out
{
    public class PhotoModel
    {
        public bool IsLiked { get; set; }
        public int Likes { get; set; }
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
    }
}
