using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestFlightControl
{
    class StubedMemoryCache : IMemoryCache
    {
        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            throw new NotImplementedException();
        }

        public static object Get(this IMemoryCache cache, object key)
        {

        }

        public static TItem Set<TItem>(this IMemoryCache cache, object key, TItem value)
        {

        }
    }
}