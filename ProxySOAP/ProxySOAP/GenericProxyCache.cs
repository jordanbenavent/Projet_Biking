using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Json;
using System.Diagnostics;

namespace ProxySOAP
{
    public class GenericProxyCache<T>
    {
        ObjectCache cache = MemoryCache.Default;
        DateTimeOffset dt_default = ObjectCache.InfiniteAbsoluteExpiration;

        public T Get(string CacheItemName)
        {
            if (!cache.Contains(CacheItemName))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = dt_default;
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                cache.Set(CacheItemName, obj, policy);
            }
            return (T)cache.Get(CacheItemName);
        }

        public T Get(string CacheItemName, double dt_seconds)
        {
            if (!cache.Contains(CacheItemName))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(dt_seconds);
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                cache.Set(CacheItemName, obj, policy);
            }
            return (T)cache.Get(CacheItemName);
        }

        public T Get(string CacheItemName, DateTimeOffset dt)
        {
            if (!cache.Contains(CacheItemName))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = dt;
                T obj = (T)Activator.CreateInstance(typeof(T), CacheItemName);
                cache.Set(CacheItemName, obj, policy);
            }
            return (T)cache.Get(CacheItemName);
        }
    }
}
