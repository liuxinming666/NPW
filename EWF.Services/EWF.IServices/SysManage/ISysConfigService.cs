using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface ISysConfigService : IDependency
    { /// <summary>
      /// 根据id获取系统配置信息
      /// </summary>
        SYS_Config GetSysByID(int ID);
        /// <summary>
        /// 添加系统配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string Insert(SYS_Config entity);
        /// <summary>
        /// 修改系统配置信息
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        string Update(SYS_Config entity);
        IEnumerable<dynamic> GetSysConfigData(int sysId);
        /// <summary>
        /// 根据区域获取配置
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

        /// <summary>
        /// 获取视频站点摄像头树状信息
        /// </summary>
        /// <param name="stnm"></param>
        /// <returns></returns>
        string getVideoManageList(string addvcd, int type);
    }
}
