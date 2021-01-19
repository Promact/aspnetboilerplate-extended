using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace CETAutomation.CacheStorage
{
  public  interface ITempFileCacheManager : ITransientDependency
    {
        void SetFile(string token, byte[] content);

        byte[] GetFile(string token);

        byte[] GetFileByName(string fileName);
    }
}
