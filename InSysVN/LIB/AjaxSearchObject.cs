using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;

namespace LIB.Model
{
    public class ObjPaging
    {
        public int TotalRow { get; set; }
    }
    public class AjaxSearchFilter
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Condition { get; set; }
        public string filterType { get; set; }
    }
    public class AjaxSearch
    {
        private dynamic Combine(dynamic item1, dynamic item2)
        {
            var dictionary1 = (IDictionary<string, object>)item1;
            var dictionary2 = (IDictionary<string, object>)item2;
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary

            foreach (var pair in dictionary1.Concat(dictionary2))
            {
                d[pair.Key] = pair.Value;
            }

            return result;
        }
        public object ViewData { get; set; }
        public int page { get; set; }
        public int PageSize { get; set; }
        public int TotalRow { get; set; }
        public int PageIndex { get; set; }
        public int StartItemIndex
        {
            get
            {
                return (PageIndex - 1) * PageSize + 1;
            }
        }
        public int EndItemIndex
        {
            get
            {
                return StartItemIndex + PageSize - 1;
            }
        }
        public int PageTotal
        {
            get
            {
                var _PageSize = PageSize > 0 ? PageSize : 1;
                return (int)Math.Ceiling((decimal)TotalRow / _PageSize);
            }
        }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public string MultiSort { get; set; }
        public string TxtSearch { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class AjaxSearchObject<T> : AjaxSearch
    {
        public AjaxSearchObject()
        {
            Lists = new List<T>();
            Filters = new List<AjaxSearchFilter>();
        }
        public IEnumerable<AjaxSearchFilter> Filters { get; set; }
        public IEnumerable<T> Lists { get; set; }

    }
}