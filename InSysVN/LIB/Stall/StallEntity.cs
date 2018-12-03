using System;
using Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Framework.Data.Attributes;

namespace LIB.Stall
{
    [Table("Stall")]
    public partial class StallEntity : BaseEntity<int>
    {
        #region Properties
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public string Name { get; set; }
        #endregion
    }
}
