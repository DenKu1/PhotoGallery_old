using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;

namespace PhotoGallery.BLL.Helpers
{
    public class ServiceHelper : IServiceHelper
    {
        public void ThrowPhotoGalleryFailedLoginExceptionIfModelIsNullOrPasswordInvalid<TEntity>(TEntity entity, bool isValidPassword) where TEntity : class
        {
            if (entity == null || !isValidPassword)
            {
                throw new PhotoGalleryFailedLoginException();
            }
        }

        public void ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull<TEntity>(TEntity entity, string errorMessage) where TEntity : class
        {
            if (entity != null)
            {
                throw new PhotoGalleryFailedRegisterException(errorMessage);
            }
        }

        public void ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(int entityId1, int entityId2)
        {
            if (entityId1 != entityId2)
            {
                throw new PhotoGalleryNotAllowedException();
            }
        }

        public void ThrowPhotoGalleryNotFoundExceptionIfModelIsNull<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new PhotoGalleryNotFoundException(typeof(TEntity));
            }
        }
    }
}