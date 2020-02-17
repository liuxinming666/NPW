/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 16:56:19
 * 描  述：Dapper通用操作接口，
 * 并暴漏出Connection链接对象，可以实现所有的Dapper的操作（需要手动写SQL）
 * *********************************************/
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace EWF.Data
{
    public interface IDatabase
    {
        #region 公用：属性，事务
        IDbConnection Connection { get; }
        bool HasActiveTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        #endregion

        #region 操作：Insert，Update，Delete
        int Insert<T>(T entity) where T : class;
        TKey Insert<TKey, T>(T entity) where T : class;
        void Insert<T>(IEnumerable<T> entities) where T : class;
        void Insert<TKey, T>(IEnumerable<T> entities) where T : class;

        int Update<T>(T entity) where T : class;
        int UpdateIgnoreNull<T>(T entity) where T : class;
        void Update<T>(IEnumerable<T> entities) where T : class;

        int Delete<T>(T entity) where T : class;
        int Delete<T>(object id) where T : class;
        int DeleteList<T>(object whereConditions) where T : class;
        int DeleteList<T>(string conditions, object parameters = null) where T : class;
        #endregion

        #region 查询：Single，List，Top
        T Get<T>(object id) where T : class;
        T GetSingle<T>(object whereCondition) where T : class;
        T GetSingle<T>(string conditions, object parameters = null) where T : class;
        IEnumerable<T> GetList<T>(object whereCondition) where T : class;
        IEnumerable<T> GetList<T>(string conditions, object parameters = null) where T : class;
        IEnumerable<T> GetSkipTake<T>(int skip, int take, string conditions, string orderby, object parameters = null) where T : class;
		
		#endregion
		
		#region 查询：Page， Count
		IEnumerable<T> GetPage<T>(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null) where T : class;
		IEnumerable<T> GetPage<T>(int pageNumber, int rowsPerPage, string tableName, string fileds, string conditions, string orderby, object parameters = null) where T : class;

		Page<T> GetListPaged<T>(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null) where T : class;

		Page<T> GetListPaged<T>(int pageIndex, int pageSize, string tableName, string fileds, string conditions, string orderby, object parameters = null) where T : class;

		/// <summary>
		/// 存储过程分页-表名支持关联查询
		/// </summary>
		Page<T> GetPagination_Join<T>(int pageIndex, int pageSize, string tableName, string fileds, string conditions, string orderby) where T : class;


		int Count<T>(string conditions, object parameters = null) where T : class;
		int Count<T>(object whereConditions) where T : class;
		int Count(string tableName, string conditions, object parameters = null);
		#endregion

		#region 执行：Sql，Proc，DataTable
		int ExecuteBySql(string strSql);
        int ExecuteBySql(string strSql, DbParameter[] dbParameter);
        int ExecuteByProc(string procName);
        int ExecuteByProc(string procName, DbParameter[] dbParameter);
        object FindObject(string strSql);
        object FindObject(string strSql, DbParameter[] dbParameter);

        DataTable FindTable(string strSql);
        DataTable FindTable(string strSql, DbParameter[] dbParameter);
        DataTable FindTable(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex, out int total);
        DataTable FindTable(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex, out int total);
        #endregion
    }
}
