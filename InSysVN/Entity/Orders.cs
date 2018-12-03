namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string OrderCode { get; set; }

        public long? CustomerId { get; set; }

        [StringLength(50)]
        public string CouponCode { get; set; }

        public decimal? CouponValue { get; set; }

        public int? CouponType { get; set; }

        public decimal? Voucher { get; set; }

        public decimal? Discount { get; set; }

        public double? PercentForPoint { get; set; }

        public decimal? MoneyOnePoint { get; set; }

        public double? PointUsed { get; set; }

        public decimal? ProductTotal { get; set; }

        public decimal? GrandTotal { get; set; }

        public DateTime? OrderDate { get; set; }

        public long? WarehouseId { get; set; }

        public decimal? PayCash { get; set; }

        public decimal? PayByCard { get; set; }

        public decimal? RefundMoney { get; set; }

        public string Note { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? PromotedDate { get; set; }

        public bool? isActive { get; set; }
    }
}
