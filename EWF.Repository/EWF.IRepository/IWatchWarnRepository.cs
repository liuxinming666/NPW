/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijua
 * 日  期：2019/5/27 12:36:04
 * 描  述：WatchWarnRepository
 * *********************************************/
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IRepository
{
    public interface IWatchWarnRepository: IDependency
    {
        /// <summary>
        /// 获取预警信息数据
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">类型：1行政区划2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        DataTable GetWarnWarnData(string startDate, string endDate, int type, string addvcd);
        /// <summary>
        /// 获取详细的预警信息数据
        /// </summary>
        /// <param name="sname">预警类型名称</param>
        /// <param name="tname">具体预警类别</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="sttp">测站类型</param>
        /// <param name="qjfz"></param>
        /// <returns></returns>
        DataTable GetWarnWarnDetailData(string sname, string tname, string startDate, string endDate, string sttp, string qjfz);
    }
}
