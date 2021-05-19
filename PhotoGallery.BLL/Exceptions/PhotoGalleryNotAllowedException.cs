namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryNotAllowedException : PhotoGalleryException
    {
        const string defaultMessage = "No permission to perform this operation";

        public PhotoGalleryNotAllowedException() : base(defaultMessage)
        {
        }

        public PhotoGalleryNotAllowedException(string message) : base(message)
        {
        }
    }
}