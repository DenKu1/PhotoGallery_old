namespace PhotoGallery.DAL.Entities
{
    public class Like
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }
        public virtual Photo Photo { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
