using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Product
{
    public class ProductReport
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategory { get; set; }
        public int QuantitySell { get; set; }
        public int QuantityPromotion { get; set; }
        public int QuantityReturn { get; set; }
        public int QuantityInventory { get; set; }
        public decimal TotalPriceSell { get; set; }
        public decimal TotalPriceReturn { get; set; }

    }
}
