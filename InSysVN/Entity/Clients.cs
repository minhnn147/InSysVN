namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clients
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string UserName { get; set; }

        [StringLength(250)]
        public string PassWord { get; set; }

        [StringLength(250)]
        public string SecretName { get; set; }

        [StringLength(250)]
        public string SecretValue { get; set; }

        [StringLength(250)]
        public string SecretDisplay { get; set; }

        public int? LifeTime { get; set; }

        [StringLength(250)]
        public string Type { get; set; }

        public bool? isActive { get; set; }

        [StringLength(50)]
        public string ConnectTo { get; set; }
    }
}
