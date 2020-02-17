/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-05 11:39:58                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： STATIONINFO_SCHEMERepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Page;

namespace EWF.Repository
{
    public class STATIONINFO_SCHEMERepository : FileRepository<STATIONINFO_SCHEME>, ISTATIONINFO_SCHEMERepository
    {
		public STATIONINFO_SCHEMERepository(IOptionsSnapshot<DbOption> options) : base(options) { }

        public Page<STATIONINFO_SCHEME> GetPageData(int pageIndex, int pageSize, string name)
        {
            var where = "";
            var orderby = "ID";
            if (!name.IsEmpty())
            {
                where = " where STNM like '%" + name + "%'";
            }

            var page = GetListPaged(pageIndex, pageSize, where, orderby);
            return page;
        }
        public IEnumerable<STATIONINFO_SCHEME> GetModelBySTCD(string STCD)
        {
            string sql = string.Format("SELECT * FROM TBL_STATIONINFO_SCHEME where STCD='{0}'", STCD);
            using (var db = database.Connection)
            {
                return db.Query<STATIONINFO_SCHEME>(sql);
            }
        }
    }
}