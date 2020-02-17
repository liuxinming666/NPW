/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/22 19:28:53
 * 描  述：SimpleCRUD.Page
 * *********************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;

namespace  Dapper
{
    public partial class SimpleCRUD
    {
        /// <summary>
        /// <para>By default queries the table matching the class name</para>
        /// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
        /// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
        /// <para>orderby is a column or list of columns to order by ex: "lastname, age desc" - not required - default is by primary key</para>
        /// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
        /// <para>Supports transaction and command timeout</para>
        /// <para>Returns a list of entities that match where conditions</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>Gets a paged list of entities with optional exact match where conditions</returns>
        public static IEnumerable<T> GetListPaged<T>(this IDbConnection connection, int pageNumber, int rowsPerPage,string tableName,string fileds, string conditions, string orderby, object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (string.IsNullOrEmpty(_getPagedListSql))
                throw new Exception("GetListPage is not supported with the current SQL Dialect");

            if (pageNumber < 1)
                throw new Exception("Page must be greater than 0");

            if (string.IsNullOrEmpty(orderby))
            {
                throw new Exception("Page must have orderby");
            }

            var currenttype = typeof(T);

            //if (!fileds.ToLower().Contains("select"))
            //{
            //    fileds = "SELECT " + fileds;
            //}

            if (!conditions.ToLower().TrimStart().StartsWith("where")) {
                conditions = "where " + conditions;
            }

            var query = _getPagedListSql;
            query = query.Replace("{SelectColumns}", fileds);
            query = query.Replace("{TableName}", tableName);
            query = query.Replace("{PageNumber}", pageNumber.ToString());
            query = query.Replace("{RowsPerPage}", rowsPerPage.ToString());
            query = query.Replace("{OrderBy}", orderby);
            query = query.Replace("{WhereClause}", conditions);
            query = query.Replace("{Offset}", ((pageNumber - 1) * rowsPerPage).ToString());

			if (Debugger.IsAttached)
				Trace.WriteLine(String.Format("GetListPaged<{0}>: {1}", currenttype, query));



			//var beforDT = System.DateTime.Now;
			////耗时巨大的代码  
			//var data = connection.Query<T>(query, parameters, transaction, true, commandTimeout);
			//var afterDT = System.DateTime.Now;
			//var ts = afterDT.Subtract(beforDT);

			//if (Debugger.IsAttached)
			//	Trace.WriteLine(String.Format("GetListPaged总共花费{0}ms.", ts.TotalMilliseconds));

			//return data;

			return connection.Query<T>(query, parameters, transaction, true, commandTimeout);
		}

		

		/// <summary>
		/// <para>By default queries the table matching the class name</para>
		/// <para>-Table name can be overridden by adding an attribute on your class [Table("YourTableName")]</para>
		/// <para>Returns a number of records entity by a single id from table T</para>
		/// <para>Supports transaction and command timeout</para>
		/// <para>conditions is an SQL where clause ex: "where name='bob'" or "where age>=@Age" - not required </para>
		/// <para>parameters is an anonymous type to pass in named parameter values: new { Age = 15 }</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection"></param>
		/// <param name="conditions"></param>
		/// <param name="parameters"></param>
		/// <param name="transaction"></param>
		/// <param name="commandTimeout"></param>
		/// <returns>Returns a count of records.</returns>
		public static int RecordCount(this IDbConnection connection,string tableName, string conditions , object parameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            //var currenttype = typeof(T);
            //var name = GetTableName(currenttype);
            var sb = new StringBuilder();
            sb.Append("Select count(1)");
            sb.AppendFormat(" from {0}", tableName);

            if (!string.IsNullOrWhiteSpace(conditions))
            {
                if (!conditions.ToLower().Contains("where"))
                {
                    sb.Append(" where ");
                }
            }
            sb.Append(" " + conditions);

            if (Debugger.IsAttached)
                Trace.WriteLine(String.Format("RecordCount<{0}>: {1}", "", sb));


			//var beforDT = System.DateTime.Now;
			////耗时巨大的代码  
			//var row = connection.ExecuteScalar<int>(sb.ToString(), parameters, transaction, commandTimeout);
			//var afterDT = System.DateTime.Now;
			//var ts = afterDT.Subtract(beforDT);

			//if (Debugger.IsAttached)
			//	Trace.WriteLine(String.Format("RecordCount总共花费{0}ms.", ts.TotalMilliseconds));

			//return row;
			return connection.ExecuteScalar<int>(sb.ToString(), parameters, transaction, commandTimeout);

		}
    }
}
