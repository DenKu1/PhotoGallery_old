namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryNotAllowedException : PhotoGalleryException
    {
        public PhotoGalleryNotAllowedException() : base()
        {
        }

        public PhotoGalleryNotAllowedException(string message) : base(message)
        {
        }
    }
}