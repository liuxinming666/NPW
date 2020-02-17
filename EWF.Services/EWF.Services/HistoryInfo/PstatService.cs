/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:48:11
 * 描  述：RiverService
 * *********************************************/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using EWF.Util;

namespace EWF.Services
{
    public class PstatService : IPstatService
    {
        private IPstatRepository repository;
        public PstatService(IPstatRepository _epository)
        {
            repository = _epository;
        }

        public IEnumerable<dynamic> GetDayData(string STCD, string sdate, string edate, int type, string addvcd, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 07:59");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:01");

            var list_history = repository.GetDayData(STCD, stime, etime, type, addvcd);
            var list_real = repository.GetDayData_ByPPTN(STCD, stime, etime, type, addvcd);

            //历史库有数据
            if (list_history.Count() > list_real.Count())
            {
                datasrc = "read";
                return ConvertTableDay(list_history);
            }
            return ConvertTableDay(list_real);
        }

        public IEnumerable<dynamic> GetMonthData(string STCD, string smonth, string emonth, int type, string addvcd, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(smonth + "-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(emonth + "-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");

            var list = repository.GetMonthData(STCD, sdate, edate, type, addvcd);


            var list_result = ConvertTableMonth(list);

            return list_result;
        }

        public DataTable GeYearData(string STCD, string year, int type, string addvcd, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(year + "-01-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(year + "-12-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");


            var list = repository.GeYearData(STCD, sdate, edate, type, addvcd);

            var list_result = ConvertTableYear(list);

            return list_result;
        }

       
        //表格转置-多日累计
        private IEnumerable<dynamic> ConvertTableDay(IEnumerable<ST_PSTATEntity> list)
        {
            var list_result = new List<dynamic>();

            double SUMACCP = 0;
            var stcd_current = "";
            foreach (var item in list)
            {
                if (stcd_current != item.STCD.Trim())
                {
                    stcd_current = item.STCD.Trim();
                    SUMACCP = 0;
                }
                SUMACCP += item.ACCP.SafeValue();
                dynamic row = new
                {
                    STCD = item.STCD,
                    STNM = item.STNM,
                    IDTM = item.IDTM,
                    STTDRCD = item.STTDRCD,
                    ACCP = item.ACCP.SafeValue().ToString("F1"),
                    SUMACCP = SUMACCP.ToString("F1")
                };
                list_result.Add(row);
            }
            return list_result;
        }

        //表格转置-旬月累计
        private IEnumerable<dynamic> ConvertTableMonth(IEnumerable<ST_PSTATEntity> list)
        {
            var list_result = new List<dynamic>();

            dynamic row_new = new ExpandoObject();
            Hashtable STCDList = new Hashtable();

            int index = 0;
            foreach (var item in list)
            {
                string STCD = item.STCD ;
                DateTime time = item.IDTM;
                int IDT = time.Day / 10;
                if (item.STTDRCD == "5" )
                {
                    time = time.AddMonths(-1);
                    IDT = 4;
                }
                if (item.STTDRCD == "4" &&IDT == 0)
                {
                    time = time.AddMonths(-1);
                }
                string Month = time.ToString("yy-MM");
                var MapKey = STCD + "-" + Month;
                if (!STCDList.ContainsKey(MapKey))
                {
                    row_new = new ExpandoObject();
                    double? tempVal = null;
                    row_new.SXun = tempVal;
                    row_new.ZXun = tempVal;
                    row_new.XXun = tempVal;
                    row_new.MonSum = tempVal;
                    
                    row_new.STNM= item.STNM.Trim();
                    row_new.Month = Month;

                    list_result.Add(row_new);
                    STCDList.Add(MapKey, index);
                    index++;
                }
                else
                {
                    row_new = list_result[Convert.ToInt32(STCDList[MapKey])];
                }

                //赋值
                switch (IDT) {
                    case 1:
                        row_new.SXun = item.ACCP.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 2:
                        row_new.ZXun = item.ACCP.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 0:
                        row_new.XXun = item.ACCP.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 4:
                        row_new.MonSum = item.ACCP.SafeValue().ToString("F1").ToDouble();
                        break;
                    //default:
                    //    throw new Exception("索引超出界限");
                }
            }
            
            return list_result;
        }

        //表格转置-年累计
        private DataTable ConvertTableYear(IEnumerable<ST_PSTATEntity> list)
        {
            #region
            DataTable dt = new DataTable();
            DataRow row_new;
            dt.Columns.Add("STNM");
            dt.Columns.Add("Year");
            dt.Columns.Add("Jan");
            dt.Columns.Add("Feb");
            dt.Columns.Add("Mar");
            dt.Columns.Add("Apr");
            dt.Columns.Add("May");
            dt.Columns.Add("June");
            dt.Columns.Add("July");
            dt.Columns.Add("Aug");
            dt.Columns.Add("Sept");
            dt.Columns.Add("Oct");
            dt.Columns.Add("Nov");
            dt.Columns.Add("Dec");
            dt.Columns.Add("YearVal");
            #endregion

            Hashtable STCDList = new Hashtable();

            int index = 0;
            foreach (var row in list)
            {
                string STCD = row.STCD;
                string STNM = row.STNM;

                DateTime time = row.IDTM.AddMonths(-1);

                int IDT = (time.Month - 1) % 12;
                string Year = time.ToString("yyyy");

                //径流量
                if (!STCDList.ContainsKey(STCD))
                {
                    row_new = dt.NewRow();
                    row_new["STNM"] = STNM.TrimEnd();
                    row_new["Year"] = Year;

                    dt.Rows.Add(row_new);
                    STCDList.Add(STCD, index);
                    index++;
                }
                else
                {
                    row_new = dt.Rows[Convert.ToInt32(STCDList[STCD])];
                }
                row_new[IDT + 2] = Convert.ToDouble(row.ACCP).QFormat(4);
            }

            foreach (var item in dt.Select())
            {
                double yearVal = 0;
                for (int i = 0; i < 12; i++)
                {
                    var obj = item[i + 2].ToDoubleOrNull();
                    yearVal += (obj == null ? 0 : obj.Value);
                }
                item["YearVal"] = yearVal.ToString("F2");
            }

            return dt;
        }

        #region 历史同期降水量旬月对比分析
        /// <summary>历史同期降水量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内旬月均值</returns>
        public IEnumerable<dynamic> GetMonthComparativeData(string STCD, string type, string sdate, string edate,  string year, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-01 08:00");
            var etime = Convert.ToDateTime(edate).AddMonths(1).ToString("yyyy-MM-01 08:00");
            var sdate_history = year + Convert.ToDateTime(sdate).ToString("-MM-01 08:00");
            var edate_history = year + Convert.ToDateTime(edate).AddMonths(1).ToString("-MM-01 08:00");
            var result = repository.GetMonthComparativeData(STCD, type, stime, etime, sdate_history, edate_history);
            return ConvertTableMonth_Comparative(result.real, result.history);
        }
        //历史同期对比表格转置--月
        private IEnumerable<dynamic> ConvertTableMonth_Comparative(IEnumerable<ST_PSTATEntity> list_real, IEnumerable<ST_PSTATEntity> list_history)
        {
            var list_result = new List<dynamic>();
            foreach (var item in list_real)
            {
                var IDTM_Comparative = "";
                double ACCP_Comparative = 0;
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.IDTM.Month == item.IDTM.Month && x.IDTM.Day == item.IDTM.Day);

                if (row_Comparative.Count() > 0)
                {
                    ACCP_Comparative = row_Comparative.FirstOrDefault().ACCP.ToDouble();
                    IDTM_Comparative = row_Comparative.FirstOrDefault().IDTM.AddMonths(-1).ToString("yyyy-MM");
                }
                dynamic row = new
                {
                    STNM = item.STNM,
                    STCD = item.STCD,
                    IDTM = item.IDTM.AddMonths(-1).ToString("yyyy-MM"),
                    ACCP = item.ACCP,
                    IDTM_Comparative = IDTM_Comparative,
                    ACCP_Comparative = ACCP_Comparative,
                };
                list_result.Add(row);
            }
            return list_result;
        }
        /// <summary>历史同期降水量对比分析-旬</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内旬月均值</returns>
        public IEnumerable<dynamic> GetTenComparativeData(string STCD, string type, string sdate, string edate, string sten, string eten, string year, ref string datasrc)
        {
            var stime = "";
            var etime = "";
            var sdate_history = "";
            var edate_history = "";
            if (sten == "01")
            {
                stime = Convert.ToDateTime(sdate).AddMonths(1).ToString("yyyy-MM-dd 08:00");
                sdate_history = year + Convert.ToDateTime(sdate).AddMonths(1).ToString("-MM-dd 08:00");
            }
            else
            {
                stime = Convert.ToDateTime(sdate + "-" + sten).ToString("yyyy-MM-dd 08:00");
                sdate_history = year + Convert.ToDateTime(sdate + "-" + sten).ToString("-MM-dd 08:00");
            }
            if (eten == "01")
            {
                etime = Convert.ToDateTime(edate).AddMonths(1).ToString("yyyy-MM-01 08:00");
                edate_history = year + Convert.ToDateTime(edate).AddMonths(1).ToString("-MM-01 08:00");
            }
            else
            {
                etime = Convert.ToDateTime(edate + "-" + eten).ToString("yyyy-MM-dd 08:00");
                edate_history = year + Convert.ToDateTime(edate + "-" + eten).ToString("-MM-dd 08:00");
            }
            var result = repository.GetMonthComparativeData(STCD, type, stime, etime, sdate_history, edate_history);
            return ConvertTableTen_Comparative(result.real, result.history);
        }
        //历史同期对比表格转置--旬
        private IEnumerable<dynamic> ConvertTableTen_Comparative(IEnumerable<ST_PSTATEntity> list_real, IEnumerable<ST_PSTATEntity> list_history)
        {
            var list_result = new List<dynamic>();
            foreach (var item in list_real)
            {
                var IDTM_Comparative = "";
                double ACCP_Comparative = 0;
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.IDTM.Month == item.IDTM.Month && x.IDTM.Day == item.IDTM.Day);

                if (row_Comparative.Count() > 0)
                {
                    ACCP_Comparative = row_Comparative.FirstOrDefault().ACCP.ToDouble();
                    IDTM_Comparative = row_Comparative.FirstOrDefault().IDTM.ToString("MM-dd");
                }
                dynamic row = new
                {
                    STNM = item.STNM,
                    STCD = item.STCD,
                    IDTM = item.IDTM.ToString("MM-dd"),
                    ACCP = item.ACCP,
                    IDTM_Comparative = IDTM_Comparative,
                    ACCP_Comparative = ACCP_Comparative,
                };
                list_result.Add(row);
            }
            return list_result;
        }
        #endregion

    }
}
