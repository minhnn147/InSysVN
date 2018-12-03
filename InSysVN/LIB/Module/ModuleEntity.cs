using System;
//using Framework.Extensions;
using Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LIB.Model
{
    [Table("Module")]
    public class ModuleEntity : BaseEntity<int>
    {
        #region Properties
        /// <summary>
        /// auto generator
        /// Gets or sets the Name value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        /// 
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }
        public int Parent { get; set; }
        public int Sorting { get; set; }
        public bool isShow { get; set; }
        #endregion
    }
    [Table("Module")]
    public class ModuleTreeViewModel : ModuleEntity
    {
        public string NameTreeView { get; set; }
    }
}
