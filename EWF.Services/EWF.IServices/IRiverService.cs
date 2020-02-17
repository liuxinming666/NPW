using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IRiverService : IDependency
    {
        Page<dynamic> GetReadData(int pageIndex,int pageSize,string STNM);

        /// <summary>
        /// 返回最新水情数据
        /// add by SUN
        /// Date:2019-05-23 13:00
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetLatestRiverData(string addvcd,string type);

        /// <summary>
        /// 返回水情信息
        /// add by SUN
        /// Date:2019-05-23 14:00
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="addvcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Page<dynamic> GetRiverData(int pageIndex, int pageSize, string stcds, string startDate, string endDate,string  addvcd,string type);
        /// <summary>
        /// 查询分块单站水情信息
        /// </summary>
        /// <param name="unit">所属单位</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverData(string unit, string stnm);
        /// <summary>
        /// 查询单站水情历史同期对比数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <param name="sdate">开始时间</param>
        /// <param name="edate">结束时间</param>
        /// <param name="compareYear">对比年份</param>
        /// <returns></returns>
        List<dynamic> GetDataForSingleRiverCompare(string stcd, string sdate, string edate, string compareYear);

        /// <summary>
        /// 获取水情信息-未分页
        /// add by SUN
        /// Date:2019-05-24 00:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverData(string stcd, string startDate, string endDate);

        /// <summary>
        /// 查询水情均值
        /// add by SUN
        /// Date:2019-05-24 10:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<dynamic> GetRvavData(string stcd, string startDate, string endDate);

        /// <summary>
        /// 查询多站水位对比数据，列标题为站码
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string GetZDataByMultiStcds(string stcds, string startDate, string endDate);
        /// <summary>
        /// 多站流量对比数据，列标题位站码
        /// add by SUN
        /// Date:2019-06-11 16:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string GetQDataByMultiStcds(string stcds, string startDate, string endDate);
        /// <summary>
        /// 查询历史多站水情数据-未分页
        /// add by JinJianping
        /// Date:2019-05-30 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetHistoryRiverByMultiStcds(string stcds, string startDate, string endDate, int year, int type);

        /// <summary>
        /// 水流沙过程对照
        /// create by zhujun on 2019-05-31 11:40
        /// </summary>
        /// <param name="stcds">测站名称</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <returns>{data.zqlist：水位流量，data.slist：含沙量}</returns>
        dynamic GetMutliStationZQS(string stcds, string stime, string etime);

        /// <summary>
        /// 获取多站水情数据（水位、流量）
        /// add by SUN
        /// Date:2019-06-11 16:00
        /// </summary>
        /// <param name="stcds">站码列表，eg:"40100150,40100160"</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        List<dynamic> GetRiverDataMultiSta(string stcds, string startDate, string endDate);

        /// <summary>
        /// 首页水位流量过程线
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="endDate"></param>
        /// <param name="stype">0表示实时水情只取八点数据，1表示在线水位时间不过滤</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRiverChartData(string stcd, string endDate, int stype);

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
        string GetSectionZ(string stcd, string stnm, string tm, string sDt);
    }
}
