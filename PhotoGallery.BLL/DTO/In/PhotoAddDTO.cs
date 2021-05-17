namespace PhotoGallery.BLL.DTO.In
{
    public class PhotoAddDTO
    {
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
