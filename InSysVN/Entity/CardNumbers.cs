namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CardNumbers
    {
        public int Id { get; set; }

        [StringLength(16)]
        public string CardNumber { get; set; }
    }
}
