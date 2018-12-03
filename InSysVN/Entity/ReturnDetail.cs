namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReturnDetail")]
    public partial class ReturnDetail
    {
        public int Id { get; set; }

        public int ReturnId { get; set; }

        public long ProductId { get; set; }

        public int QuantityReturn { get; set; }

        public decimal PriceReturn { get; set; }
    }
}
