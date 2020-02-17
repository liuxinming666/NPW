/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:36:04
 * 描  述：RiverRepository
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
    public interface IRiverRepository : IDependency
    {
        Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM);

        /// <summary>
        /// 获取指定时间段内最新水情
        /// add by SUN
        /// Date:2019-05-22 23:00
        /// </summary>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetLatestRiver(string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 查询水情信息
        /// add by SUN
        /// Date:2019-05-23 10:00
        /// </summary>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间，为空则起始时间为表中最早时间</param>
        /// <param name="endDate">结束时间，为空则结束时间为表中最后时间</param>
        /// <param name="addvcd">行政区、流域编码</param>
        /// <param name="type">行政区、流域类型</param>
        /// <returns></returns>
        Page<dynamic> GetRiverData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type);

        /// <summary>
        /// 查询首页单块单站水情信息
        /// </summary>
        ///<param name="unit">所属单位</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverData(string unit, string stnm);

        /// <summary>
        /// 查询水情信息-未分页
        /// add by SUN
        /// Date:2019-05-23 17:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="hourFilter">小时过滤(整点过滤，范围-1-23，-1表示不过滤)，默认只显示8点数据  eg:传入6,则只显示6点数据</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverData(string stcd, string startDate, string endDate, int hourFilter = 8);

        /// <summary>
        /// 查询多站水情信息-未分页
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverDataByMultiStcds(string stcds, string startDate, string endDate);

        /// <summary>
        /// 查询水情均值
        /// add by SUN
        /// Date:2019-05-24 10:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRvavData(string stcd, string startDate, string endDate);
        /// <summary>
        /// 查询历史多站水情对比信息-未分页
        /// add by JinJianping
        /// Date:2019-05-30 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        dynamic GetMutiRiverData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history);
        /// <summary>
        /// 查询历史多站水情对比信息-未分页
        /// add by JinJianping
        /// Date:2019-05-30 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        dynamic GetMutiSedData_Comparative(string stcds, string startDate, string endDate, string startDate_history, string endDate_history);
        /// <summary>
        /// 水流沙过程对照
        /// create by zhujun on 2019-5-31 11:36
        /// </summary>
        /// <param name="stcds">测站名称</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <returns>{data.zqlist：水位流量，data.slist：含沙量}</returns>
        dynamic GetMutliStationZQS(string stcds, string stime, string etime);

        /// <summary>
        /// 断面水位数据
        /// add by SUN
        /// Date:2019-07-10 11:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="tm"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        DataSet GetSectionZ(string stcd, string stnm, string tm, string sDt);

    }

}