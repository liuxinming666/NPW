/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:48:11
 * 描  述：RiverService
 * *********************************************/
using EWF.Entity;
using EWF.Entity.Models;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EWF.Services
{
    public class RiverService: IRiverService
    {
        private IRiverRepository repository;
        DataOption dataOption;
        public RiverService(IRiverRepository _epository, IOptionsSnapshot<DataOption> _option)
        {
            repository = _epository;
            dataOption = _option.Get("DataOption");
        }

        public Page<dynamic> GetReadData(int pageIndex, int pageSize,string STNM)
        {

            var list = repository.GetReadData(pageIndex,pageSize,STNM);
            return list;
        }

        /// <summary>
        /// 获取一天内最新水情信息
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetLatestRiverData(string addvcd, string type)
        {
            string eDate = DateTime.Now.ToString();
            string sDate = DateTime.Now.AddDays(-dataOption.Interval).ToString();
            var list = repository.GetLatestRiver(sDate, eDate, addvcd,type);
            return list.ToList<dynamic>();
        }
        
        public Page<dynamic> GetRiverData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type)
        {
            var list = repository.GetRiverData(pageIndex, pageSize, stcds, startDate, endDate, addvcd, type);
            return list;
        }
        public IEnumerable<dynamic> GetRiverData(string unit, string stnm)
        {
            var list = repository.GetRiverData(unit, stnm);
            return list;
        }

        public IEnumerable<dynamic> GetRiverData(string stcd, string startDate, string endDate)
        {
            return repository.GetRiverData(stcd, startDate, endDate);
        }
        /// <summary>
        /// 首页水位流量过程线
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="endDate"></param>
        /// <param name="stype">0表示实时水情只取八点数据，1表示在线水位时间不过滤</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRiverChartData(string stcd, string endDate, int stype)
        {
            string startDate = "";// DateTime.Now.AddDays(-dataOption.SysRiver).ToString();            
            if (stype == 0)
            {
                //实时水情只取八点数据
                startDate = Convert.ToDateTime(endDate).AddDays(-dataOption.SysRiver).ToString();
                return repository.GetRiverData(stcd, startDate, endDate);
            }
            else
            {
                //在线水位不过滤时间，-1表示不过滤
                startDate = Convert.ToDateTime(endDate).AddHours(-dataOption.SysOnlineZ).ToString();
                return repository.GetRiverData(stcd, startDate, endDate, -1);
            }
            
        }

        /// <summary>
        /// 查询水情均值
        /// add by SUN
        /// Date:2019-05-24 10:00
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<dynamic> GetRvavData(string stcd, string startDate, string endDate)
        {
            return repository.GetRvavData(stcd, startDate, endDate).ToList();
        }

        public List<dynamic> GetDataForSingleRiverCompare(string stcd, string sdate, string edate, string compareYear)
        {
            string sdate2 = compareYear + sdate.ToDateTime().ToString("-MM-dd HH:mm");
            string edate2 = compareYear + edate.ToDateTime().ToString("-MM-dd HH:mm");


            List<dynamic> listCurrentYear = repository.GetRiverData(stcd, sdate, edate).ToList();
            
            List<dynamic> listCompareYear = repository.GetRiverData(stcd, sdate2, edate2).ToList();


            #region 格式转换

            List<dynamic> re = new List<dynamic>();
            foreach (var item in listCurrentYear)
            {
                
                DateTime tm = item.TM;
                var itemCopare = listCompareYear.Find(s => s.TM.ToString("MM-dd HH:mm") == tm.ToString("MM-dd HH:mm"));
                if (itemCopare != null)
                {
                   
                }

                dynamic row = new ExpandoObject();
                row.STCD = item.STCD;
                row.STNM = item.STNM;
                row.TM = tm;
                row.CurrentZ = item.Z;
                row.CurrentQ = item.Q;
                row.CurrentS = item.S;
                row.CompareZ = itemCopare?.Z;
                row.CompareQ = itemCopare?.Q;
                row.CompareS = itemCopare?.S;

                re.Add(row);

            }
            

            #endregion

            return re;
        }

        /// <summary>
        /// 查询多站水位对比数据，站码为列标题
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string GetZDataByMultiStcds(string stcds, string startDate, string endDate)
        {
            List<dynamic> listData=repository.GetRiverDataByMultiStcds(stcds, startDate, endDate).ToList();
            string[] arrStcd;
            //创建结果表结构 BEGIN
            DataTable dt = new DataTable(); 
            dt.Columns.Add("TM"); 
            if (!string.IsNullOrEmpty(stcds))
            {
                arrStcd = stcds.Split(',');
                foreach (string strStcd in arrStcd)
                {
                    if (!dt.Columns.Contains(strStcd))
                    {
                        dt.Columns.Add(strStcd.Trim('\''));
                    }
                    
                }
            }
            //创建结果表结构 END

            //按照新表结构重新组织数据
            Dictionary<string, DataRow> dir = new Dictionary<string, DataRow>();
            foreach (dynamic dy in listData)
            {
                string key = dy.TM.ToString();
                if (!dir.Keys.Contains(key))
                {
                    DataRow drNew = dt.NewRow();
                    
                    drNew["TM"] = key;
                    drNew[dy.STCD] = dy.Z.ToString();
                    dt.Rows.Add(drNew);
                    dir.Add(key, drNew);
                }
                else
                {
                    dir[key][dy.STCD] = dy.Z;
                }
            }
            return Json.ToJson(dt);
        }

        /// <summary>
        /// 多站流量对比，列标题为站码
        /// add by SUN
        /// Date:2019-06-11 16:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string GetQDataByMultiStcds(string stcds, string startDate, string endDate)
        {
            List<dynamic> listData = repository.GetRiverDataByMultiStcds(stcds, startDate, endDate).ToList();
            string[] arrStcd;
            //创建结果表结构 BEGIN
            DataTable dt = new DataTable();
            dt.Columns.Add("TM");
            if (!string.IsNullOrEmpty(stcds))
            {
                arrStcd = stcds.Split(',');
                foreach (string strStcd in arrStcd)
                {
                    if (!dt.Columns.Contains(strStcd))
                    {
                        dt.Columns.Add(strStcd.Trim('\''));
                    }

                }
            }
            //创建结果表结构 END

            //按照新表结构重新组织数据
            Dictionary<string, DataRow> dir = new Dictionary<string, DataRow>();
            foreach (dynamic dy in listData)
            {
                string key = dy.TM.ToString();
                if (!dir.Keys.Contains(key))
                {
                    DataRow drNew = dt.NewRow();

                    drNew["TM"] = key;
                    drNew[dy.STCD] = dy.Q.ToString();
                    dt.Rows.Add(drNew);
                    dir.Add(key, drNew);
                }
                else
                {
                    dir[key][dy.STCD] = dy.Q;
                }
            }
            return Json.ToJson(dt);
        }


        /// <summary>
        /// 获取多站水情数据（水位、流量）
        /// add by SUN
        /// Date:2019-06-11 16:00
        /// </summary>
        /// <param name="stcds">站码列表，eg:"40100150,40100160"</param>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public List<dynamic> GetRiverDataMultiSta(string stcds, string startDate, string endDate)
        {
            List<dynamic> listData = repository.GetRiverDataByMultiStcds(stcds, startDate, endDate).ToList();
            return listData;
        }

        /// <summary>
        /// 查询历史多站水情对比数据-未分页
        /// add by JinJianping
        /// Date:2019-05-30 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetHistoryRiverByMultiStcds(string stcds, string startDate, string endDate, int year, int type)
        {
            string startDate_History = year + "-" + Convert.ToDateTime(startDate).Month + "-" + Convert.ToDateTime(startDate).Day + " 07:59";
            string endDate_History = year + "-" + Convert.ToDateTime(endDate).Month + "-" + Convert.ToDateTime(endDate).Day + " 08:01";

            dynamic result;
            if (type == 3)//含沙量
            {
                result = repository.GetMutiSedData_Comparative(stcds, startDate + " 07:59", endDate + " 08:01", startDate_History, endDate_History);
                return ConvertMultiSed_Comparative(result.sedreal, result.sedhistory, type);
            }
            else//水位、流量
            {
                result = repository.GetMutiRiverData_Comparative(stcds, startDate + " 07:59", endDate + " 08:01", startDate_History, endDate_History);
                return ConvertMultiRiver_Comparative(result.riverreal, result.riverhistory, type);
            }
        }
        
        /// <summary>
        /// 水流沙过程对照
        /// create by zhujun on 2019-05-31 11:40
        /// </summary>
        /// <param name="stcds">测站名称</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <returns>{data.zqlist：水位流量，data.slist：含沙量}</returns>
        public dynamic GetMutliStationZQS(string stcds, string stime, string etime)
        {
            var data = repository.GetMutliStationZQS(stcds, stime, etime);
            
            List<ST_RIVEREntity> zqlist = data.zqlist;
            List<ST_RIVEREntity> slist = data.slist;


            var list = new List<dynamic>();
            var stcdArray = stcds.Split(',');
            foreach (var item in stcdArray)
            {
                if (item.Trim().IsEmpty()) continue;
                dynamic temp = new ExpandoObject();
                temp.STCD = item;

                var tempList = zqlist.Where(x => x.STCD == item);
                if (tempList.Count() > 0)
                {
                    var zMaxRow = tempList.OrderByDescending(x => x.Z).First();
                    var qMaxRow = tempList.OrderByDescending(x => x.Q).First();

                    temp.STNM = zMaxRow.STNM;
                    temp.STM = stime;
                    temp.ETM = etime;


                    temp.MAXZ = zMaxRow.Z;
                    temp.MAXZ_TM = zMaxRow.TM;
                    temp.MAXQ = qMaxRow.Q;
                    temp.MAXQ_TM = qMaxRow.TM;
                }

                if (slist.Count(x => x.STCD == item) > 0)
                {
                    var sMaxRow = slist.Where(x => x.STCD == item).OrderByDescending(x => x.S).First();
                    
                    temp.MAXS = sMaxRow.Z;
                    temp.MAXS_TM = sMaxRow.Z;
                }
               

                temp.WRNF = 0;
                temp.STW = 0;
                list.Add(temp);
            }


            dynamic result = new ExpandoObject();
            result.zqlist = data.zqlist;
            result.slist = data.slist;
            result.countlist = list;
            
            return result;
        }

        #region 数据处理组装 -对比分析
        //水情历史同期对比分析
        private IEnumerable<dynamic> ConvertMultiRiver_Comparative(IEnumerable<ST_RIVEREntity> list_real, IEnumerable<ST_RIVEREntity> list_history, int type)
        {
            var list_result = new List<dynamic>();

            foreach (var item in list_real)
            {
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.TM.Month == item.TM.Month && x.TM.Day == item.TM.Day);

                double? Z_Comparative = null;
                if (row_Comparative.Count() > 0)
                {
                    if (type == 1)
                    {
                        Z_Comparative = row_Comparative.FirstOrDefault().Z;
                    }
                    else
                    {
                        Z_Comparative = row_Comparative.FirstOrDefault().Q;
                    }
                }

                dynamic row = new
                {
                    STCD = item.STCD,
                    STNM = item.STNM.Trim(),
                    TM = item.TM,
                    Z = item.Z,             
                    Z_Comparative = Z_Comparative
                };
                list_result.Add(row);
            }
            return list_result;
        }
        //多站含沙量历史同期对比分析
        private IEnumerable<dynamic> ConvertMultiSed_Comparative(IEnumerable<ST_SEDEntity> list_real, IEnumerable<ST_SEDEntity> list_history, int type)
        {
            var list_result = new List<dynamic>();

            foreach (var item in list_real)
            {
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.TM.Month == item.TM.Month && x.TM.Day == item.TM.Day);

                decimal? Z_Comparative = null;
                if (row_Comparative.Count() > 0)
                {
                    Z_Comparative = row_Comparative.FirstOrDefault().S;
                }

                dynamic row = new
                {
                    STCD = item.STCD,
                    STNM = item.STNM.Trim(),
                    TM = item.TM,
                    Z = item.S,
                    Z_Comparative = Z_Comparative
                };
                list_result.Add(row);
            }
            return list_result;
        }

        #endregion

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
        public string GetSectionZ(string stcd, string stnm, string tm, string sDt)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(repository.GetSectionZ(stcd,stnm,tm,sDt));
        }


    }
}
