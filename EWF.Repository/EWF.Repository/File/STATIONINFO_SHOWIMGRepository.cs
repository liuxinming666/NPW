/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-05 11:39:58                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Repository                                  
*│　类    名： STATIONINFO_SHOWIMGRepository                                      
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
    public class STATIONINFO_SHOWIMGRepository : FileRepository<STATIONINFO_SHOWIMG>, ISTATIONINFO_SHOWIMGRepository
    {
        public STATIONINFO_SHOWIMGRepository(IOptionsSnapshot<DbOption> options) : base(options) { }
		public IEnumerable<STATIONINFO_SHOWIMG> GetModelById(int ShowId)
        {
            string sql = string.Format("SELECT * FROM TBL_STATIONINFO_SHOWIMG where Show_Id='{0}'", ShowId);
            using (var db = database.Connection)
            {
                return db.Query<STATIONINFO_SHOWIMG>(sql);
            }
        }
    }
}