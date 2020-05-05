﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.DAL.Entities;

namespace PhotoGallery.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
    }
}