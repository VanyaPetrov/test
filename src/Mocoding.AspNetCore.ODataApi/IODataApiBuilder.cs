﻿using System;
using System.Collections.Generic;
using Mocoding.AspNetCore.OdataApi.DataAccess;

namespace Mocoding.AspNetCore.OdataApi
{
    public interface IODataApiBuilder
    {
        IODataApiBuilder UseFactory(IRepositoryFactory factory);
        IEnumerable<Type> GetUsedTypes();
        IODataApiBuilder AddResource<T>(string customRoute = null)
            where T : class, IEntity, new();
    }
}