using System;

namespace PhotoGallery.BLL.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }
        //TODO: Add more exceptions
    }
}