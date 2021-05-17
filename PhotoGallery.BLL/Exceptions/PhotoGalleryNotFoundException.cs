namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryNotFoundException : PhotoGalleryException
    {
        public PhotoGalleryNotFoundException() : base()
        {
        }

        public PhotoGalleryNotFoundException(string message) : base(message)
        {
        }
    }
}