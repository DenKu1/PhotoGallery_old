namespace PhotoGallery.BLL.DTO.In
{
    public class AlbumUpdateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
