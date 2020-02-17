using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IStationService : IDependency
    {
        IEnumerable<dynamic> GetStationList(string stcds, string addvcd, string type);
        List<dynamic> GetThreeLineData(string stcd,string sDate,string eDate);
        /// <summary>
        /// 获取查询条件智能显示信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="strType"></param>
        /// <param name="sttp">默认空字符:所有站点</param>
        /// <param name="count"></param>
        /// <param name="type">1：行政区 2：流域</param>
        /// <param name="addvcd">行政区或者流域编码</param>
        List<dynamic> GetSearchKeywords(string keyword, string strType, int count, string sttp, int type, string addvcd);

        /// <summary>
        /// 根据站码、年份和月份获取水位流量关系数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<dynamic> GetZQRLYearMonthData(string stcd, string year);

        /// <summary>
        /// 获取测站水位流量关系曲线有数据的年份和月份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetStationZQRLYearMonth(string stcd);

        /// <summary>
        /// 过流断面数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="year"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        List<dynamic> GetGLDMInfo(string stcd, string stnm, string year, string sDt);

        /// <summary>
        /// 获取测站断面时间序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetStationSectionYears(string stcd);

        /// <summary>
        /// 获取测站断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetStationSectionNames(string stcd);

        /// <summary>
        /// 获取水流沙过程FLASH数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        dynamic GetCalLineData(string stcd, string sDate, string eDate);

        /// <summary>
        /// 获取测站的断面信息，时间序列和断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        dynamic GetStationSectionInfo(string stcd);

        IEnumerable<dynamic> GetRainStationInfo(string stcd);

        #region Add byYM
        #region 历史特佂值
        //start
        /// <summary>
        /// add by YM 
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <returns></returns>
        List<dynamic> GetYearsZ(string stcd);
        /// <summary>
        /// 根据站码获取测站的年流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetYearsQ(string stcd);
        /// <summary>
        /// 根据站码获取测站的年含沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetYearsSand(string stcd);
        /// <summary>
        /// 根据站码获取测站的年径流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetYearsJL(string stcd);
        /// <summary>
        /// 根据站码获取测站的年输沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetYearsSSL(string stcd);
        /// <summary>
        /// 根据站码获取测站的年降水量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetYearsRain(string stcd);
        /// <summary>
        /// 根据站码获取输沙量径流量
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        dynamic GetYearsSSLJLL(string stcd);
        /// <summary>
        /// 根据条件获取测站的所有特征值数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        dynamic GetAllCVData(string stcd);
        #endregion
        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetZQRLData(string stcd);
        /// <summary>
        /// 获取测站有数据的年份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        List<dynamic> GetStationZQRLYear(string stcd);
        /// <summary>
        ///获取测站选择年份的数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        List<dynamic> GetZQRLYearsData(string stcd,string years);
        #endregion
        #region 设施设备
        /// <summary>
        /// 获取设施设备分类的信息树数据
        /// </summary>
        /// <returns></returns>
        dynamic GetFacEqTreeData();
        /// <summary>
        /// 根据站码和表名获取相关的表信息和测站的相关设施设备信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        dynamic GetFacEqFieldsAndData(string stcd, string tableName);
        #endregion
        #endregion

        //end
    }
}
