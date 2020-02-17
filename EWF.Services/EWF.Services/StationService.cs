using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWF.Services
{
    public class StationService:IStationService
    {
        private IStationRepository repository;

        public StationService(IStationRepository _epository)
        {
            repository = _epository;
        }

        public IEnumerable<dynamic> GetStationList(string stcds,string addvcd, string type)
        {
            return repository.GetStationList(stcds, addvcd, type);
        }

        public IEnumerable<dynamic> GetRainStationInfo(string stcd)
        {
            return repository.GetRainStationInfo(stcd);
        }

        public List<dynamic> GetThreeLineData(string stcd, string sDate, string eDate)
        {
            var list = repository.GetThreeLineData(stcd,sDate,eDate);
            return list.ToList();
        }

        /// <summary>
        /// 过流断面数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="stnm"></param>
        /// <param name="year"></param>
        /// <param name="sDt"></param>
        /// <returns></returns>
        public List<dynamic> GetGLDMInfo(string stcd, string stnm, string year, string sDt)
        {
            var list = repository.GetGLDMInfo(stcd,stnm,year,sDt);

            return list.ToList();
        }

        /// <summary>
        /// 根据站码、年份和月份获取水位流量关系数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<dynamic> GetZQRLYearMonthData(string stcd, string year)
        {
            var list = repository.GetZQRLYearMonthData(stcd,year);

            return list.ToList();
        }

        /// <summary>
        /// 获取测站水位流量关系曲线有数据的年份和月份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetStationZQRLYearMonth(string stcd)
        {
            var list = repository.GetStationZQRLYearMonth(stcd);

            return list.ToList();
        }

        /// <summary>
        /// 获取测站断面时间序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetStationSectionYears(string stcd)
        {
            var list = repository.GetStationSectionYears(stcd);
            return list.ToList();
        }

        public List<dynamic> GetStationSectionNames(string stcd)
        {
            var list = repository.GetStationSectionNames(stcd);

            return list.ToList();
        }

        /// <summary>
        /// 获取水流沙过程FLASH数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public dynamic GetCalLineData(string stcd, string sDate, string eDate)
        {
            var result = repository.GetCalLineData(stcd, sDate, eDate);

            return result;
        }

        /// <summary>
        /// 获取测站的断面信息，时间序列和断面名称序列
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public dynamic GetStationSectionInfo(string stcd)
        {
            var result = repository.GetStationSectionInfo(stcd);

            return result;
        }

        public List<dynamic> GetSearchKeywords(string keyword, string strType, int count, string sttp, int type, string addvcd)
        {
            var list = repository.GetSearchKeywords(keyword, strType, count, sttp, type, addvcd);
            return list.ToList();
        }

        #region Add  by YM 
        #region 历史特征值
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsZ(string stcd)
        {
            var list = repository.GetYearsZ(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取测站的年流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsQ(string stcd)
        {
            var list = repository.GetYearsQ(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取测站的年含沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsSand(string stcd)
        {
            var list = repository.GetYearsSand(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取测站的年径流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsJL(string stcd)
        {
            var list = repository.GetYearsJL(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取测站的年输沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsSSL(string stcd)
        {
            var list = repository.GetYearsSSL(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取测站的年降水量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetYearsRain(string stcd)
        {
            var list = repository.GetYearsRain(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 根据站码获取输沙量径流量
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public dynamic GetYearsSSLJLL(string stcd)
        {
            var list = repository.GetYearsRain(stcd);
            return list;
        }
        /// <summary>
        /// 根据条件获取测站的所有特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public dynamic GetAllCVData(string stcd)
        {
            var list = repository.GetAllCVData(stcd);
            return list;
        }
        #endregion
        #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetZQRLData(string stcd)
        {
            var list = repository.GetZQRLData(stcd);
            return list.ToList();
        }
        /// <summary>
        /// 获取测站有数据的年份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public List<dynamic> GetStationZQRLYear(string stcd)
        {
            var list = repository.GetStationZQRLYear(stcd);
            return list.ToList();
        }
        /// <summary>
        ///获取测站选择年份的数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public List<dynamic> GetZQRLYearsData(string stcd, string years)
        {
            var list = repository.GetZQRLYearsData(stcd, years);
            return list.ToList();
        }
        #endregion
        #region 设施设备
        /// <summary>
        /// 获取设施设备分类的信息树数据
        /// </summary>
        /// <returns></returns>
        public dynamic GetFacEqTreeData()
        {
            var list = repository.GetFacEqTreeData();
            return list;
        }
        /// <summary>
        /// 根据站码和表名获取相关的表信息和测站的相关设施设备信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public dynamic GetFacEqFieldsAndData(string stcd, string tableName)
        {
            var list = repository.GetFacEqFieldsAndData(stcd, tableName);
            return list;
        }
        #endregion
        #endregion
    }
}
