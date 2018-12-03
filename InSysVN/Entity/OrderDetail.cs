namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        [StringLength(255)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string Barcode { get; set; }

        public int? Quantity { get; set; }

        public decimal? SellPrice { get; set; }

        public decimal? Discount { get; set; }

        public bool? isActive { get; set; }
    }
}
