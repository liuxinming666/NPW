/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 16:56:19
 * 描  述：Dapper通用接口
 * *********************************************/
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;



namespace EWF.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class,new()
    {
        #region 构造函数
        public DbOption dbOption;
        public IDatabase database;
        
        //public RepositoryBase(IOptionsSnapshot<DbOption> _options)
        //{
        //    dbOption = _options.Get("Default_Option");
        //    if (dbOption == null)
        //    {
        //        throw new ArgumentNullException(nameof(DbOption));
        //    }
        //    database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
        //}
        #endregion

        #region 对象实体：添加--修改--删除
        public int Insert(T entity)
        {
            return database.Insert<T>(entity);
        }
        public void Insert(List<T> entitys)
        {
            database.Insert<T>(entitys);
        }

        public Tkey Insert<Tkey>(T entity)
        {
            return database.Insert<Tkey, T>(entity);
        }
        public void Insert<Tkey>(List<T> entitys)
        {
            database.Insert<Tkey, T>(entitys);
        }

        public bool Update(T entity)
        {
            var rows = database.Update<T>(entity);
            return rows > 0 ? true : false;
        }
        public bool UpdateIgnoreNull(T entity)
        {
            var rows = database.UpdateIgnoreNull<T>(entity);
            return rows > 0 ? true : false;
        }
        public void Update(List<T> entitys)
        {
            database.Update<T>(entitys);
        }
        
        public int Delete(T entity)
        {
            return database.Delete<T>(entity);
        }
        public int Delete(object id)
        {
            return database.Delete<T>(id);
        }
        public int DeleteList(object whereConditions)
        {
            return database.DeleteList<T>(whereConditions);
        }
        public int DeleteList(string conditions, object parameters = null)
        {
            return database.DeleteList<T>(conditions, parameters);
        }

        #endregion

        #region 对象查询：实体--列表--分页
        public T Get(object id)
        {
            return database.Get<T>(id);
        }
        public List<T> GetList(object whereCondition)
        {
            return database.GetList<T>(whereCondition).ToList();
        }
        public List<T> GetList(string conditions, object parameters = null)
        {
            return database.GetList<T>(conditions, parameters).ToList();
        }
        
        public List<T> SkipTake(int skip, int take, string conditions, string orderby, object parameters = null)
        {
            return database.GetSkipTake<T>(skip, take, conditions, orderby, parameters).ToList();
        }
        public Page<T> GetListPaged(int pageIndex, int pageSize, string conditions, string orderby, object parameters = null)
        {
            var totalItems = database.Count<T>(conditions, parameters);
            var items = database.GetPage<T>(pageIndex, pageSize, conditions, orderby, parameters);
            var lastPageItems = totalItems % pageSize;
            var totalPages = totalItems / pageSize;

            return new Page<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = lastPageItems == 0 ? totalPages : totalPages + 1,
                Items = items.ToList(),
                TotalItems = totalItems
            };
        }

        public int Count(object whereConditions)
        {
            return database.Count<T>(whereConditions);
        }
        public int Count(string conditions, object parameters = null)
        {
            return database.Count<T>(conditions, parameters);
        }
        #endregion
    }
}
