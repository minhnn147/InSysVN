using Dapper;
using DapperExtensions.Mapper;
using Framework.Helper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApplication.Code
{
    public class CustomPluralizedMapper<T> : ClassMapper<T> where T : class
    //public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T> where T : class
    {
        public override void Table(string tableName)
        {
            //if (tableName.Equals("Person", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    TableName = "Persons";
            //}
            //base.Table(tableName);
            string table = EntityType.GetAttributeValue((TableAttribute dna) => dna.Name);
            //Properties.
            base.Table(table);
            base.AutoMap();
        }
    }

    //public class CustomClassMapper<T> : ClassMapper<T>, SqlMapper.ITypeMap where T : class
    //{
    //    public ConstructorInfo FindConstructor(string[] names, Type[] types)
    //    {
    //        return this.EntityType.GetConstructor(Type.EmptyTypes);
    //    }

    //    public ConstructorInfo FindExplicitConstructor()
    //    {
    //        return null;
    //    }

    //    public SqlMapper.IMemberMap GetConstructorParameter(
    //                        ConstructorInfo constructor, string columnName)
    //    {
    //        return null;
    //    }

    //    public SqlMapper.IMemberMap GetMember(string columnName)
    //    {
    //        if (Properties.SingleOrDefault(x =>
    //                        x.ColumnName == columnName) != null)
    //        {
    //            return new CustomMemberMap(
    //                        this.EntityType.GetMember(Properties.First(x =>
    //                            x.ColumnName == columnName).Name).Single(),
    //                        columnName);
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //}

    public class CustomMemberMap : SqlMapper.IMemberMap
    {
        private readonly MemberInfo _member;
        private readonly string _columnName;

        public CustomMemberMap(MemberInfo member, string columnName)
        {
            this._member = member;
            this._columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }

        public FieldInfo Field
        {
            get { return _member as FieldInfo; }
        }

        public Type MemberType
        {
            get
            {
                switch (_member.MemberType)
                {
                    case MemberTypes.Field:
                        return ((FieldInfo)_member).FieldType;
                    case MemberTypes.Property:
                        return ((PropertyInfo)_member).PropertyType;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public ParameterInfo Parameter
        {
            get { return null; }
        }

        public PropertyInfo Property
        {
            get { return _member as PropertyInfo; }
        }
    }

    //MyCustomClassMapper
}