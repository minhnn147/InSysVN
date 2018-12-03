using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class EditInlineRequest<T>
    {
        public T Id { get; set; }
        public string ColumnName { get; set; }
        public string Value { get; set; }
        public byte Type { get; set; } //1: string, 2: Datetime, 3: Number, Bit
    }
}