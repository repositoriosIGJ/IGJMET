using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts.Common
{
    public interface ICacheStore<T> where T: Entidad
    {
        IList<T> GetAll();

        T Get(Int32 id);

        void Add(T entity);

        void Remove(T entity);

        void SetCache(IList<T> entities);

        void Clear();
    }
}
