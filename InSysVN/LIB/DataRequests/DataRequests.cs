using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.DataRequests
{
    public class DataRequest
    {
        public DataRequest(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
    public class DeleteRequest : DataRequest
    {
        public DeleteRequest(int id) : base(id)
        {
        }
    }
    public class DeleteSelectMessage<TKey>
    {
        public int Id { get; set; }
        public TKey[] Ids { get; set; }
    }

    public class SelectIdsMessage<TKey> where TKey : IConvertible
    {
        public TKey[] Ids { get; set; }
    }
    public class PagingRequest
    {
        public int objId { get; set; }
        public string Filter { get; set; }
        public string Keyword { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string FirstCharCode { get; set; }
        public byte Type { get; set; }
    }

    public class StatusRequest
    {
        public int Id { get; set; }
        public byte value { get; set; }
        public byte userId { get; set; }
        public string Note { get; set; }

    }
}
