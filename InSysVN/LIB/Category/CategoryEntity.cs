using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static LIB.ExcelExtension;

namespace LIB
{
    [Table("Category")]
    public class CategoryEntity : CategoryEntityMore
    {
        #region Properties
        public int? Id { get; set; }
        [DisplayName("Tên danh mục")]
        [ExcelColumn(ColumnName: "B")]
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [DefaultValue(true)]
        public bool isActive { get; set; }
        [DisplayName("Hình ảnh")]
        public string ImagePath { get; set; }
        [DisplayName("Miêu tả")]
        public string Description { get; set; }
        #endregion
    }
    public class CategoryEntityMore
    {
    }
}
