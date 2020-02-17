/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2018/11/20 18:23:44
 * 描  述：OracleHelper
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

namespace EWF.Data.Dapper
{
    public class OracleDatabase : Database
    {
        public OracleDatabase(string connectionString) :
           base(connectionString, DatabaseType.Oracle)
        {
            SimpleCRUD.SetDialectForOracle();
        }
        
        #region 操作：Insert，Update，Delete
        public override int Insert<T>(T entity) 
        {
            var result = Connection.Insert_Oracle<T>(entity, _transaction);
            return result.ToInt();
        }
        public override void Insert<T>(IEnumerable<T> entities) 
        {
            //如果有活动的事务，直接用现有的事务，如果没有事务，则创建事务，批量执行
            if (HasActiveTransaction)
            {
                foreach (var item in entities)
                {
                    Connection.Insert_Oracle<T>(item, _transaction);
                }
            }
            else
            {
                BeginTransaction();
                try
                {
                    foreach (var item in entities)
                    {
                        Connection.Insert_Oracle<T>(item, _transaction);
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

        public override int Update<T>(T entity) 
        {
            return Connection.Update_Oracle<T>(entity, _transaction);
        }
        public override void Update<T>(IEnumerable<T> entities) 
        {
            //如果有活动的事务，直接用现有的事务，如果没有事务，则创建事务，批量执行
            if (HasActiveTransaction)
            {
                foreach (var item in entities)
                {
                    Connection.Update_Oracle<T>(item, _transaction, null);
                }
            }
            else
            {
                BeginTransaction();
                try
                {
                    foreach (var item in entities)
                    {
                        Connection.Update_Oracle<T>(item, _transaction, null);
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

        public override int Delete<T>(T entity) 
        {
            return Connection.Delete_Oracle<T>(entity, _transaction, null);
        }
        public override int Delete<T>(object id) 
        {
            return Connection.Delete_Oracle<T>(id, _transaction, null);
        }

        public override int DeleteList<T>(object whereConditions) 
        {
            return Connection.DeleteList_Oracle<T>(whereConditions, _transaction, null);
        }
        public override int DeleteList<T>(string conditions, object parameters = null) 
        {
            return Connection.DeleteList<T>(conditions, parameters, _transaction, null);
        }
        #endregion

        #region 查询：Single，List，Page，Skip，Take，Count
        public override T Get<T>(object id) 
        {
            return Connection.Get<T>(id, _transaction, null);
        }
        public override T GetSingle<T>(object whereCondition) 
        {
            return Connection.GetList_Oracle<T>(whereCondition, _transaction).FirstOrDefault();
        }
        public override T GetSingle<T>(string conditions, object parameters = null)
        {
            return Connection.GetList<T>(conditions, parameters, _transaction).FirstOrDefault();
        }

        public override IEnumerable<T> GetList<T>(object whereCondition) 
        {
            return Connection.GetList_Oracle<T>(whereCondition, _transaction);
        }
        public override IEnumerable<T> GetList<T>(string conditions, object parameters = null)
        {
            return Connection.GetList<T>(conditions, parameters, _transaction);
        }
        public override IEnumerable<T> GetSkipTake<T>(int skip, int take, string conditions, string orderby, object parameters = null) 
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> GetPage<T>(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null) 
        {
            return Connection.GetListPaged<T>(pageNumber, rowsPerPage, conditions, orderby, parameters, _transaction);
        }
        public override int Count<T>(string conditions, object parameters = null) 
        {
            return Connection.RecordCount<T>(conditions, parameters, _transaction, null);
        }
        public override int Count<T>(object whereConditions) 
        {
            return Connection.RecordCount_Oracle<T>(whereConditions, _transaction, null);
        }

        #endregion
    }
}
