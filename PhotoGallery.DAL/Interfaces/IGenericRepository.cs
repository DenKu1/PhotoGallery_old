﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PhotoGallery.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
    }
}
