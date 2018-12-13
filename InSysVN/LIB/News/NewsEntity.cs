using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class NewsEntity
    {
        public long? ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string ImageTitle { get; set; }
        public int CreateBy { get; set; }

    }
}
