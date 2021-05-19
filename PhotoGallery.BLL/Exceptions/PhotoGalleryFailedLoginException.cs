namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryFailedLoginException : PhotoGalleryException
    {
        const string defaultMessage = "Incorrect username or password";

        public PhotoGalleryFailedLoginException() : base(defaultMessage)
        {
        }

        public PhotoGalleryFailedLoginException(string message) : base(message)
        {
        }
    }
}
