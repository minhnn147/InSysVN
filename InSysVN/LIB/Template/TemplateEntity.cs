using Framework.Data.Attributes;
using LIB.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper.ExtendedMethods;

namespace LIB.Model
{
    [Table("Template")]
    public class TemplateEntity : BaseEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        [Display(Name = "Tên Template")]
        public string Name { get; set; }
        [MaxLength(500)]
        [Display(Name = "Link Template")]
        public string Url { get; set; }
        public int Type { get; set; }
    }
}
