using AutoMapper;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public abstract class Service : IService
    {
        protected readonly IUnitOfWork _unit;
        protected readonly IMapper _mp;

        public Service(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
            _mp = mapper;
        }

        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _unit.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
