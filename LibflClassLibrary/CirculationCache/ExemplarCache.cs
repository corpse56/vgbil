using LibflClassLibrary.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.CirculationCache
{
    public class ExemplarCache
    {
        MemoryCache cache_ = new MemoryCache("ExemplarCache");
        public ExemplarBase GetExemplar(int id, string fund)
        {
            ExemplarBase result;
            if (cache_.Contains(fund+id))
            {
                result = (ExemplarBase)cache_[fund+id];
            }
            else
            {
                result = ExemplarFactory.CreateExemplar(id, fund);
                AddExemplar(result);
            }
            return result;
        }
        private void AddExemplar(ExemplarBase exemplar)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(3);
            cache_.Add(exemplar.CahceKey, exemplar, policy);
        }
    }
}
//public static class Caching
//{
//    /// <summary>
//    /// A generic method for getting and setting objects to the memory cache.
//    /// </summary>
//    /// <typeparam name="T">The type of the object to be returned.</typeparam>
//    /// <param name="cacheItemName">The name to be used when storing this object in the cache.</param>
//    /// <param name="cacheTimeInMinutes">How long to cache this object for.</param>
//    /// <param name="objectSettingFunction">A parameterless function to call if the object isn't in the cache and you need to set it.</param>
//    /// <returns>An object of the type you asked for</returns>
//    public static T GetObjectFromCache<T>(string cacheItemName, int cacheTimeInMinutes, Func<T> objectSettingFunction)
//    {
//        ObjectCache cache = MemoryCache.Default;
//        var cachedObject = (T)cache[cacheItemName];
//        if (cachedObject == null)
//        {
//            CacheItemPolicy policy = new CacheItemPolicy();
//            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheTimeInMinutes);
//            cachedObject = objectSettingFunction();
//            cache.Set(cacheItemName, cachedObject, policy);
//        }
//        return cachedObject;
//    }