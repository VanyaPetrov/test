﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mocoding.AspNetCore.OdataApi.DataAccess;

using Mocoding.EasyDocDb;

namespace Mocoding.AspNetCore.OdataApi.EasyDocDB
{
    public class DocumentCollectionCrudRepository<TData> : ICrudRepository<TData>
        where TData : class, IEntity, new()
    {
        public DocumentCollectionCrudRepository(IDocumentCollection<TData> collection)
        {
            Collection = collection;
        }

        protected IDocumentCollection<TData> Collection { get; }

        public async Task ReInitRepository(List<TData> list)
        {
            Collection.Documents.Select(_ => _.Delete());
            foreach (var entity in list)
            {
                await Collection.New(entity).Save();
            }
        }

        public IQueryable<TData> GetAll() => Collection.Documents.Select(_ => _.Data).AsQueryable();

        public async Task<TData> AddOrUpdate(TData entity)
        {
            if (entity.Id.HasValue)
            {
                var item = GetById(entity.Id.Value);
                await item.SyncUpdate(entity);
            }
            else
            {
                entity.Id = Guid.NewGuid();
                await Collection.New(entity).Save();
            }

            return entity;
        }

        public async Task Delete(Guid id)
        {
            var item = GetById(id);
            if (item == null)
                throw new NullReferenceException("Can't find entity with id: " + id);
            await item.Delete();
        }

        private IDocument<TData> GetById(Guid id) => Collection.Documents.FirstOrDefault(_ => _.Data.Id == id);
    }
}
