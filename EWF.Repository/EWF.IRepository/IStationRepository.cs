using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using System.Dynamic;

namespace EWF.IRepository
{
    public interface IStationRepository : IDependency
    {
        IEnumerable<dynamic> GetStationList(string stcds, string addvcd, string type);
        IEnumerable<dynamic> GetThreeLineData(string stcd, string sDate, string eDate);
        IEnumerable<dynamic> GetSearchKeywords(string keyword, string strType, int count, string sttp, int type, string addvcd);
        /// <summary>
        /// 根据站码、年份和月份获取水位流量关系数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetZQRLYearMonthData(string stcd, string year);

        /// <summary>
        /// 获取测站水位流量关系曲线有数据的年份和月份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetStationZQRLYearMonth(string stcd);

        /// <summary>
        /// 获取测站断面时间序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetStationSectionYears(string stcd);

        /// <summary>
        /// 获取雨量站信息数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetRainStationInfo(string stcd);

        /// <summary>
        /// 获取测站断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetStationSectionNames(string stcd);

        /// <summary>
        /// 过流断面数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="year"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetGLDMInfo(string stcd, string stnm, string year, string sDt);

        /// <summary>
        /// 获取水流沙过程线FLASH数据
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

        #region Add by YM
        #region 历史特佂值
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsZ(string stcd);
        /// <summary>
        /// 根据站码获取测站的年流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsQ(string stcd);
        /// <summary>
        /// 根据站码获取测站的年含沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsSand(string stcd);
        /// <summary>
        /// 根据站码获取测站的年径流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsJL(string stcd);
        /// <summary>
        /// 根据站码获取测站的年输沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsSSL(string stcd);
        /// <summary>
        /// 根据站码获取测站的年降水量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetYearsRain(string stcd);
        /// <summary>
        /// 根据条件获取测站的所有特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        dynamic GetAllCVData(string stcd);
        /// <summary>
        /// 根据站码获取输沙量径流量
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        //dynamic GetYearsSSLJLL(string stcd);
        #endregion
        # region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetZQRLData(string stcd);
        /// 获取测站有数据的年份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetStationZQRLYear(string stcd);
        /// <summary>
        ///获取测站选择年份的数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetZQRLYearsData(string stcd, string years);
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
    }
}
