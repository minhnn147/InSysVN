namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [Key]
        [Column(Order = 0)]
        public long Id { get; set; }

        [StringLength(255)]
        public string ProductCategory { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Barcode { get; set; }

        public string MainImage { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int InventoryNumber { get; set; }

        [StringLength(50)]
        public string ComputeUnit { get; set; }

        public decimal? Price { get; set; }

        public decimal? SellPrice { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public int? Allowcated { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long WarehouseId { get; set; }

        public DateTime? DateSync { get; set; }
    }
}
