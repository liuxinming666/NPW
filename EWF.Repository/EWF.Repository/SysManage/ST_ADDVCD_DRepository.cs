using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Repository
{
    public class ST_ADDVCD_DRepository : DefaultRepository<ST_ADDVCD_D>, IST_ADDVCD_DRepository
    {

        public ST_ADDVCD_DRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
        }

        /// <summary>
        /// 获取行政区划分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ADDVCD"></param>
        /// <param name="ADDVNM"></param>
        /// <returns></returns>
        public Page<ST_ADDVCD_D> GetAddvcdData(int pageIndex, int pageSize, string ADDVCD, string ADDVNM,string TYPE)
        {
            var where = " where 1=1 ";
            var orderby = " ADDVCD ";
            if (!ADDVCD.IsEmpty())
            {
                if (TYPE == "1")
                {
                    if (ADDVCD.Substring(5, 2) == "00")
                        where += " and substring(addvcd,1,4)='" + ADDVCD.Substring(1, 4) + "' and type=" + TYPE;
                    else
                        where += " and addvcd='" + ADDVCD + "' and type=" + TYPE;
                }
                else
                    where += " and addvcd='" + ADDVCD + "' and type=" + TYPE;
            }
            //if (!ADDVCD.IsEmpty())
            //{
            //    where += "and ADDVCD like '" + ADDVCD + "%' ";
            //}
            if (!ADDVNM.IsEmpty())
            {
                where += " and ADDVNM like '%" + ADDVNM + "%' ";
            }
            //if (TYPE != "0")
            //{
            //    where += " and TYPE = '" + TYPE + "' ";
            //}
            //var db = new RepositoryBase(database);
            //var pageuser = db.GetListPaged<ST_ADDVCD_D>(pageIndex, pageSize, where, orderby);
            var pageuser = GetListPaged(pageIndex, pageSize, where, orderby);
            return pageuser;
        }
    }
}
