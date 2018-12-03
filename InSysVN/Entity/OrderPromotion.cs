namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderPromotion")]
    public partial class OrderPromotion
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public long ProductId { get; set; }

        [StringLength(255)]
        public string ProductName { get; set; }

        [StringLength(20)]
        public string Barcode { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        public int? Quantity { get; set; }
    }
}
