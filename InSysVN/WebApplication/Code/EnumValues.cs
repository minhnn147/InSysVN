using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication.Code
{
    public class EnumValues
    {
        public enum Sphere //Lĩnh vực
        {
            [Description("Kinh tế - xã hội")]
            Socioeconomic = 1,
            [Description("Giao thông đường bộ")]
            Road = 2,
            [Description("Giao thông đường sắt")]
            Railway = 3,
            [Description("Giao thông đường thủy nội địa")]
            InlandWaterway = 4,
            [Description("Hàng Hải")]
            Marine = 5,
            [Description("Hàng không")]
            Air = 6,
            [Description("Giao thông đô thị")]
            UrbanTransport = 7,
            [Description("Giao thông nông thôn")]
            RuralTransport = 8,
        }
    }
}