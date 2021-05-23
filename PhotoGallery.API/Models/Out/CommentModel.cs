namespace PhotoGallery.API.Models.Out
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}
