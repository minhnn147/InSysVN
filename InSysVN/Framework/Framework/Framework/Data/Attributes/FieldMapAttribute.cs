using System;

namespace Framework.Data.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldMapAttribute : System.Attribute
    {
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapAttribute"/> class.
        /// </summary>
        public FieldMapAttribute()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("");
        }
    }
    [Flags]
    public enum CrudFieldType
    {
        Create = 0x1,
        Read = 0x2,
        Update = 0x4,
        Delete = 0x8,
        DontUse = 0x10,
        All = 0x20
    }

    public class CrudField : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudField"/> class.
        /// CrudField(false) : Create only
        /// CrudField : Create and Update
        /// </summary>
        /// <param name="update">if set to <c>true</c> [update].</param>
        /// <param name="create">if set to <c>true</c> [create].</param>
        public CrudField(bool update = true, bool create = true)
        {
            if (create)
                UsedFor = UsedFor | CrudFieldType.Create;
            if (update)
                UsedFor = UsedFor | CrudFieldType.Update;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudField"/> class.
        /// CrudField(CrudFieldType.Create|CrudFieldType.Update)
        /// </summary>
        /// <param name="userFor">The user for.</param>
        public CrudField(CrudFieldType userFor)
        {
            UsedFor = userFor;
        }
        public CrudFieldType UsedFor { get; set; }
    }
}
