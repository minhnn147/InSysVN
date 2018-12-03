using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Framework.Data.Entity
{
    [BsonSerializer]
    public abstract class BaseEntity<TKey>
    {
        [BsonId]
        public TKey OId
        {
            get;
            set;
        }
    }
}
