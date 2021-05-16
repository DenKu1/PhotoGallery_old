namespace PhotoGallery.DAL.Entities
{
    public class BaseEntity<TBase>
    {
        public TBase Id { get; set; }
    }
}
