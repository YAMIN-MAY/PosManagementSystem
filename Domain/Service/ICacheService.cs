using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetTestSolution.Domain.Service
{
    public interface ICacheService
    {
        public T GetData<T>(string key);
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
        public object RemoveData(string key);
    }
}
