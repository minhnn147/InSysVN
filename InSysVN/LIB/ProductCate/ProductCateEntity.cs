using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ProductCate
{
    [Table("ProductCate")]
    public class ProductCateEntity
    {
        public int ProductId { get; set; }
        public int CateId { get; set; }
    }
}
