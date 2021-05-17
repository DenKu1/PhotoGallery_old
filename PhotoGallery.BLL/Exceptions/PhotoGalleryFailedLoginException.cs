namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryFailedLoginException : PhotoGalleryException
    {
        public PhotoGalleryFailedLoginException() : base()
        {
        }

        public PhotoGalleryFailedLoginException(string message) : base(message)
        {
        }
    }
}
