using System;

namespace PhotoGallery.BLL.Exceptions
{
    public class PhotoGalleryException : Exception
    {
        public PhotoGalleryException() : base()
        {
        }

        public PhotoGalleryException(string message) : base(message)
        {
        }
    }
}
