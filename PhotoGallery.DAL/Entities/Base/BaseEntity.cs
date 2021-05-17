namespace PhotoGallery.DAL.Entities.Base
{
    public abstract class BaseEntity<TBase>
    {
        public TBase Id { get; set; }
    }
}
