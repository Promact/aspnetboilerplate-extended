using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.CacheStorage
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        public const string TempFileCacheName = "TempFileCacheName";

        private readonly ICacheManager _cacheManager;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void SetFile(string token, byte[] content)
        {
            _cacheManager.GetCache(TempFileCacheName).Set(token, content, new TimeSpan(0, 0, 1, 0)); // expire time is 1 min by default
        }

        public byte[] GetFile(string token)
        {
            return _cacheManager.GetCache(TempFileCacheName).Get(token, ep => ep) as byte[];
        }
        public byte[] GetFileByName(string fileName)
        {
            return _cacheManager.GetCache(TempFileCacheName).Get(fileName, ep => ep) as byte[];
        }
    }
}
