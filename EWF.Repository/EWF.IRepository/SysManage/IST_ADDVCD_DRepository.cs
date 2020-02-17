using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository
{
    public interface IST_ADDVCD_DRepository: IRepositoryBase<ST_ADDVCD_D>, IDependency
    {
        /// <summary>
        /// 获取行政区划分页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ADDVCD"></param>
        /// <param name="ADDVNM"></param>
        /// <returns></returns>
        Page<ST_ADDVCD_D> GetAddvcdData(int pageIndex, int pageSize, string ADDVCD, string ADDVNM,string TYPE);
    }
}
