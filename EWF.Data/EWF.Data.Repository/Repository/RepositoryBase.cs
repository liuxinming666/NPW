/* **********************************************
 * 版  本：1.0
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 16:56:19
 * 描  述：定义仓储模型中的数据标准操作接口
 * *********************************************/
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace EWF.Data.Repository
{
    [Obsolete("此操作数据库类已经废弃，如需使用可以使用Database里边的方法")]
    public class RepositoryBase : IRepositoryBase
    {
        #region 构造函数
        private IDatabase database;
        public RepositoryBase(IDatabase _database)
        {
            this.database = _database;
        }
        #endregion

        #region 事物提交
        public IRepositoryBase BeginTrans()
        {
            database.BeginTransaction();
            return this;
        }
        public void Commit()
        {
            database.Commit();
        }
        public void Rollback()
        {
            database.Rollback();
        }
        #endregion

        #region 对象实体：添加--修改--删除
        public int Insert<T>(T entity) where T : class
        {
            return database.Insert(entity);
        }
        public void Insert<T>(List<T> entitys) where T : class
        {
            database.Insert<T>(entitys);
        }

        public Tkey Insert<Tkey,T>(T entity) where T : class
        {
            return database.Insert<Tkey, T>(entity);
        }
        public void Insert<Tkey,T>(List<T> entitys) where T : class
        {
            database.Insert<Tkey, T>(entitys);
        }

        public bool Update<T>(T entity) where T : class
        {
            var rows = database.Update<T>(entity);
            return rows > 0 ? true : false;
        }
        public bool UpdateIgnoreNull<T>(T entity) where T : class
        {
            var rows = database.UpdateIgnoreNull<T>(entity);
            return rows > 0 ? true : false;
        }
        public void Update<T>(List<T> entitys) where T : class
        {
            database.Update<T>(entitys);
        }
        
        public int Delete<T>(T entity) where T : class
        {
            return database.Delete<T>(entity);
        }
        public int Delete<T>(object predicate) where T : class
        {
            return database.Delete<T>(predicate);
        }
        public int DeleteList<T>(object whereConditions) where T : class
        {
            return database.DeleteList<T>(whereConditions);
        }
        public int DeleteList<T>(string conditions, object parameters = null) where T : class
        {
            return database.DeleteList<T>(conditions, parameters);
        }

        #endregion

        #region 对象查询：实体--列表--分页
        public T Get<T>(object id) where T : class
        {
            return database.Get<T>(id);
        }
        public List<T> GetList<T>(object whereCondition) where T : class
        {
            return database.GetList<T>(whereCondition).ToList();
        }

        public List<T> GetList<T>(string conditions, object parameters = null) where T : class
        {
            return database.GetList<T>(conditions, parameters).ToList();
        }
        public List<T> SkipTake<T>(int skip, int take, string conditions, string orderby, object parameters = null) where T : class
        {
            return database.GetSkipTake<T>(skip, take, conditions, orderby, parameters).ToList();
        }

        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public Page<T> GetListPaged<T>(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null) where T : class
        {
            var totalItems = database.Count<T>(conditions);
            var items = database.GetPage<T>(pageIndex, pageSize, conditions, orderby, parameters).ToList();
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

        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public Page<T> GetListPaged<T>(int pageIndex, int pageSize, string tableName, string fileds, string conditions, string orderby, object parameters = null) where T : class
        {
            var totalItems = database.Count(tableName, conditions,parameters);
            var items = database.GetPage<T>(pageIndex, pageSize, tableName, fileds, conditions, orderby, parameters).ToList();
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

        #endregion

        #region 执行 SQL 语句
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public int ExecuteBySql(string strSql)
        {
            return database.ExecuteBySql(strSql);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public int ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            return database.ExecuteBySql(strSql, dbParameter);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public int ExecuteByProc(string procName)
        {
            return database.ExecuteByProc(procName);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public int ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            return database.ExecuteByProc(procName, dbParameter);
        }
        #endregion

        #region 数据源 查询
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public DataTable FindTable(string strSql)
        {
            return database.FindTable(strSql);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public DataTable FindTable(string strSql, DbParameter[] dbParameter)
        {
            return database.FindTable(strSql, dbParameter);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public DataTable FindTable(string strSql, Pagination pagination)
        {
            int total = pagination.records;
            var data = database.FindTable(strSql, pagination.sidx, pagination.sord.ToLower() == "asc" ? true : false, pagination.rows, pagination.page, out total);
            pagination.records = total;
            return data;
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public DataTable FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination)
        {
            int total = pagination.records;
            var data = database.FindTable(strSql, dbParameter, pagination.sidx, pagination.sord.ToLower() == "asc" ? true : false, pagination.rows, pagination.page, out total);
            pagination.records = total;
            return data;
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public object FindObject(string strSql)
        {
            return database.FindObject(strSql);
        }
        [Obsolete("此操作数据库类中方法已经废弃，请使用Database里边的方法")]
        public object FindObject(string strSql, DbParameter[] dbParameter)
        {
            return database.FindObject(strSql, dbParameter);
        }
        #endregion
    }
}
