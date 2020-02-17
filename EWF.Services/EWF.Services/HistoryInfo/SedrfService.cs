/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 17:26:15
 * 描  述：SedrfService
 * *********************************************/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EWF.Services
{
    public class SedrfService : ISedrfService
    {
        private ISedrfRepository repository;
        private IRvavRepository repository_rvav;
        public SedrfService(ISedrfRepository _epository, IRvavRepository _repository_rvav)
        {
            repository = _epository;
            repository_rvav = _repository_rvav;
        }

        public IEnumerable<ST_SEDRFDayViewModel> GetDayData(string STCD, string Addvcd, string type, string sdate, string edate, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 07:59");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:01");

            var list_history = repository.GetDayData(STCD, Addvcd, type, stime, etime);


            //历史库有数据
            if (list_history.Count() > 0)
            {
                return ConvertTableDay(list_history);
            }
            var list_rvav = repository_rvav.GetDayData(STCD,Addvcd,type, stime, etime);
            if (list_rvav.Count() > 0)
            {
                return ConvertTableDayByRVAV(list_rvav);
            }
            var list_real = repository_rvav.GetDayData_River(STCD, Addvcd, type, stime, etime);
            datasrc = "real";
            return ConvertTableDayByRiver(list_real);
        }

        public IEnumerable<dynamic> GetMonthData(string STCD, string Addvcd, string type, string smonth, string emonth, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(smonth + "-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(emonth + "-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");

            var list = repository.GetMonthData(STCD, Addvcd, type, sdate, edate);


            var list_result = ConvertTableMonth(list);

            return list_result;
        }

        public DataTable GeYearData(string STCD, string Addvcd, string type, string year, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(year + "-01-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(year + "-12-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");


            var list = repository.GeYearData(STCD, Addvcd, type, sdate, edate);

            var list_result = ConvertTableYear(list);

            return list_result;
        }


        public IEnumerable<dynamic> GetDayData_Comparative(string STCD, string Addvcd, string type, string sdate, string edate, string year, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 07:59");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:01");
            var sdate_history = year  + Convert.ToDateTime(sdate).ToString("-MM-dd 07:59");
            var edate_history =year  + Convert.ToDateTime(edate).ToString("-MM-dd 08:01"); 

            var result = repository.GetDayData_Comparative(STCD,Addvcd,type, stime, etime, sdate_history, edate_history);
            
            if (result.real == null || result.real.Count == 0)
            {
                datasrc = "数据来源水情均值表，径流量由流量计算";
                var result_rvav=repository_rvav.GetDayData_Comparative(STCD, Addvcd, type, stime, etime, sdate_history, edate_history);
                return ConvertTableDay_Comparative_RVAV(result_rvav.real, result_rvav.history);
            }
            
            return ConvertTableDay_Comparative(result.real, result.history);
        }


        #region 数据处理组装 -查询统计
        //表格转置-多日累计
        private IEnumerable<ST_SEDRFDayViewModel> ConvertTableDay(IEnumerable<ST_SEDRFEntity> list)
        {
            var list_result = new List<ST_SEDRFDayViewModel>();

            foreach (var item in list)
            {
                var row = new ST_SEDRFDayViewModel
                {
                    STCD = item.STCD,
                    STNM = item.STNM.Trim(),
                    IDTM = item.IDTM,
                    WRNF = item.WRNF,
                    STW  = item.STW
                };
                list_result.Add(row);
            }
            return list_result;
        }
        private IEnumerable<ST_SEDRFDayViewModel> ConvertTableDayByRVAV(IEnumerable<ST_RVAVEntity> list)
        {
            var list_result = new List<ST_SEDRFDayViewModel>();

            foreach (var item in list)
            {
                var wrnt  =  (item.AVQ * 24 * 60 * 60) / (1000 * 1000);
                var row   = new ST_SEDRFDayViewModel
                {
                    STCD = item.STCD,
                    STNM = item.STNM.Trim(),
                    IDTM = item.IDTM,
                    WRNF = wrnt.SafeValue().QFormat(4).ToDouble(),
                    STW  = null
                };
                list_result.Add(row);
            }
            return list_result;
        }
        private IEnumerable<ST_SEDRFDayViewModel> ConvertTableDayByRiver(IEnumerable<ST_RIVEREntity> list)
        {
            var list_result = new List<ST_SEDRFDayViewModel>();

            foreach (var item in list)
            {
                var wrnf=  (item.Q * 24 * 60 * 60) / (1000 * 1000);
                var row   = new ST_SEDRFDayViewModel
                {
                    STCD = item.STCD,
                    STNM = item.STNM.Trim(),
                    IDTM = item.TM,
                    WRNF =wrnf.SafeValue().QFormat(4).ToDouble(),
                    STW  = null
                };
                list_result.Add(row);
            }
            return list_result;
        }
        
        //表格转置-旬月累计
        private IEnumerable<dynamic> ConvertTableMonth(IEnumerable<ST_SEDRFEntity> list)
        {
            var list_result = new List<dynamic>();

            dynamic row_new = new ExpandoObject();
            Hashtable STCDList = new Hashtable();

            int index = 0;
            foreach (var item in list)
            {
                string STCD = item.STCD;
                DateTime time = item.IDTM;
                int IDT = time.Day / 10;
                if (item.STTDRCD == "5")
                {
                    time = time.AddMonths(-1);
                    IDT = 4;
                }
                if (item.STTDRCD == "4" && IDT == 0)
                {
                    time = time.AddMonths(-1);
                }
                string Month = time.ToString("yy-M");
                var MapKey = STCD + "-" + Month;
                if (!STCDList.ContainsKey(MapKey))
                {
                    row_new = new ExpandoObject();

                    row_new.STNM = item.STNM.Trim();
                    row_new.Month = Month;

                    double? tempVal = null;
                    row_new.WSXun = tempVal;
                    row_new.SSXun = tempVal;
                    row_new.WZXun = tempVal;
                    row_new.SZXun = tempVal;
                    row_new.WXXun = tempVal;
                    row_new.SXXun = tempVal;
                    row_new.WMonSum = tempVal;
                    row_new.SMonSum = tempVal;


                    list_result.Add(row_new);
                    STCDList.Add(MapKey, index);
                    index++;
                }
                else
                {
                    row_new = list_result[Convert.ToInt32(STCDList[MapKey])];
                }

                //赋值
                switch (IDT)
                {
                    case 1:
                        row_new.WSXun = item.WRNF==null?null: item.WRNF.Value.QFormat(4).ToDoubleOrNull();
                        row_new.SSXun = item.STW == null ? null : item.STW.Value.QFormat(4).ToDoubleOrNull();
                        break;
                    case 2:
                        row_new.WZXun = item.WRNF == null ? null : item.WRNF.Value.QFormat(4).ToDoubleOrNull();
                        row_new.SZXun = item.STW == null ? null : item.STW.Value.QFormat(4).ToDoubleOrNull();
                        break;
                    case 0:
                        row_new.WXXun = item.WRNF == null ? null : item.WRNF.Value.QFormat(4).ToDoubleOrNull();
                        row_new.SXXun = item.STW == null ? null : item.STW.Value.QFormat(4).ToDoubleOrNull();
                        break;
                    case 4:
                        row_new.WMonSum = item.WRNF == null ? null : item.WRNF.Value.QFormat(4).ToDoubleOrNull();
                        row_new.SMonSum = item.STW == null ? null : item.STW.Value.QFormat(4).ToDoubleOrNull();
                        break;
                    default:
                        break;
                }
            }

            return list_result;
        }

        //表格转置-年累计
        private DataTable ConvertTableYear(IEnumerable<ST_SEDRFEntity> list)
        {
            #region
            DataTable dt = new DataTable();
          
            dt.Columns.Add("STNM");
            dt.Columns.Add("Year");
            dt.Columns.Add("CountType");
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

            DataRow row_new;
            DataRow row_new_s;
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
                if (!STCDList.ContainsKey(STCD+"-W"))
                {
                    row_new = dt.NewRow();
                    row_new["STNM"] = STNM.TrimEnd();
                    row_new["Year"] = Year;
                    row_new["CountType"] = "径流量";

                    dt.Rows.Add(row_new);
                    STCDList.Add(STCD + "-W", index);
                    index++;
                }
                else
                {
                    row_new = dt.Rows[Convert.ToInt32(STCDList[STCD + "-W"])];
                }
                row_new[IDT + 3] = Convert.ToDouble(row.WRNF).QFormat(4);


                //输沙量
                if (!STCDList.ContainsKey(STCD + "-S"))
                {
                    row_new_s = dt.NewRow();
                    row_new_s["STNM"] = STNM.TrimEnd();
                    row_new_s["Year"] = Year;
                    row_new_s["CountType"] = "输沙量";

                    dt.Rows.Add(row_new_s);
                    STCDList.Add(STCD + "-S", index);
                    index++;
                }
                else
                {
                    row_new_s = dt.Rows[Convert.ToInt32(STCDList[STCD + "-S"])];
                }
                row_new_s[IDT + 3] = Convert.ToDouble(row.STW).QFormat(4);
            }

            foreach (var item in dt.Select())
            {
                double yearVal = 0;
                for (int i = 0; i < 12; i++)
                {
                    var obj = item[i + 2].ToDoubleOrNull();
                    yearVal += (obj == null ? 0 : obj.Value);
                }
                item["YearVal"] = yearVal.QFormat(4);
            }

            return dt;
        }

        #endregion

        #region 数据处理组装 -对比分析
        //表格转置-多日累计
        private IEnumerable<dynamic> ConvertTableDay_Comparative(IEnumerable<ST_SEDRFEntity> list_real, IEnumerable<ST_SEDRFEntity> list_history)
        {
            var list_result = new List<dynamic>();

            double SUMACCP = 0;
            var stcd_current = "";
            foreach (var item in list_real)
            {
                if (stcd_current != item.STCD.Trim())
                {
                    stcd_current = item.STCD.Trim();
                    SUMACCP = 0;
                }
                SUMACCP += item.WRNF.SafeValue();
                dynamic row = new
                {
                    STCD = item.STCD,
                    STNM = item.STNM,
                    IDTM = item.IDTM,
                    STTDRCD = item.STTDRCD,
                    ACCP = item.WRNF.SafeValue().ToString("F1"),
                    SUMACCP = SUMACCP.ToString("F1")
                };
                list_result.Add(row);
            }
            return list_result;
        }

        private IEnumerable<dynamic> ConvertTableDay_Comparative_RVAV(IEnumerable<ST_RVAVEntity> list_real, IEnumerable<ST_RVAVEntity> list_history)
        {
            var list_result = new List<dynamic>();

            double SUMVAL = 0;
            double SUMVAL_Comparative = 0;
            var stcd_current = "";
            foreach (var item in list_real)
            {
                if (stcd_current != item.STCD.Trim())
                {
                    stcd_current = item.STCD.Trim();
                    SUMVAL = 0;
                    SUMVAL_Comparative = 0;
                }

                var row_Comparative=list_history.Where(x=>x.STCD==item.STCD)
                    .Where(x=>x.IDTM.Month==item.IDTM.Month&&x.IDTM.Day==item.IDTM.Day);

                double WRNF_Comparative=0;
                if (row_Comparative.Count() > 0)
                {
                    WRNF_Comparative = row_Comparative.FirstOrDefault()
                        .AVQ.SafeValue() * 24 * 60 * 60 / (1000 * 1000);
                }
                
                var WRNF=item.AVQ.SafeValue() * 24 * 60 * 60/(1000*1000);


                SUMVAL += WRNF;
                SUMVAL_Comparative += WRNF_Comparative;
                
                dynamic row = new
                {
                    STNM=item.STNM,
                    IDTM = item.IDTM,
                    WRNF = WRNF.QFormat(4).ToDouble(),
                    WRNF_Comparative = WRNF_Comparative.QFormat(4).ToDouble(),
                    SUMVAL = SUMVAL.QFormat(4).ToDouble(),
                    SUMVAL_Comparative= SUMVAL_Comparative.QFormat(4).ToDouble(),
                };
                list_result.Add(row);
            }
            return list_result;
        }

        //表格转置-旬月累计
        private IEnumerable<dynamic> ConvertTableMonth_Comparative(IEnumerable<ST_SEDRFEntity> list)
        {
            var list_result = new List<dynamic>();

            dynamic row_new = new ExpandoObject();
            Hashtable STCDList = new Hashtable();

            int index = 0;
            foreach (var item in list)
            {
                string STCD = item.STCD;
                DateTime time = item.IDTM;
                int IDT = time.Day / 10;
                if (item.STTDRCD == "5")
                {
                    time = time.AddMonths(-1);
                    IDT = 4;
                }
                if (item.STTDRCD == "4" && IDT == 0)
                {
                    time = time.AddMonths(-1);
                }
                string Month = time.ToString("yyyy-MM");
                var MapKey = STCD + "-" + Month;
                if (!STCDList.ContainsKey(MapKey))
                {
                    row_new = new ExpandoObject();

                    row_new.STNM = item.STNM.Trim();
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
                switch (IDT)
                {
                    case 1:
                        row_new.SXun = item.WRNF.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 2:
                        row_new.ZXun = item.WRNF.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 0:
                        row_new.XXun = item.WRNF.SafeValue().ToString("F1").ToDouble();
                        break;
                    case 4:
                        row_new.MonSum = item.WRNF.SafeValue().ToString("F1").ToDouble();
                        break;
                    default:
                        throw new Exception("索引超出界限");
                }
            }

            return list_result;
        }

        //表格转置-年累计
        private DataTable ConvertTableYear_Comparative(IEnumerable<ST_SEDRFEntity> list)
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
                row_new[IDT + 2] = Convert.ToDouble(row.WRNF).QFormat(4);
            }

            foreach (var item in dt.Select())
            {
                double yearVal = 0;
                for (int i = 0; i < 12; i++)
                {
                    var obj = item[i + 2].ToDoubleOrNull();
                    yearVal += (obj == null ? 0 : obj.Value);
                }
                item["YearVal"] = yearVal.QFormat(4);
            }

            return dt;
        }

        #endregion
    }
}
