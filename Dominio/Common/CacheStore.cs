using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Contracts.Common;
using Domain.Entities;

namespace Domain.Common
{
    public class CacheStore<T> : ICacheStore<T> where T : Entidad
    {
        private IList<T> _cache;

        private Entidad _sync;

        public CacheStore()
        {
            _cache = new List<T>();
            _sync = new Entidad();
        }

        public virtual IList<T> GetAll()
        {
            lock (_sync)
            {
                return _cache;
            }
        }

        public virtual T Get(Int32 id)
        {
            lock (_sync)
            {
                return _cache.SingleOrDefault(x => x.Id == id);
            }
        }

        public virtual void Add(T entity)
        {
            lock (_sync)
            {
                if (!_cache.Contains(entity))
                    _cache.Add(entity);
            }
        }

        public virtual void Remove(T entity)
        {
            lock (_sync)
            {
                if (_cache.Contains(entity))
                    _cache.Remove(entity);
            }
        }

        public virtual void SetCache(IList<T> entities)
        {
            lock (_sync)
            {
                _cache.Clear();
                ((List<T>)_cache).AddRange(entities);
            }
        }

        public virtual void Clear()
        {
            lock (_sync)
            {
                _cache.Clear();
            }
        }
    }
}
