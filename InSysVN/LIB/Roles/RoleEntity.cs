using System;
//using Framework.Extensions;
using Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LIB.Model
{
    [Table("Roles")]
    public class RoleEntity : BaseEntity<int>
    {
        #region Properties
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// auto generator
        /// Gets or sets the Name value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool isShow { get; set; }


        #endregion
    }
}
