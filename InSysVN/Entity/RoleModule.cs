namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleModule")]
    public partial class RoleModule
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ModuleId { get; set; }

        public bool? Add { get; set; }

        public bool? Edit { get; set; }

        public bool? View { get; set; }

        public bool? Delete { get; set; }

        public bool? Import { get; set; }

        public bool? Export { get; set; }

        public bool? Upload { get; set; }

        public bool? Publish { get; set; }

        public bool? Report { get; set; }

        public bool? Sync { get; set; }

        public bool? Accept { get; set; }

        public bool? Cancel { get; set; }

        public bool? Record { get; set; }
    }
}
