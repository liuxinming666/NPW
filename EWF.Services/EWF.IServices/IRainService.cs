using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EWF.IServices
{
    public interface IRainService : IDependency
    {
        /// <summary>
        /// 获取每个站的最新降雨
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetLatestRain(int type, string addvcd);
        /// <summary>
        /// 返回雨情信息
        /// add by JinJianPing
        /// Date:2019-05-27 14:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable GetRainData(string stcds, string startDate, string endDate, int type, string addvcd);

        /// <summary>
        /// 返回等值面等值线geojson数据
        /// add by SUN
        /// Date:2019-05-27 15:00
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <param name="featureType">线面类型，0-返回等值线要素，1-返回等值面要素，默认值为1</param>
        /// <returns></returns>
        Task<string> GetRainAnalysisData(string startDate, string endDate, int areatype, string addvcd, int type = 1, int featureType = 1);

        /// <summary>
        /// 查询时段累积雨量
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="stcds">站码列表，格式：'41203700','41101600'，当传至为空时，查询全部站点数据</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
		Page<dynamic> GetRainDataPeriod(int page, int rows, string stcds, string startDate, string endDate, int type, string addvcd);

        /// <summary>
        /// 返回旬月雨情信息
        /// add by JinJianPing
        /// Date:2019-05-28 11:23
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable GetRainMonthData(string stcds, string startDate, string endDate, int type, string addvcd);
        /// <summary>
        /// 获取某个测站起止时间内所有时段雨量，即数据库记录
        /// </summary>
        /// <param name="stcd">测站编码</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        List<dynamic> GetRainDataPeriodDetail(string stcd, string startDate, string endDate);

        /// <summary>
        /// 获取雨强信息
        /// add by SUN
        /// Date:2019-05-30 10:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<dynamic> GetRaininess(string startDate, string endDate, int type, string addvcd);

        // <summary>
        /// 获取区域的面平均雨量，根据权重计算
        /// add by SUN
        /// Date:2019-05-30 13:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="adcd">区划码</param>
        /// <param name="featureType">要素类型（0-时段雨量DRP,1-日雨量DYP,默认值1)</param>
        /// <returns></returns>
        double GetAreaAvg(string startDate, string endDate, string adcd, int featureType=1);

        /// <summary>
        /// 返回等值面等值线geojson数据
        /// add by SUN
        /// Date:2019-05-27 15:00
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <returns></returns>
        List<dynamic> GetLatestRainData(int areatype, string addvcd, int type = 1);

        /// <summary>
        /// 获取首页雨量柱状图信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="endDate"></param>
        /// <param name="stype">0：时段降雨DRP  1：昨日降雨DYP</param>
        /// <returns></returns>
        List<dynamic> GetRainChartData(string stcd, string endDate, string stype);
        /// <summary>
        /// 首页获取最新24小时降雨量统计
        /// </summary>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        List<dynamic> GetLatestRainStaticData(int type, string addvcd);
        /// <summary>
        /// 首页获取最新24小时降雨量统计数量明细
        /// </summary>
        /// <param name="slevel">降雨量级别</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        List<dynamic> GetRainStaticDetailData(string slevel, string endDate, int type, string addvcd);

        /// <summary>
        /// 返回等值面等值线分析所需要的数据
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <param name="addvcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<dynamic> GetSumRain(string startDate, string endDate, int areatype, string addvcd, int type = 1);

        /// <summary>
        /// 获取时段雨量
        /// add by SUN
        /// Date:2019-08-03 18:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rainValue"></param>
        /// <param name="rainValue2"></param>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        DataTable GetRainDataPeriod(string stcds, string startDate, string endDate, double rainValue, double rainValue2, string addvcd);
    }
}
