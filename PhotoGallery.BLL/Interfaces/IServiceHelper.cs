namespace PhotoGallery.BLL.Interfaces
{
    public interface IServiceHelper
    {
        void ThrowPhotoGalleryNotFoundExceptionIfModelIsNull<TEntity>(TEntity entity) where TEntity : class;
        void ThrowPhotoGalleryNotAllowedExceptionIfDifferentId(int entityId1, int entityId2);
        void ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull<TEntity>(TEntity entity, string errorMessage) where TEntity : class;
        void ThrowPhotoGalleryFailedLoginExceptionIfModelIsNullOrPasswordInvalid<TEntity>(TEntity entity, bool isValidPassword) where TEntity : class;
    }
}