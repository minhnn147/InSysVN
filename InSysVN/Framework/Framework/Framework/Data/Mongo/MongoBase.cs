using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Framework.Data.Entity;

namespace Framework.Data.Mongo
{
    public class MongoBase<TEntity, TKey> : IMongoBase<TEntity, TKey> where TEntity : BaseEntity<TKey>, new()
    {
        #region Instance
        public MongoBase(string schema)
        {
            _schema = schema;
            if (string.IsNullOrWhiteSpace(_schema))
                throw new Exception("No tenant.");
        }
        #endregion

        #region Properties
        private readonly string _schema;
        public string Schema { get { return _schema; } }

        private string _databaseName;
        public virtual string DatabaseName
        {
            get { return _databaseName ?? (_databaseName = DataHelper.GetMongoDbName() + "_" + Schema); }
            set { _databaseName = value; }
        }

        private MongoClientSettings _clientSettings;
        public virtual MongoClientSettings ClientSettings
        {
            get
            {
                if (_clientSettings != null) return _clientSettings;

                _clientSettings = new MongoClientSettings
                {
                    ConnectionMode = ConnectionMode.Automatic,
                    ConnectTimeout = new TimeSpan(0, 0, 0, 30),
                    SocketTimeout = new TimeSpan(0, 0, 0, 30),
                    GuidRepresentation = GuidRepresentation.CSharpLegacy,
                    MaxConnectionIdleTime = new TimeSpan(0, 0, 0, 30),
                    MaxConnectionLifeTime = new TimeSpan(0, 0, 0, 30),
                    MaxConnectionPoolSize = 1000,
                    MinConnectionPoolSize = 10,
                    Server = new MongoServerAddress(DataHelper.GetMongoDbHost(), DataHelper.GetMongoDbPort())
                };
                return _clientSettings;
            }
            set { _clientSettings = value; }
        }

        private MongoServer _server;
        public virtual MongoServer Server
        {
            get
            {
                if (_server != null) return _server;
                var mongoClient = new MongoClient(ClientSettings);
                _server = mongoClient.GetServer();
                return _server;
            }
            set { _server = value; }
        }

        private MongoDatabase _database;
        public virtual MongoDatabase Database
        {
            get
            {
                if (_database != null) return _database;

                if (!Server.DatabaseExists(DatabaseName))
                    Server.CreateDatabaseSettings(DatabaseName);
                _database = Server.GetDatabase(DatabaseName);
                return _database;
            }
            set { _database = value; }
        }

        private MongoCollection<TEntity> _collection;
        public virtual MongoCollection<TEntity> Collection
        {
            get
            {
                if (_collection != null) return _collection;

                string collectionName = typeof(TEntity).Name;
                if (!Database.CollectionExists(collectionName))
                    Database.CreateCollection(collectionName);
                _collection = Database.GetCollection<TEntity>(collectionName);
                return _collection;
            }
            set { _collection = value; }
        }
        #endregion

        #region Method
        public virtual bool Insert(ref TEntity item)
        {
            if (typeof(TKey) == typeof(ObjectId))
            {
                item.OId = (TKey)((object)ObjectId.GenerateNewId());
                var result = Collection.Insert(item);
                return result.Ok;
            }
            return false;
        }

        public virtual bool Insert(ref List<TEntity> items)
        {
            if (typeof(TKey) == typeof(ObjectId))
            {
                foreach (TEntity item in items)
                    item.OId = (TKey)((object)ObjectId.GenerateNewId());
                var result = Collection.InsertBatch(items);
                return result.Count(p => p.Ok) == items.Count;
            }
            return false;
        }

        public virtual bool Update(ref TEntity item)
        {
            var result = Collection.Save(item);
            return result.Ok;
        }

        public virtual bool Update(ref List<TEntity> items)
        {
            var result = items.Select(item => Collection.Save(item)).ToList();
            return result.Count(p => p.Ok) == items.Count;
        }

        public virtual bool Delete(ref TEntity item)
        {
            var result = Collection.Remove(Query<TEntity>.EQ(p => p.OId, item.OId));
            return result.Ok;
        }

        public virtual bool Delete(ref List<TEntity> items)
        {
            var result = items.Select(item => Collection.Remove(Query<TEntity>.EQ(p => p.OId, item.OId))).ToList();
            return result.Count(p => p.Ok) == items.Count;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> condition)
        {
            List<TEntity> items = GetQuery(condition).ToList();
            var result = items.Select(item => Collection.Remove(Query<TEntity>.EQ(p => p.OId, item.OId))).ToList();
            return result.Count(p => p.Ok) == items.Count;
        }

        public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> condition = null, Expression<Func<TEntity, bool>> orderBy = null, OrderByDirection orderDir = OrderByDirection.Ascending, int take = 0, int skip = 0)
        {
            var query = Collection.AsQueryable();
            if (condition != null) query = query.Where(condition);
            if (take > 0 && skip >= 0) query = query.Take(take).Skip(skip);
            if (orderBy != null)
            {
                if (orderDir == OrderByDirection.Ascending)
                    query = query.OrderBy(orderBy);
                else if (orderDir == OrderByDirection.Descending)
                    query = query.OrderByDescending(orderBy);
            }
            return query;
        }

        public virtual TEntity GetByPk(TKey key)
        {
            return Collection.FindOneById(BsonValue.Create(key));
        }

        public virtual TEntity GetByCondition(Expression<Func<TEntity, bool>> condition)
        {
            return GetQuery(condition).FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetQuery().ToList();
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> condition)
        {
            return GetQuery(condition).ToList();
        }

        public virtual long GetCount()
        {
            return Collection.Count();
        }

        public virtual long GetCount(Expression<Func<TEntity, bool>> condition)
        {
            return GetQuery(condition).LongCount();
        }

        public virtual void Disconnect()
        {
            if (Server.State != MongoServerState.Disconnected)
                Server.Disconnect();
        }

        public virtual void Reconnect()
        {
            if (Server.State == MongoServerState.Disconnected)
                Server.Reconnect();
        }

        #endregion
    }
}
