/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:48:11
 * 描  述：RiverService
 * *********************************************/
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWF.Services
{
    public class RainService : IRainService
    {
        private IRainRepository repository;
        DataOption dataOption;
        public RainService(IRainRepository _epository, IOptionsSnapshot<DataOption> _option)
        {
            repository = _epository;
            dataOption = _option.Get("DataOption");
        }

        public List<dynamic> GetLatestRain(int type, string addvcd)
        {
            string eDate = DateTime.Now.ToString();
            string sDate = DateTime.Now.AddDays(-dataOption.Interval).ToString();
            var list = repository.GetLatestRain(sDate, eDate, type, addvcd);
            return list.ToList<dynamic>();
        }
        /// <summary>
        /// 返回雨情信息
        /// add by JinJianPing
        /// Date:2019-05-27 14:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetRainData(string stcds, string startDate, string endDate, int type, string addvcd)
        {
            var list = repository.GetRainData(stcds, startDate, endDate, type, addvcd);
            DataTable dt = RainDaysConvertTable(list, startDate, endDate);
            return dt;
        }
        /// <summary>
        /// add by JinJianPing
        /// 表格转置
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable RainDaysConvertTable(IEnumerable<dynamic> list, string startDate, string endDate)
        {
            #region 表头
            DateTime StartDate = Convert.ToDateTime(startDate);
            DateTime EndDate = Convert.ToDateTime(endDate);

            DataTable dt = new DataTable();

            dt.Columns.Add("站名");
            while (DateTime.Compare(StartDate, EndDate) < 0)
            {
                dt.Columns.Add(StartDate.ToString("M月d日"));
                StartDate = StartDate.AddDays(1);
            }
            dt.Columns.Add(EndDate.ToString("M月d日"));
            dt.Columns.Add("累计");
            #endregion
            Dictionary<string, DataRow> STCDList = new Dictionary<string, DataRow>();

            List<dynamic> templist = list.ToList();
            if (templist.Count > 0)
            {
                foreach (dynamic row in list)
                {
                    string STCD = row.STCD.ToString();
                    string STNM = row.STNM.TrimEnd();
                    string strTime = Convert.ToDateTime(row.TM).ToString("M月d日");

                    #region 日雨量
                    if (!STCDList.ContainsKey(STCD))
                    {
                        DataRow row_wrnf = dt.NewRow();

                        row_wrnf["站名"] = STNM.TrimEnd();
                        row_wrnf[strTime] = row.DYP;
                        row_wrnf["累计"] = row.DYP;
                        dt.Rows.Add(row_wrnf);
                        STCDList.Add(STCD, row_wrnf);

                    }
                    else
                    {
                        STCDList[STCD][strTime] = row.DYP;
                        if (!(row.DYP is System.DBNull || row.DYP is null))
                        {
                            //if (!string.IsNullOrEmpty(row.DYP.ToString()))
                            //{
                            if (!string.IsNullOrEmpty(STCDList[STCD]["累计"].ToString()))
                            {
                                STCDList[STCD]["累计"] = Convert.ToDouble(STCDList[STCD]["累计"].ToString()) + Convert.ToDouble(row.DYP);
                            }
                            else
                            {
                                STCDList[STCD]["累计"] = row.DYP;
                            }
                            //}
                        }
                    }
                    #endregion

                }
            }
            else
            {
                DataRow row_wrnf = dt.NewRow();
                dt.Rows.Add(row_wrnf);
            }
            return dt;
        }
        /// <summary>
        /// add by JinJianPing
        /// 表头
        /// </summary>      
        /// <returns></returns>
        //private DataTable RainDaysTitle(string startDate, string endDate)
        //{
        //    #region 表头
        //    DateTime StartDate = Convert.ToDateTime(startDate);
        //    DateTime EndDate = Convert.ToDateTime(endDate);

        //    DataTable dt = new DataTable();

        //    dt.Columns.Add("站名");
        //    while (DateTime.Compare(StartDate, EndDate) < 0)
        //    {
        //        StartDate = StartDate.AddDays(1);
        //        dt.Columns.Add(StartDate.ToString("M月d日"));
        //    }
        //    dt.Columns.Add("累计");
        //    #endregion
        //    return dt;
        //}

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
        public async Task<string> GetRainAnalysisData(string startDate, string endDate, int areatype, string addvcd, int type = 1, int featureType = 1)
        {
            var list = repository.GetSumRain(startDate, endDate, areatype, addvcd, type);
            var listData = list.ToList();
            listData.Add(new { x = 95.65, y = 42.5, value = 0 });
            listData.Add(new { x = 121.76, y = 31.58, value = 0 });
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(listData);
            if (jsonData == "[]")
            {
                return null;
            }

            MapGIS.GISServiceClient gis = new MapGIS.GISServiceClient();
            MapGIS.JsonPointToRasterRequest r = new MapGIS.JsonPointToRasterRequest(jsonData, addvcd);
            var rr = await gis.JsonPointToRasterAsync(jsonData, addvcd);
            var polygon = await gis.RasterToLineAndPolygon_GeoJSONAsync(rr.JsonPointToRasterResult.ToString(), "RAINYICHUN", addvcd, featureType);
            var result = "";
            if (polygon.RasterToLineAndPolygon_GeoJSONResult.Length > 0)
            {
                result = polygon.RasterToLineAndPolygon_GeoJSONResult[0];
            }

            return result;
        }

        /// <summary>
        /// 返回等值面等值线分析所需要的数据
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <param name="addvcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<dynamic> GetSumRain(string startDate, string endDate, int areatype, string addvcd, int type = 1)
        {
            var list = repository.GetSumRain(startDate, endDate, areatype, addvcd, type);

            return list.ToList<dynamic>();
        }

        public Page<dynamic> GetRainDataPeriod(int page, int rows, string stcds, string startDate, string endDate, int type, string addvcd)
        {
            var list = repository.GetRainDataPeriod(page, rows, stcds, startDate, endDate, type, addvcd);
            return list;
        }
        /// <summary>
        /// 获取区域的面平均雨量，根据权重计算
        /// add by SUN
        /// Date:2019-05-30 13:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="featureType">要素类型（0-时段雨量DRP,1-日雨量DYP)</param>
        /// <returns></returns>
        public double GetAreaAvg(string startDate, string endDate,string adcd, int featureType = 1)
        {
            var result = repository.GetAreaAvg(startDate, endDate, adcd, featureType);
            return result;
        }
        /// <summary>
        /// 返回旬月雨情信息
        /// add by JinJianPing
        /// Date:2019-05-28 11:44
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetRainMonthData(string stcds, string startDate, string endDate, int type, string addvcd)
        {
            var list = repository.GetRainMonthData(stcds, startDate, endDate, type, addvcd);
            DataTable dt = GetMonthConvertTable(list, startDate, endDate);
            return dt;
        }
        public DataTable GetMonthConvertTable(IEnumerable<dynamic> list, string startDate, string endDate)
        {
            DataTable dt = new DataTable();
            #region 表头            
            dt.Columns.Add("站名");
            dt.Columns.Add("时间");
            dt.Columns.Add("上旬");
            dt.Columns.Add("中旬");
            dt.Columns.Add("下旬");
            dt.Columns.Add("合计");
            dt.Columns.Add("全月");
            dt.Columns.Add("差值");
            #endregion
            DateTime StartTime = Convert.ToDateTime(startDate + " -1 0:0");
            DateTime EndTime = Convert.ToDateTime(endDate + " -1 8:0");
            List<dynamic> tempList = list.ToList();
            Dictionary<string, DataRow> STCDList = new Dictionary<string, DataRow>();

            #region 填充表格数据
            if (tempList.Count > 0)
            {
                while (DateTime.Compare(StartTime, EndTime) < 0)
                {
                    string STCD = "";
                    string STNM = "";

                    foreach (dynamic row in list)
                    {
                        STCD = row.STCD.ToString();
                        STNM = row.STNM.TrimEnd();
                        DateTime time = DateTime.Parse(row.IDTM.ToString());

                        string strTime = time.ToString("yyyy年MM月");
                        string strMonth = time.Year + "_" + time.Month;
                        if (Convert.ToDateTime(time.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(endDate + "-01"))
                        {
                            if (!STCDList.ContainsKey(STCD + "_" + strMonth))
                            {

                                DataRow row_wrnf = dt.NewRow();
                                row_wrnf["站名"] = STNM.TrimEnd();
                                row_wrnf["时间"] = strTime;

                                List<dynamic> xunList = tempList.Where(t => t.STTDRCD == "4" && t.STCD == STCD && (Convert.ToDateTime(t.IDTM).ToString("yyyy-MM") == time.ToString("yyyy-MM") || Convert.ToDateTime(t.IDTM).ToString("yyyy-MM") == time.AddMonths(1).ToString("yyyy-MM"))).ToList();//旬列表
                                List<dynamic> monthList = tempList.Where(t => t.STTDRCD == "5" && t.STCD == STCD && Convert.ToDateTime(t.IDTM).ToString("yyyy-MM") == time.AddMonths(1).ToString("yyyy-MM")).ToList();//月列表

                                double sum = 0;//上中下旬合计
                                #region 计算旬
                                if (xunList.Count > 0)
                                {
                                    int IDT = 0;
                                    int month = 0;
                                    foreach (var item in xunList)
                                    {
                                        IDT = (Convert.ToDateTime(item.IDTM).Day - 1) / 10;
                                        month = Convert.ToDateTime(item.IDTM).Month;
                                        if (IDT == 0 && item.IDTM.ToString("yyyy-MM-dd") == Convert.ToDateTime(time).AddMonths(1).ToString("yyyy-MM") + "-01")
                                        {
                                            row_wrnf["下旬"] = item.ACCP;
                                            if (item.ACCP != null)
                                                sum += Convert.ToDouble(item.ACCP);
                                        }
                                        else if (IDT == 1 && time.Month == month)
                                        {
                                            row_wrnf["上旬"] = item.ACCP;
                                            if (item.ACCP != null)
                                                sum += Convert.ToDouble(item.ACCP);
                                        }
                                        else if (IDT == 2 && time.Month == month)
                                        {
                                            row_wrnf["中旬"] = item.ACCP;
                                            if (item.ACCP != null)
                                                sum += Convert.ToDouble(item.ACCP);
                                        }
                                    }
                                    row_wrnf["合计"] = sum;
                                }
                                #endregion

                                #region 读取全月
                                if (monthList.Count > 0)
                                {
                                    if (monthList[0].ACCP != null)
                                        row_wrnf["全月"] = monthList[0].ACCP;
                                    row_wrnf["差值"] = sum - Convert.ToDouble(monthList[0].ACCP);
                                }
                                #endregion
                                
                                dt.Rows.Add(row_wrnf);
                                STCDList.Add(STCD + "_" + strMonth, row_wrnf);
                                sum = 0;
                            }
                        }
                    }
                    StartTime = StartTime.AddMonths(1);
                }
            }
            else
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            #endregion
            return dt;
        }

        /// <summary>
        /// 获取某个测站起止时间内所有时段雨量，即数据库记录
        /// </summary>
        /// <param name="stcd">测站编码</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public List<dynamic> GetRainDataPeriodDetail(string stcd, string startDate, string endDate)
        {
            var list = repository.GetRainDataPeriodDetail(stcd, startDate, endDate);
            return list.ToList<dynamic>();
        }
        /// <summary>
        /// 获取雨强信息
        /// add by SUN
        /// Date:2019-05-30 10:00
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<dynamic> GetRaininess(string startDate, string endDate, int type, string addvcd)
        {

            //1小时雨强
            var listDataOne = repository.GetRaininess(startDate, endDate, type, addvcd, 1).ToList();
            foreach (var dd in listDataOne)
            {
                dd.group = "1小时";
                dd.SPANTM = dd.STM.ToString("yyyy-MM-dd HH:mm") + "至" + dd.ETM.ToString("yyyy-MM-dd HH:mm");
            }
            //3小时雨强
            var listDataThree = repository.GetRaininess(startDate, endDate, type, addvcd, 3);
            foreach (var dd in listDataThree)
            {
                dd.group = "3小时";
                dd.SPANTM = dd.STM.ToString("yyyy-MM-dd HH:mm") + "至" + dd.ETM.ToString("yyyy-MM-dd HH:mm");
            }
            //6小时雨强
            var listDataSix = repository.GetRaininess(startDate, endDate, type, addvcd, 6);
            foreach (var dd in listDataSix)
            {
                dd.group = "6小时";
                dd.SPANTM = dd.STM.ToString("yyyy-MM-dd HH:mm") + "至" + dd.ETM.ToString("yyyy-MM-dd HH:mm");
            }
            listDataOne.AddRange(listDataThree);
            listDataOne.AddRange(listDataSix);
            return listDataOne;
        }
        /// <summary>
        /// 返回首页24小时和昨日降雨数据
        /// </summary>
        /// <param name="areatype">类型：1行政区2流域</param>
        /// <param name="addvcd">区域编码</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <returns></returns>
        public List<dynamic> GetLatestRainData(int areatype, string addvcd, int type = 1)
        {
            string eDate = ""; //DateTime.Now.ToString();
            string sDate = "";// DateTime.Now.AddDays(-1).ToString();
            if (type == 1)
            {
                sDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 08:00";
                eDate = DateTime.Now.ToString("yyyy-MM-dd") + " 08:00";
            }
            else
            {
                sDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:00");
                eDate = DateTime.Now.ToString("yyyy-MM-dd HH:00");
            }
            var list = repository.GetSumRain(sDate, eDate, areatype, addvcd, type);
            return list.ToList<dynamic>();
        }
        /// <summary>
        /// 获取首页雨量柱状图信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="stype">0：时段降雨DRP  1：昨日降雨DYP</param>
        /// <returns></returns>
        public List<dynamic> GetRainChartData(string stcd, string endDate, string stype)
        {
            string sdate = "";
            if (stype == "0")
            {
                sdate = Convert.ToDateTime(endDate).AddHours(-dataOption.Sys24DRP).ToString();
            }
            else
                sdate = Convert.ToDateTime(endDate).AddDays(-dataOption.SysDYP).ToString();
            var list = repository.GetRainChartData(stcd, sdate, endDate, stype);
            return list.ToList<dynamic>();
        }
        /// <summary>
        /// 首页获取最新24小时降雨量统计
        /// </summary>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public List<dynamic> GetLatestRainStaticData(int type, string addvcd)
        {
            string sdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            string edate= DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            var list = repository.GetLatestRainStaticData(sdate, edate, type, addvcd).ToList<dynamic>();
            DataTable dt = new DataTable();
            dt.Columns.Add("SLEVEL", typeof(string));
            dt.Columns.Add("SCOUNT", typeof(int));
            dt.Columns.Add("SSORT", typeof(string));
            dt.Columns.Add("TM", typeof(string));
            string[] arrLevel = { ">=250|5", "100~250|4", "50~100|3", "25~50|2", "10~25|1", "0~10|0" };
            foreach (var item in arrLevel)
            {                
                var listData = list.Find(data => data.SLEVEL == item.Split('|')[0]);
                DataRow dr = dt.NewRow();
                if (listData == null)
                {
                    dr["SLEVEL"] = item.Split('|')[0];
                    dr["SCOUNT"] = 0;
                    dr["TM"] = edate;
                    dr["SSORT"] = item.Split('|')[1];
                }
                else
                {
                    dr["SLEVEL"] = listData.SLEVEL;
                    dr["SCOUNT"] = listData.SCOUNT;
                    dr["TM"] = edate;
                    dr["SSORT"] = listData.SSORT;
                }
                dt.Rows.Add(dr);
            }
            List<dynamic> listLevel = new List<dynamic>();
            listLevel = dt.DtToDynamic();

            return listLevel;
        }
        /// <summary>
        /// 首页获取最新24小时降雨量统计数量明细
        /// </summary>
        /// <param name="slevel">降雨量级别</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">1行政区 2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public List<dynamic> GetRainStaticDetailData(string slevel, string endDate, int type, string addvcd)
        {
            string sdate = Convert.ToDateTime(endDate).AddDays(-1).ToString("yyyy-MM-dd HH:mm");
            var list = repository.GetRainStaticDetailData(slevel, sdate, endDate, type, addvcd);
            return list.ToList<dynamic>();
        }
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
        public DataTable GetRainDataPeriod(string stcds, string startDate, string endDate, double rainValue, double rainValue2, string addvcd)
        {
            DataTable dtRain = repository.GetRainDataPeriod(stcds, startDate, endDate, addvcd);
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("RVNM");
            dtNew.Columns.Add("STCD");
            dtNew.Columns.Add("STNM");
            dtNew.Columns.Add("TM");
            int startHour = DateTime.Parse(startDate).Hour;
            if (startHour % 2 != 0)
            {
                startHour += 1;
            }
            else
            {
                startHour += 2;
            }
            int i_col;
            for(int i = 0; i < 12; i++)
            {
                i_col = (startHour + i * 2) % 24;
                dtNew.Columns.Add(i_col + "时");
            }
            dtNew.Columns.Add("Total");
            dtNew.Columns.Add("DYP");
            dtNew.Columns.Add("WTH");

            Hashtable STList = new Hashtable();
            int colCount = dtNew.Columns.Count;
            double hourspan =double.Parse(dtNew.Columns[colCount - 4].ToString().Replace("时", ""));
            int newrow_index = 0;
            foreach (DataRow row in dtRain.Rows)
            {
                if (row["TM"] is System.DBNull || row["STCD"] is System.DBNull)
                {
                    continue;
                }
                int Val_index = 5;
                DateTime vtempdate = DateTime.Parse(row["TM"].ToString()).AddHours(-hourspan);
                int vtemphour = vtempdate.Hour;
                int vtempminute = vtempdate.Minute;
                int vtempsecond = vtempdate.Second;
                vtemphour = (int)Math.Ceiling((vtemphour + vtempminute * 1.0 / 60) / 2.0);
                if (vtemphour == 0)
                {
                    vtemphour = 12;
                }
                Val_index = vtemphour + 3;

                DateTime idtm = Convert.ToDateTime(row["TM"]);
                if ((idtm.Hour + idtm.Minute * 1.0 / 60) <= hourspan)
                {
                    idtm = idtm.AddDays(-1);
                }
                if (!STList.ContainsKey(row["STCD"].ToString().Trim() + idtm.ToString("MM-dd")))
                {
                    DataRow newrow = dtNew.NewRow();
                    newrow["STCD"] = row["STCD"]; 
                    newrow["STNM"] = row["STNM"];
                    newrow["RVNM"] = row["RVNM"];
                    newrow["TM"] = idtm.ToString("MM-dd");
                    newrow["WTH"] = row["WTH"];
                    if (!(row["DRP"] is System.DBNull))
                    {
                        newrow[Val_index] = row["DRP"];

                        if (idtm.Hour == Convert.ToInt32(dtNew.Columns[Val_index].ColumnName.Replace("时", "")) && row["WTH"].ToString().Trim() == "7" || row["WTH"].ToString().Trim() == "6")
                        {
                            newrow[Val_index] = newrow[Val_index].ToString() + "*";
                        }
                        newrow["Total"] = Convert.ToDouble(row["DRP"]);

                    }
                    if ((idtm.ToString("MM-dd") != Convert.ToDateTime(endDate).ToString("MM-dd")) || Convert.ToDateTime(endDate).Hour > hourspan)
                    {
                        STList.Add(row["STCD"].ToString() + idtm.ToString("MM-dd"), newrow_index++);

                        dtNew.Rows.Add(newrow);
                    }
                }
                else
                {
                    int row_index = Convert.ToInt32(STList[row["STCD"].ToString() + idtm.ToString("MM-dd")]);
                    if (dtNew.Rows[row_index][Val_index] is System.DBNull)
                    {
                        if (!(row["DRP"] is System.DBNull))
                        {
                            dtNew.Rows[row_index][Val_index] = row["DRP"];

                            if (Convert.ToDateTime(row["TM"]).Hour == Convert.ToInt32(dtNew.Columns[Val_index].ColumnName.Replace("时", "")) && row["WTH"].ToString().Trim() == "7" || row["WTH"].ToString().Trim() == "6")
                            {
                                dtNew.Rows[row_index][Val_index] = row["DRP"].ToString() + "*";
                            }
                             
                        }
                    }
                    else
                    {
                        if (!(row["DRP"] is System.DBNull))
                        {
                            double dValue = 0;
                            if (dtNew.Rows[row_index][Val_index].ToString().Trim() != "")
                            {
                                dValue = Convert.ToDouble(dtNew.Rows[row_index][Val_index].ToString().Replace("*", ""));
                            }
                            dtNew.Rows[row_index][Val_index] = Convert.ToDouble(dValue) + Convert.ToDouble(row["DRP"]);

                            if (idtm.Hour == Convert.ToInt32(dtNew.Columns[Val_index].ColumnName.Replace("时", "")) && row["WTH"].ToString().Trim() == "7" || row["WTH"].ToString().Trim() == "6")
                            {
                                dtNew.Rows[row_index][Val_index] = Convert.ToDouble(dValue) + Convert.ToDouble(row["DRP"]) + "*";
                            }
                             
                        }
                    }
                    if (!(row["DRP"] is System.DBNull) && !(dtNew.Rows[row_index]["Total"] is System.DBNull))
                    {

                        dtNew.Rows[row_index]["Total"] = Convert.ToDouble(dtNew.Rows[row_index]["Total"]) + Convert.ToDouble(row["DRP"]);
                    }
                }
            }

            for (int ij = 0; ij < dtNew.Rows.Count; ij++)
            {
                for (int ci = 4; ci < dtNew.Columns.Count; ci++)
                {
                    string vresult = "";
                    string vvv = dtNew.Rows[ij][ci].ToString();
                    double rain = -1.0;
                    bool vflag = false;
                    bool israin = false;
                    if (vvv != "")
                    {
                        try
                        {
                            if (vvv.IndexOf("*") != -1)
                            {
                                rain = Convert.ToDouble(vvv.Replace("*", ""));
                                israin = true;
                            }
                            else
                            {
                                rain = Convert.ToDouble(vvv);
                            }
                            vflag = true;
                        }
                        catch (Exception eee)
                        {
                            vflag = false;
                        }

                    }

                    if (vflag)
                    {
                        string vwth = "", vbfs = "";

                        try
                        {
                            vwth = dtNew.Rows[ij]["WTH"].ToString();
                        }
                        catch
                        {
                            ;
                        }

                        vbfs = " style = 'background-color:#78FFCB' ";


                        if (rain > rainValue)
                        {
                            if (dtNew.Columns[ci].ColumnName != "Total")
                            {
                                vresult = "<font color='#ff00ff' " + vbfs + ">" + rain.ToString("f1") + "</font>";
                            }
                            else
                            {
                                vresult = "<font color='#ff00ff'>" + rain.ToString("f1") + "</font>";
                            }
                        }
                        if (rain > rainValue2)
                        {
                            if (dtNew.Columns[ci].ColumnName != "Total")
                            {
                                vresult = "<font color='#ff0000' " + vbfs + ">" + rain.ToString("f1") + "</font>";
                            }
                            else
                            {
                                vresult = "<font color='#ff0000'>" + rain.ToString("f1") + "</font>";
                            }
                        }
                        else if (rain <= rainValue)
                        {
                            if (israin)
                            {


                                if (Math.Abs(rain - 0.0) < 1e-6)
                                {
                                    if (dtNew.Columns[ci].ColumnName != "Total")
                                    {
                                        vresult = "<font color='#b7b7b7' " + vbfs + ">" + rain.ToString("f1") + "</font>";
                                    }
                                    else
                                    {
                                        vresult = "<font color='#b7b7b7'>" + rain.ToString("f1") + "</font>";
                                    }
                                }
                                else
                                {
                                    if (dtNew.Columns[ci].ColumnName != "Total")
                                    {
                                        vresult = "<font  " + vbfs + ">" + rain.ToString("f1") + "</font>";
                                    }
                                }


                            }
                            else
                            {
                                if (Math.Abs(rain - 0.0) < 1e-6)
                                {
                                    vresult = "<font color='#b7b7b7'>" + rain.ToString("f1") + "</font>";
                                }
                                else
                                {
                                    vresult = rain.ToString("f1");
                                }
                            }
                        }

                        dtNew.Rows[ij][ci] = vresult;
                    }
                }
               
            }
            return dtNew;
        }
    }
}
