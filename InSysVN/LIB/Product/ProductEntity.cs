using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static LIB.ExcelExtension;

namespace LIB.Product
{
    [Table("Products")]
    public class ProductEntity : ProductEntityMore
    {
        #region Properties
        public long? Id { get; set; }
        [ExcelColumn("A")]
        [DisplayName("Danh mục")]
        public int ProductCategory { get; set; }
        [ExcelColumn("B")]
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [XmlElement(IsNullable = false)]
        [ExcelColumn("D")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        [ExcelColumn("E")]
        [DisplayName("Đơn vị tính")]
        public string ComputeUnit { get; set; }
        [ExcelColumn("F")]
        [DisplayName("Giá")]
        public decimal? Price { get; set; }
        public string Image { get; set; }
        [ExcelColumn("H")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        public bool Status { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Specifications { get; set; }
        public bool isDel { get; set; }
        public string CategoryName { get; set; }
        #endregion
    }
    public class ProductEntityMore
    {
        public bool AllowEdit{ get; set; }
    }
    public class ProductId_QuantityModel
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductsChartTop10Model
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductFilter
    {
        public string WarehouseIds { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class ProductResult
    {
        public bool Status { get; set; }
        public List<Product> Data { get; set; }
        public string Message { get; set; }
        public int TotalRows { get; set; }
    }
    public class Product
    {
        public string Barcode { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategory { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ProductName { get; set; }
        public string ComputeUnit { get; set; }
        public decimal SellPrice { get; set; }
        public string MainImage { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
