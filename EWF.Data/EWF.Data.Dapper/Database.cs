/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 17:09:19
 * 描  述：Database
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using EWF.Util;
using EWF.Data;
using EWF.Util.Page;
using System.Diagnostics;

namespace EWF.Data.Dapper
{
    /// <summary>
    /// Database的Dapper实现类
    /// </summary>
    public class Database : IDatabase, IDisposable
    {
        #region 构造函数
        public Database(string connectionString, DatabaseType _dbType = DatabaseType.SqlServer)
        {
            ConnectionString = connectionString;
            dbType = _dbType;
            _connection = ConnectionFactory.CreateConnection(ConnectionString, _dbType);
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                }

                Connection.Close();
            }
        }
        #endregion

        #region 属    性
        private DatabaseType dbType;
        private IDbConnection _connection;

        protected string ConnectionString;
        public IDbConnection Connection
        {
            get
            {
                if (_connection.ConnectionString == "")
                {
                    _connection = ConnectionFactory.CreateConnection(ConnectionString, dbType);
                }

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        protected IDbTransaction _transaction;
        public bool HasActiveTransaction
        {
            get
            {
                return _transaction != null;
            }
        }
       
        #endregion

        #region 事    务
        public void BeginTransaction()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            _transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }
        #endregion

        #region 操作：Insert，Update，Delete
        public virtual int Insert<T>(T entity) where T : class
        {
            var result = Connection.Insert<T>(entity, _transaction);
            return result.ToInt() ;
        }
        public virtual TKey Insert<TKey, T>(T entity) where T : class
        {
            var result = Connection.Insert<TKey,T>(entity, _transaction);
            return result;
        }
        public virtual void Insert<T>(IEnumerable<T> entities) where T : class
        {
            //如果有活动的事务，直接用现有的事务，如果没有事务，则创建事务，批量执行
            if (HasActiveTransaction)
            {
                foreach (var item in entities)
                {
                    Connection.Insert<T>(item, _transaction);
                }
            }
            else
            {
                BeginTransaction();
                try
                {
                    foreach (var item in entities)
                    {
                        Connection.Insert<T>(item, _transaction);
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    if (HasActiveTransaction)
                    {
                        Rollback();
                    }
                   
                    throw ex;
                }
            }
        }

        public virtual void Insert<TKey, T>(IEnumerable<T> entities) where T : class
        {
            //如果有活动的事务，直接用现有的事务，如果没有事务，则创建事务，批量执行
            if (HasActiveTransaction)
            {
                foreach (var item in entities)
                {
                    Connection.Insert<TKey, T>(item, _transaction);
                }
            }
            else
            {
                BeginTransaction();
                try
                {
                    foreach (var item in entities)
                    {
                        Connection.Insert<TKey, T>(item, _transaction);
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    if (HasActiveTransaction)
                    {
                        Rollback();
                    }

                    throw ex;
                }
            }
        }

        public virtual int  Update<T>(T entity) where T : class
        {
            return Connection.Update<T>(entity, _transaction);
        }
        public virtual int UpdateIgnoreNull<T>(T entity) where T : class
        {
            return Connection.UpdateIgnoreNull<T>(entity, _transaction);
        }
        public virtual void Update<T>(IEnumerable<T> entities) where T : class
        {
            //如果有活动的事务，直接用现有的事务，如果没有事务，则创建事务，批量执行
            if (HasActiveTransaction)
            {
                foreach (var item in entities)
                {
                    Connection.Update<T>(item, _transaction, null);
                }
            }
            else
            {
                BeginTransaction();
                try
                {
                    foreach (var item in entities)
                    {
                        Connection.Update<T>(item, _transaction, null);
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    if (HasActiveTransaction)
                    {
                        Rollback();
                    }

                    throw ex;
                }
            }
        }
        
        public virtual int Delete<T>(T entity) where T : class
        {
            return Connection.Delete<T>(entity, _transaction, null);
        }
        public virtual int Delete<T>(object id) where T : class
        {
            return Connection.Delete<T>(id, _transaction, null);
        }

        public virtual int DeleteList<T>(object whereConditions) where T : class
        {
            return Connection.DeleteList<T>(whereConditions, _transaction, null);
        }
        public virtual int DeleteList<T>(string conditions, object parameters = null) where T : class
        {
            return Connection.DeleteList<T>(conditions, parameters, _transaction, null);
        }
        #endregion

        #region 查询：Single，List，Page，Skip，Take，Count
        public virtual T Get<T>(object id) where T : class
        {
            return Connection.Get<T>(id, _transaction, null);
        }
        public virtual T GetSingle<T>(object whereCondition) where T : class
        {
            return Connection.GetList<T>(whereCondition, _transaction).FirstOrDefault();
        }
        public virtual T GetSingle<T>(string conditions, object parameters = null) where T : class
        {
            return Connection.GetList<T>(conditions, parameters, _transaction).FirstOrDefault();
        }

        public virtual IEnumerable<T> GetList<T>(object whereCondition) where T : class
        {
            return Connection.GetList<T>(whereCondition, _transaction);
        }
        public virtual IEnumerable<T> GetList<T>(string conditions, object parameters = null) where T : class
        {
            return Connection.GetList<T>(conditions, parameters, _transaction);
        }
        public virtual IEnumerable<T> GetSkipTake<T>(int skip, int take, string conditions, string orderby, object parameters = null) where T : class
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetPage<T>(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null) where T : class
        {
            return Connection.GetListPaged<T>(pageNumber, rowsPerPage, conditions, orderby, parameters, _transaction);
        }

        public virtual IEnumerable<T> GetPage<T>(int pageNumber, int rowsPerPage, string tableName, string fileds, string conditions, string orderby, object parameters = null) where T : class
        {
            return Connection.GetListPaged<T>(pageNumber, rowsPerPage, tableName, fileds, conditions, orderby, parameters, _transaction);
        }

        public Page<T> GetListPaged<T>(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null) where T : class
        {
            var totalItems = Count<T>(conditions);
            var items = GetPage<T>(pageIndex, pageSize, conditions, orderby, parameters).ToList();
            var lastPageItems = totalItems % pageSize;
            var totalPages = totalItems / pageSize;

            return new Page<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = items,
                TotalPages = lastPageItems == 0 ? totalPages : totalPages + 1
            };
        }

        public Page<T> GetListPaged<T>(int pageIndex, int pageSize, string tableName, string fileds, string conditions, string orderby, object parameters = null) where T : class
        {
			var items = GetPage<T>(pageIndex, pageSize, tableName, fileds, conditions, orderby, parameters).ToList();
			var totalItems = Count(tableName, fileds, conditions, parameters);
          
            var lastPageItems = totalItems % pageSize;
            var totalPages = totalItems / pageSize;

            return new Page<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems,//lastPageItems == 0 ? totalPages : totalPages + 1,
                Items = items,
                TotalPages = lastPageItems == 0 ? totalPages : totalPages + 1//totalPages
            };
        }


		/// <summary>
		/// 存储过程分页-表名支持关联查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="tableName"></param>
		/// <param name="fileds"></param>
		/// <param name="conditions"></param>
		/// <param name="orderby"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public Page<T> GetPagination_Join<T>(int pageIndex, int pageSize, string tableName, string fileds, string conditions, string orderby) where T : class
		{
			var parameters = new DynamicParameters();
			parameters.Add("SqlWhere", conditions, dbType: DbType.String);
			parameters.Add("PageSize", pageSize, dbType: DbType.Int32);
			parameters.Add("PageIndex", pageIndex, dbType: DbType.Int32);
			parameters.Add("SqlTable",tableName, dbType: DbType.String);
			parameters.Add("SqlField", fileds, dbType: DbType.String);
			parameters.Add("SqlOrder", orderby, dbType: DbType.String);
			parameters.Add("Count", dbType: DbType.Int32, direction: ParameterDirection.Output);

		

			var beforDT = System.DateTime.Now;

			//耗时巨大的代码  
			var items = Connection.Query<T>("Pagination_Join", parameters, commandTimeout: 60, commandType: CommandType.StoredProcedure);

			//var total = Convert.ToInt32(new DbHelper(Connection).ExecuteScalar(CommandType.Text, sql, null));

			var afterDT = System.DateTime.Now;
			var ts = afterDT.Subtract(beforDT);

			if (Debugger.IsAttached)
				Trace.WriteLine(String.Format("RecordCount总共花费{0}ms.", ts.TotalMilliseconds));


			var totalItems = parameters.Get<int>("Count");
			
			var lastPageItems = totalItems % pageSize;
			var totalPages = totalItems / pageSize;

			return new Page<T>
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				TotalItems = totalItems,//lastPageItems == 0 ? totalPages : totalPages + 1,
				Items = items.ToList(),
				TotalPages = lastPageItems == 0 ? totalPages : totalPages + 1//totalPages
			};
		}


		public virtual int Count<T>(object whereConditions) where T : class
		{
			return Connection.RecordCount<T>(whereConditions, _transaction, null);
		}
		public virtual int Count<T>(string conditions, object parameters = null) where T : class
        {
            return Connection.RecordCount<T>(conditions, parameters, _transaction, null);
        }

		public virtual int Count(string tableName,string conditions, object parameters = null)
        {
            return Connection.RecordCount(tableName,conditions, parameters, _transaction, null);
        }
       
		private  int Count(string tableName,string fileds, string conditions, object parameters = null)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("select {0} ",fileds);
			sb.AppendFormat(" from {0} ", tableName);

			if (!string.IsNullOrWhiteSpace(conditions))
			{
				if (!conditions.ToLower().TrimStart().StartsWith("where"))
				{
					sb.Append(" where ");
				}
				sb.Append(" " + conditions);
			}
			


			var sql = string.Format("Select Count(1) From ({0}) As t", sb.ToString());





			//var beforDT = System.DateTime.Now;
			//耗时巨大的代码  
			var total = Connection.ExecuteScalar<int>(sql, parameters);
			//var afterDT = System.DateTime.Now;
			//var ts = afterDT.Subtract(beforDT);

			//if (Debugger.IsAttached)
			//	Trace.WriteLine(String.Format("RecordCount总共花费{0}ms.", ts.TotalMilliseconds));

			//return total;


			return Connection.ExecuteScalar<int>(sql, parameters);
		}
		
		#endregion

		#region 执行: SQL 语句
		public int ExecuteBySql(string strSql)
        {
            if (_transaction == null)
            {
                using (var connection = Connection)
                {
                    return connection.Execute(strSql);
                }
            }
            else
            {
                _transaction.Connection.Execute(strSql, null, _transaction);
                return 0;

            }
        }
        public int ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (_transaction == null)
            {
                using (var connection = Connection)
                {
                    return connection.Execute(strSql, dbParameter);
                }
            }
            else
            {
                _transaction.Connection.Execute(strSql, dbParameter, _transaction);
                return 0;

            }
        }
        public int ExecuteByProc(string procName)
        {
            if (_transaction == null)
            {
                using (var connection = Connection)
                {
                    return connection.Execute(procName);
                }
            }
            else
            {
                _transaction.Connection.Execute(procName, null, _transaction);
                return 0;

            }
        }
        public int ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            if (_transaction == null)
            {
                using (var connection = Connection)
                {
                    return connection.Execute(procName, dbParameter);
                }
            }
            else
            {
                _transaction.Connection.Execute(procName, dbParameter, _transaction);
                return 0;

            }
        }
        #endregion

        #region 查询: DataTable
        public object FindObject(string strSql)
        {
            return FindObject(strSql, null);
        }
        public object FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = Connection)
            {
                return new DbHelper(dbConnection).ExecuteScalar(CommandType.Text, strSql, dbParameter);
            }
        }

        public DataTable FindTable(string strSql)
        {
            return FindTable(strSql, null);
        }
        public DataTable FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = Connection)
            {
                var IDataReader = new DbHelper(dbConnection).ExecuteReader(CommandType.Text, strSql, dbParameter);
                return ConvertExtension.IDataReaderToDataTable(IDataReader);
            }
        }

        public DataTable FindTable(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex, out int total)
        {
            return FindTable(strSql, null, orderField, isAsc, pageSize, pageIndex, out total);
        }
        public DataTable FindTable(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex, out int total)
        {
            using (var dbConnection = Connection)
            {
                StringBuilder sb = new StringBuilder();
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                string OrderBy = "";

                if (!string.IsNullOrEmpty(orderField))
                {
                    if (orderField.ToUpper().IndexOf("ASC") + orderField.ToUpper().IndexOf("DESC") > 0)
                    {
                        OrderBy = "Order By " + orderField;
                    }
                    else
                    {
                        OrderBy = "Order By " + orderField + " " + (isAsc ? "ASC" : "DESC");
                    }
                }
                else
                {
                    OrderBy = "order by (select 0)";
                }
                sb.Append("Select * From (Select ROW_NUMBER() Over (" + OrderBy + ")");
                sb.Append(" As rowNum, * From (" + strSql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
                total = Convert.ToInt32(new DbHelper(dbConnection).ExecuteScalar(CommandType.Text, "Select Count(1) From (" + strSql + ") As t", dbParameter));
                var IDataReader = new DbHelper(dbConnection).ExecuteReader(CommandType.Text, sb.ToString(), dbParameter);
                DataTable resultTable = ConvertExtension.IDataReaderToDataTable(IDataReader);
                resultTable.Columns.Remove("rowNum");
                return resultTable;
            }
        }
        #endregion
    }
}
