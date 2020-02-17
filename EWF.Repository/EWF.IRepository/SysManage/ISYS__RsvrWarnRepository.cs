using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IRepository.SysManage
{
    public interface ISYS__RsvrWarnRepository :  IDependency
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
        Page<dynamic> GetRsvrWarnData(int pageIndex, int pageSize, string stnm, int type, string addvcd);
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        string UpdateData(SYS_ST_RSVRFSR_B model);
    }
}
