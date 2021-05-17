namespace PhotoGallery.BLL.DTO.In
{
    public class CommentAddDTO
    {
        public int PhotoId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
