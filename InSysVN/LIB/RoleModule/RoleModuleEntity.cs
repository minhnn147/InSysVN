using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LIB.RoleModule
{
    [Table("RoleModule")]
	public class RoleModuleEntity: BaseEntity<int>
	{
		#region Properties
		/// <summary>
		/// auto generator
		/// Gets or sets the RoleId value.
		/// 2017-08-07 17:02:24Z
		/// </summary>
		public int RoleId { get; set; }

		/// <summary>
		/// auto generator
		/// Gets or sets the ModuleId value.
		/// 2017-08-07 17:02:24Z
		/// </summary>
		public int ModuleId { get; set; }
        [XmlElement(IsNullable = false)]
        public string ModuleName { get; set; }
        public string ModuleDisplayName { get; set; }
        /// <summary>
        /// auto generator
        /// Gets or sets the Add value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Thêm")]
        public bool Add { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Edit value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Sửa")]
        public bool Edit { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the View value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Xem")]
        public bool View { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Delete value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Xóa")]
        public bool Delete { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Import value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Import")]
        public bool Import { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Export value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Export")]
        public bool Export { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Upload value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Upload")]
        public bool Upload { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Publish value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Publish")]
        public bool Publish { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Report value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Report")]
        public bool Report { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Print value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Sync")]
        public bool Sync { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Accept value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Accept")]
        public bool Accept { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Cancel value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// auto generator
        /// Gets or sets the Record value.
        /// 2017-08-07 17:02:24Z
        /// </summary>
        [DisplayName("Record")]
        public bool Record { get; set; }

		#endregion
        //extenstion

    }
}
