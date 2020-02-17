using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EWF.IServices
{
    public interface IVideoService : IDependency
    {
        /// <summary>
        /// 获取视频站点列表
        /// </summary>
        /// <returns></returns>
        DataTable GetVideoList(string STCD,int type, string addvcd);
        /// <summary>
        /// 获取视频站点列表字符串
        /// </summary>
        /// <returns></returns>
        string GetVideos(string STCD, int type, string addvcd);
        /// <summary>
        /// 获取视频站点配置列表
        /// </summary>
        /// <returns></returns>
        DataTable GetVideoManageList(string STCD);
    }
}
