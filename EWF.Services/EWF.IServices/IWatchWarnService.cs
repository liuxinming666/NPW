/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijua
 * 日  期：2019/5/27 12:36:04
 * 描  述：WatchWarnRepository
 * *********************************************/
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IWatchWarnService : IDependency
    {
        /// <summary>
        /// 获取预警列表统计数据
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="type">类型：1行政区划2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        string GetWatchWarn(string startDate, int type, string addvcd);
        /// <summary>
        /// 根据指定的sname和tname返回对应的预警站点列表
        /// </summary>
        /// <param name="sname">预警类型名称</param>
        /// <param name="tname">具体预警类别</param>
        /// <param name="sdate">时间</param>
        /// <param name="sttp">测站类型</param>
        /// <param name="qjfz"></param>
        /// <returns></returns>
        string Get_WatchWarnDetailData(string sname, string tname, string sttp, string sdate, string qjfz);
    }
}
