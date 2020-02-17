/* **********************************************
 * 版  本：1.0
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 16:56:19
 * 描  述：定义仓储模型中的泛型数据标准操作接口
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
    /// 定义仓储模型中的泛型数据标准操作接口
    /// </summary>
    public interface IRepositoryBase<T> where T : class,new()
    {
        int Insert(T entity);
        void Insert(List<T> entitys);

        TKey Insert<TKey>(T entity);
        void Insert<TKey>(List<T> entities);

        bool Update(T entity);
        bool UpdateIgnoreNull(T entity);
        void Update(List<T> entitys);

        int Delete(T entity);
        int Delete(object id);
        int DeleteList(object whereConditions);
        int DeleteList(string conditions, object parameters = null) ;

        T Get(object id);
        List<T> GetList(object whereCondition);
        List<T> GetList(string conditions, object parameters = null);

        int Count(object whereConditions);
        int Count(string conditions, object parameters = null);

        /// <summary>
        /// 部分查询
        /// </summary>
        /// <param name="skip">跳过的行数</param>
        /// <param name="take">获取的行数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">分页排序</param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        List<T> SkipTake(int skip, int take, string conditions, string orderby, object parameters = null);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">当前页码:从1开始</param>
        /// <param name="pageSize">返回的行数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">分页排序</param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        Page<T> GetListPaged(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null);
    }
}
