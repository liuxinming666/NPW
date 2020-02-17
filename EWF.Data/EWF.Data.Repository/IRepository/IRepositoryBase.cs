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
    /// <summary>
    /// 定义仓储模型中的数据标准操作接口
    /// </summary>
    public interface IRepositoryBase
    {
        IRepositoryBase BeginTrans();
        void Commit();
        void Rollback();

        #region 操作：Insert，Update，Delete
        int Insert<T>(T entity) where T : class;
        void Insert<T>(List<T> entitys) where T : class;

        TKey Insert<TKey,T>(T entity) where T : class;
        void Insert<TKey,T>(List<T> entities) where T : class;

        bool Update<T>(T entity) where T : class;
        bool UpdateIgnoreNull<T>(T entity) where T : class;
        void Update<T>(List<T> entitys) where T : class;

        int Delete<T>(T entity) where T : class;
        int Delete<T>(object id) where T : class;
        int DeleteList<T>(object whereConditions) where T : class;
        int DeleteList<T>(string conditions, object parameters = null) where T : class;
        #endregion

        #region 查询：Single，List，Page，Top，Count
        T Get<T>(object id) where T : class;
        List<T> GetList<T>(object whereCondition) where T : class;
        List<T> GetList<T>(string conditions, object parameters = null) where T : class;
        
        /// <summary>
        /// 部分查询
        /// </summary>
        /// <param name="skip">跳过的行数</param>
        /// <param name="take">获取的行数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">分页排序</param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        List<T> SkipTake<T>(int skip, int take, string conditions, string orderby, object parameters = null) where T : class;
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">返回的行数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">分页排序</param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        Page<T> GetListPaged<T>(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null) where T : class;
        
        #endregion

        #region 执行：Sql，Proc，DataTable
        int ExecuteBySql(string strSql);
        int ExecuteBySql(string strSql, params DbParameter[] dbParameter);
        int ExecuteByProc(string procName);
        int ExecuteByProc(string procName, params DbParameter[] dbParameter);
        object FindObject(string strSql);
        object FindObject(string strSql, DbParameter[] dbParameter);

        DataTable FindTable(string strSql);
        DataTable FindTable(string strSql, DbParameter[] dbParameter);
        DataTable FindTable(string strSql, Pagination pagination);
        DataTable FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination);

        #endregion
    }
}
