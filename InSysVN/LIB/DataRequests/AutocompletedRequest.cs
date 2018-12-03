using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.DataRequests
{
    public class AutocompletedRequest : DataRequest
    {
        public string Filter { get; set; }
        public int Page { get; set; }
        public AutocompletedRequest(int id) : base(id)
        {
        }
        public AutocompletedRequest() : this(0)
        {

        }
    }
}
