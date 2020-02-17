/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： SYS_VIDEO                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository.SysManage;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Page;

namespace EWF.Repository
{
    public class SYS_VIDEOMANAGERepository : DefaultRepository<SYS_VIDEOMANAGE>, ISYS_VIDEOMANAGERepository
    {
        public SYS_VIDEOMANAGERepository(IOptionsSnapshot<DbOption> options) : base(options) { }
		public Page<SYS_VIDEOMANAGE> GetCameraData(int pageIndex, int pageSize, string name, int type, string addvcd)
        {
            //var condition = "";
            //var orderby = "STCD";
            //if (!name.IsEmpty())
            //{
            //    condition = "where (SName like '%" + name + "%')";
            //}
            //var page = GetListPaged(pageIndex, pageSize, condition, orderby);
            //return page;

            //获取时段内累积雨量值
            string sqlInnerText = "SELECT [ID],[STCD],[SNAME],[SIP],[SPORT],[USERNAME],[PASSWORD],[PASSWAY],[LINETYPE],[SEBLONG] ,[REMARKS],[MAINPAGE] FROM [EW_NPW].[dbo].[TBL_SYS_VIDEOMANAGE] where 1=1";
            //参数列表
            var sqlParams = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                sqlInnerText += " and (SName like '%" + name + "%')";
                sqlParams.Add("SName", name);
            }
            var tableName = "(" + sqlInnerText + ") AA inner join  EW_NPW.dbo.ST_STBPRP_V BB on AA.STCD=BB.STCD and BB.TYPE=" + type + " and BB.ADDVCD='" + addvcd + "'";
            var flied = "AA.[ID],AA.[STCD],AA.[SNAME],AA.[SIP],AA.[SPORT],AA.[USERNAME],AA.[PASSWORD],AA.[PASSWAY],AA.[LINETYPE],AA.[SEBLONG] ,AA.[REMARKS],AA.[MAINPAGE]";
            var where = "1=1";
            var orderby = "AA.STCD DESC";

            //var db = new RepositoryBase(database);
            var page = database.GetListPaged<SYS_VIDEOMANAGE>(pageIndex, pageSize, tableName, flied, where, orderby, sqlParams);

            return page;
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraNameList(string SNAME)
        {
            return database.GetList<SYS_VIDEOMANAGE>(new {  SNAME = SNAME });
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraList(string STCD, string SNAME,string SIP, string SPORT)
        {
            return database.GetList<SYS_VIDEOMANAGE>(new { STCD = STCD, SNAME=SNAME, SIP=SIP, SPORT=SPORT });
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraListBySTCD(string STCD)
        {
            return database.GetList<SYS_VIDEOMANAGE>(new { STCD = STCD });
        }
        public SYS_VIDEOMANAGE GetCameraByID(int ID)
        {
            return database.Get<SYS_VIDEOMANAGE>(ID);
        }
    }
}