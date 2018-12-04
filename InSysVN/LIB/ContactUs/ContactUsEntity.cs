using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class ContactUsEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public bool IsReply { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ReplyDate { get; set; }
    }
}
