using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using LibCore.Data.Entity;

namespace LibCore.Data.Mongo
{
    public interface IMongoBase<TEntity, in TKey> where TEntity : BaseEntity<TKey>, new()
    {
        bool Insert(ref TEntity item);
        bool Insert(ref List<TEntity> items);

        bool Update(ref TEntity item);
        bool Update(ref List<TEntity> items);

        bool Delete(ref TEntity item);
        bool Delete(ref List<TEntity> items);
        bool Delete(Expression<Func<TEntity, bool>> condition);

        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> condition = null,
            Expression<Func<TEntity, bool>> orderBy = null, OrderByDirection orderDir = OrderByDirection.Ascending,
            int take = 0, int skip = 0);

        TEntity GetByPk(TKey key);
        TEntity GetByCondition(Expression<Func<TEntity, bool>> condition);

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> condition);

        Int64 GetCount();
        Int64 GetCount(Expression<Func<TEntity, bool>> condition);

        void Disconnect();
        void Reconnect();
    }
}
