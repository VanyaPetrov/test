﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Mocoding.AspNetCore.OdataApi.DataAccess;
using Mocoding.AspNetCore.OdataApi.EasyDocDB.Helpers;
using Mocoding.EasyDocDb;
using Mocoding.EasyDocDb.FileSystem;
using Mocoding.EasyDocDb.Json;

namespace Mocoding.AspNetCore.OdataApi.EasyDocDB
{
    public class Factory : IRepositoryFactory
    {
        private readonly string _conn;
        private readonly IRepository _repository;

        public Factory(string conn)
        {
            _conn = conn;
            _repository = new EmbeddedRepository(new JsonSerializer());
        }

        public ICrudRepository<T> Create<T>(string name)
            where T : class, IEntity, new()
        {
            var t = typeof(T);
            var attribute = t.GetCustomAttribute(typeof(ReadOptimizedAttribute));
            return attribute != null ?
                new DocumentCrudRepository<T>(_repository.Init<List<T>>(Path.Combine(_conn, name + ".json")).Result)
             : new DocumentCollectionCrudRepository<T>(_repository.InitCollection<T>(Path.Combine(_conn, name)).Result) as ICrudRepository<T>;
        }
    }
}
