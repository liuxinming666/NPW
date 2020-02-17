/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：RiverRepository
 * *********************************************/
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Util;
using EWF.Entity.Models;
using EWF.Util.Page;

namespace EWF.Repository.Oracle
{
    public class RiverRepository 
    {
        private IDatabase database;
        public RiverRepository(IOptionsSnapshot<DbOption> options)
        {
           var dbOption = options.Get("RTDB_Opion");
            if (dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            database = DapperFactory.CreateDapper(dbOption.DbType, dbOption.ConnectionString);
        }

        public IEnumerable<dynamic> GetLatestRiver(string startDate, string endDate, string addvcd, string type)
        {
            throw new NotImplementedException();
        }

        public dynamic GetMutiRiverData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history)
        {
            throw new NotImplementedException();
        }

        public dynamic GetMutiSedData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history)
        {
            throw new NotImplementedException();
        }

        public dynamic GetMutliStationZQS(string stcds, string stime, string etime)
        {
            throw new NotImplementedException();
        }

        public Page<dynamic> GetReadData(int pageIndex,int pageSize,string STNM)
        {
           
            return null;

            //var list = database.GetList<ST_RIVER_R>(new { STCD = "41203700", Z = 3.44 });


            //var list = database.GetList<ST_RIVER_R>(sql);

            //return list;
        }
 
        /// <summary>
        /// 查询水情信息
        /// </summary>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        public Page<dynamic> GetRiverData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type)
        {
            return null;
        }

        public IEnumerable<dynamic> GetRiverData(string stcd, string startDate, string endDate, int hourFilter = 8)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetRiverData(string unit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetRiverDataByMultiStcds(string stcds, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetRvavData(string stcd, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public int InsertTest()
        {
            var db = new RepositoryBase(database);

            db.BeginTrans();
           
            db.Commit();
            return 0;
        }


    }
}
