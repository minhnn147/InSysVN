using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Enum
{
    public class EnumSystem
    {
        public enum TemplateType
        {
            [Description("All")]
            Type0 = 0,
            [Description("Hệ thống")]
            Type1 = 1,
        }
        public enum EmailQueueStatus
        {
            [Description("Upload")]
            Status1 = 0,
            [Description("Xử lý")]
            Status2 = 2,
            [Description("Thành công")]
            Status3 = 3,
            [Description("Lỗi")]
            Status4 = 4,
        }

        public enum EmailType
        {
            NoReply = 1,
            Customer = 2,
            Order = 3,
        }
    }
}
