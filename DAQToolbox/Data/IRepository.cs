using System;
using System.Collections.Generic;

namespace DAQToolbox.Data
{
    public interface IRepository<T> where T: BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        void Insert(T entity);
        void Update(T updatedEntity);
        void Delete(Guid id);
    }
}
