﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NetTestSolution.Domain.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService(IDatabase database)
        {
            _database = database;
        }
        public T GetData<T>(string key)
        {
            var value = _database.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _database.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _database.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _database.KeyDelete(key);
            }
            return false;
        }

    }
}
