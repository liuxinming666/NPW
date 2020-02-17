/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/5/31 10:36:04
 * 描  述：IVideoRepository
 * *********************************************/
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
    public interface IVideoRepository:IDependency
    {
        /// <summary>
        /// 获取视频站点列表
        /// </summary>
        /// <returns></returns>
        DataTable GetVideoList(string STCD, int type, string addvcd);

        /// <summary>
        /// 获取视频站点配置列表
        /// </summary>
        /// <returns></returns>
        DataTable GetVideoManageList(string STCD);
    }

}