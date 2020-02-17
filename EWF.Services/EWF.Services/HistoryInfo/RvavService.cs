/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/28 15:57:53
 * 描  述：RvavService
 * *********************************************/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EWF.Services.HistoryInfo
{
    public class RvavService : IRvavService
    {
        private IRvavRepository repository;
        public RvavService(IRvavRepository _epository)
        {
            repository = _epository;
        }

        public Page<ST_RVAVEntity> GetPageData(int pageIndex, int pageSize, string STCD, string STTDRCD, string addvcd, string type)
        {
            return repository.GetPageData(pageIndex, pageSize, STCD, STTDRCD, addvcd, type);
        }

        public int Count(string STCD)
        {
            var result = repository.Count(new { STCD = STCD });
            return result;
        }

        public ST_RVAVEntity Get(int ID)
        {
            var result = repository.Get(ID);
            return result;
        }

        public bool Insert(ST_RVAVEntity entity)
        {
            var result = repository.Insert<string>(entity);
            if (!result.IsEmpty())
            {
                return true; ;
            }
            return false;
        }

        public bool Update(ST_RVAVEntity entity)
        {

            var result = repository.UpdateIgnoreNull(entity);
            return result;
        }

        public bool Delete(ST_RVAVEntity entity)
        {
            var result = repository.Delete(entity);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteList(object whereConditions)
        {
            var result = repository.DeleteList(whereConditions);
            if (result > 0)
            {
                return true;
            }
            return false;
        }


        public IEnumerable<ST_RVAVEntity> GetDayData(string STCD, string Addvcd, string type, string sdate, string edate, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 07:59");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:01");

            var list_history = repository.GetDayData(STCD,Addvcd,type, stime, etime);

            //历史库有数据
            if (list_history.Count() > 0)
            {
                datasrc = "read";
                return list_history;
            }

            var list_real = repository.GetDayData_River(STCD, Addvcd, type, stime, etime);

            return DayTableConvert(list_real);
        }
        
        public IEnumerable<dynamic> GetMonthData(string STCD, string Addvcd, string type, string smonth, string emonth, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(smonth + "-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(emonth + "-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");

            var list = repository.GetMonthData(STCD, Addvcd, type, sdate, edate);
            
            var list_result = MonthTableConvert(list);

            return list_result;
        }

        public DataTable GeYearData(string STCD, string Addvcd,string type, string year, ref string datasrc)
        {
            var sdate = Convert.ToDateTime(year + "-01-01").ToString("yyyy-MM-dd 08:00");
            var edate = Convert.ToDateTime(year + "-12-01").AddMonths(1).ToString("yyyy-MM-dd 08:00");


            var list = repository.GeYearData(STCD, Addvcd, type, sdate, edate);

            var list_result = ConvertTableYear(list);

            return list_result;
        }

        public IEnumerable<dynamic> GetData_Comparative(string STCD, string Addvcd, string type, string avgType, string sdate, string edate, string year)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 07:59");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:01");
            var sdate_history = year + Convert.ToDateTime(sdate).ToString("-MM-dd 07:59");
            var edate_history = year + Convert.ToDateTime(edate).ToString("-MM-dd 08:01");

            var result = repository.GetData_Comparative(STCD,Addvcd,type, avgType, stime, etime, sdate_history, edate_history);

           

            return ConvertTableDay_Comparative(result.real, result.history);
        }
        

        #region 数据处理组装 -查询统计
        private IEnumerable<ST_RVAVEntity> DayTableConvert(IEnumerable<ST_RIVEREntity> list)
        {
            List<ST_RVAVEntity> result=new  List<ST_RVAVEntity>();


            return result;
        }
        
        //表格转置-旬月均值
        private IEnumerable<dynamic> MonthTableConvert(IEnumerable<ST_RVAVEntity> list)
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
                        row_new.ZSXun = item.AVZ;
                        row_new.QSXun = item.AVQ;
                        break;
                    case 2:
                        row_new.ZZXun = item.AVZ;
                        row_new.QZXun = item.AVQ;
                        break;
                    case 0:
                        row_new.ZXXun = item.AVZ;
                        row_new.QXXun = item.AVQ;
                        break;
                    case 4:
                        row_new.ZMon = item.AVZ;
                        row_new.QMon = item.AVQ;
                        break;
                    //default:
                    //    throw new Exception("索引超出界限");
                }
            }

            return list_result;
        }

        //表格转置-年累计
        private DataTable ConvertTableYear(IEnumerable<ST_RVAVEntity> list)
        {
            #region
            DataTable dt = new DataTable();
            DataRow row_new_Z;
            DataRow row_new_Q;
            dt.Columns.Add("STNM");
            dt.Columns.Add("CountType");
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

                #region 水位
                if (!STCDList.ContainsKey(STCD + "-AVZ"))
                {
                    row_new_Z = dt.NewRow();
                    row_new_Z["STNM"] = STNM.TrimEnd();
                    row_new_Z["Year"] = Year;
                    row_new_Z["CountType"] = "水位";

                    dt.Rows.Add(row_new_Z);
                    STCDList.Add(STCD + "-AVZ", index);
                    index++;
                }
                else
                {
                    row_new_Z = dt.Rows[Convert.ToInt32(STCDList[STCD + "-AVZ"])];
                }
                row_new_Z[IDT + 3] = row.AVZ.ToString();
                #endregion

                #region 流量
                if (!STCDList.ContainsKey(STCD + "-AVQ"))
                {
                    row_new_Q = dt.NewRow();
                    row_new_Q["STNM"] = STNM.TrimEnd();
                    row_new_Q["Year"] = Year;
                    row_new_Q["CountType"] = "流量";

                    dt.Rows.Add(row_new_Q);
                    STCDList.Add(STCD + "-AVQ", index);
                    index++;
                }
                else
                {
                    row_new_Q = dt.Rows[Convert.ToInt32(STCDList[STCD + "-AVQ"])];
                }
                row_new_Q[IDT + 3] = Convert.ToDouble(row.AVQ).QFormat(4);
                #endregion
                
            }

            foreach (var item in dt.Select())
            {
                decimal yearVal = 0;
                for (int i = 0; i < 12; i++)
                {
                    var temp=item[i + 3].ToString();

                    yearVal += (temp.IsEmpty() ? 0 : Convert.ToDecimal(temp));
                }
                if (item["CountType"].ToString() == "水位")
                    item["YearVal"] = (yearVal / 12).ToString("F2");
                else
                    item["YearVal"] = Convert.ToDouble(yearVal / 12).QFormat();
            }

            return dt;
        }
        #endregion

        #region 数据处理组装 -对比分析
        //水情多日均值历史同期对比分析
        private IEnumerable<dynamic> ConvertTableDay_Comparative(IEnumerable<ST_RVAVEntity> list_real, IEnumerable<ST_RVAVEntity> list_history)
        {
            var list_result = new List<dynamic>();
            
            foreach (var item in list_real)
            {
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.IDTM.Month == item.IDTM.Month && x.IDTM.Day == item.IDTM.Day);

                double? AVQ_Comparative = null;
                double? AVZ_Comparative = null;

                if (row_Comparative.Count() > 0)
                {
                    AVQ_Comparative = row_Comparative.FirstOrDefault().AVQ;
                    AVZ_Comparative = row_Comparative.FirstOrDefault().AVZ;
                }
                
                dynamic row = new
                {
                    STCD=item.STCD,
                    STNM = item.STNM,
                    IDTM = item.IDTM,
                    AVQ = item.AVQ,
                    AVZ = item.AVZ,
                    AVQ_Comparative = AVQ_Comparative,
                    AVZ_Comparative = AVZ_Comparative,
                };
                list_result.Add(row);
            }
            return list_result;
        }
        #endregion
    }
}
