using System;
using System.Collections.Concurrent;

namespace Utils
{
    public class Cache<T>
    {
        private static  ConcurrentBag<T> _bag;

        Cache()
        {
            _bag = new ConcurrentBag<T>();
        }

        public static void Add(T obj)
        {
            _bag.Add(obj);
        }
    }
}
