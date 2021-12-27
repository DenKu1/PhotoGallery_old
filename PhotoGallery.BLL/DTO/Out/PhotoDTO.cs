using System;

namespace PhotoGallery.BLL.DTO.Out
{
    public class PhotoDTO
    {
        public bool IsLiked { get; set; }
        public int Likes { get; set; }
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string[] Tags { get; set; }
        public DateTime Created { get; set; }
    }
}
