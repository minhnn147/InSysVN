using System;
using Framework.Data.Attributes;
//using Framework.Extensions;

namespace LIB
{
    public class BaseEntity<TKey>
    {
        ///// <summary>
        ///// Gets or sets the identifier.
        ///// </summary>
        ///// <value>The identifier.</value>
        //[CrudField(CrudFieldType.Update | CrudFieldType.Delete)] // Just use in proceduce update/delete
        //public TKey Id { get; set; }
        ///// <summary>
        ///// Gets or sets the created date.
        ///// </summary>
        ///// <value>The created date.</value>
        //[CrudField(UsedFor = CrudFieldType.DontUse)] // Dont use this properties in proceduce insert/update/delete
        //public DateTime CreatedDate { get; set; }
        ///// <summary>
        ///// Gets or sets the modified date.
        ///// </summary>
        ///// <value>The modified date.</value>
        //[CrudField(UsedFor = CrudFieldType.DontUse)] // Dont use this properties in proceduce insert/update/delete
        //public DateTime ModifiedDate { get; set; }
    }
}
