/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/23 22:21:22
 * 描  述：分页查询返回的通用对象
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Page
{
    /// <summary>
    /// 分页查询返回的通用对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T>
    {
        /// <summary>
        ///     The current page number contained in this page of result set
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     The number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     The total number of pages in the full result set
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        ///     The total number of records in the full result set
        /// </summary>
        public int TotalItems { get; set; }
        
        /// <summary>
        ///     The actual records on this page
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        ///     User property to hold anything.
        /// </summary>
        public object Context { get; set; }
    }
}
