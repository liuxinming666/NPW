using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.IRepository
{
    public interface ISysConfigRepository : IRepositoryBase<SYS_Config>, IDependency
    {
        IEnumerable<dynamic> GetSysConfigData(int sysId);
        /// <summary>
        /// 根据区域代码获取系统配置
        /// </summary>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        IEnumerable<SYS_Config> GetSysConfigByAddvcd(string addvcd);
        /// <summary>
        /// 根据系统配置的测站编码获取测站名称
        /// </summary>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetStnmBySysconfig(string addvcd);
        /// <summary>
        /// 根据测站名称获取测站编码
        /// </summary>
        /// <param name="stnm"></param>
        /// <returns></returns>
        DataTable GetStationListByStnm(string stnm);
    }
}
