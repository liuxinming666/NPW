using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository
{
    public interface IRiverWarnSetRepository :  IDependency
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stnm">测站名称</param>
        /// <param name="type">类型</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        Page<dynamic> GetRiverWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd);
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        string UpdateData(ST_RVFCCH_B model);
    }
}
