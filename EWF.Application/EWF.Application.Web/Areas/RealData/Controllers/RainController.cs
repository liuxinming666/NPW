using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Util;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading;
using System.Collections;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    [Authorize]
    public class RainController : EWFBaseController
    {
        private IRainService service;
        private IStationService staService;
         
        public RainController(IRainService _service, IStationService _staService)
        {
            service = _service;
            staService = _staService;            
        }
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["DD"] = "tt";
            return View();
        }
        /// <summary>
        /// 获取每个站的最新雨情
        /// CCG
        /// </summary>
        /// <returns></returns>
        public IActionResult RainNew()
        {
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var pageObj = service.GetLatestRain(type, addvcd);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj
            };
            return Content(data.ToJson());
        }
        #region 多日雨情
        /// <summary>
        /// 多日雨情页面
        /// </summary>
        /// <returns></returns>
        public IActionResult RainQueryDays()
        {
            ViewData["DD"] = "tt";
            return View();
        }
        /// <summary>
        /// 多日雨情表格不分页
        /// </summary>
        /// <param name="stcds">站码以逗号分隔</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IActionResult GetRainData(string stcds, string startDate, string endDate)
        {
            var result = "";
            var rowdata = "";
            var total = 0;

            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            if (DateTime.Compare(Convert.ToDateTime(endDate + " 8:0").AddMonths(-1), Convert.ToDateTime(startDate + " 8:0")) >= 0)
            {
                result = "{\"rows\":" + rowdata + ",\"total\":" + total + ",\"error\":\"间隔时间不能大于一个月\"}";
            }
            else
            {
                DataTable dt = service.GetRainData(stcds, startDate, endDate,type,addvcd);
                if(dt.Rows.Count > 0)
                    result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + "}";
                else
                    result = "{\"rows\":,\"total\":" + dt.Rows.Count + "}";
            }
            return Content(result);
            //DataTable dtTitle = service.RainDaysTitle(startDate, endDate);
            //string strTitle = "";// = string.Join(",", dtTitle);
            //for (int i = 0; i < dtTitle.Columns.Count; i++)
            //{
            //    strTitle += dtTitle.Columns[i] + ",";
            //}
            //if (strTitle.Length > 0)
            //    strTitle = strTitle.Substring(0, strTitle.Length - 1);
        }
        #endregion

        #region 时段雨量查询
        /// <summary>
        /// 时段雨量查询
        /// </summary>
        /// <returns></returns>
        public IActionResult RainPeriod()
        {
            ViewData["DD"] = "tt";
            return View();
        }

        public IActionResult GetRainDataPeriod(int page, int rows, string stcds, string startDate, string endDate)
        {
            var total = 0;

            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var pageObj = service.GetRainDataPeriod(page, rows, stcds, startDate, endDate, type, addvcd);
            
            //获取区域面平均
            var areaAvg = service.GetAreaAvg(startDate, endDate, addvcd, 0);
            var arr = new ArrayList();
            dynamic dyAreaAvg = new System.Dynamic.ExpandoObject();
            dyAreaAvg.RVNM = "全流域";
            dyAreaAvg.STNM = "面平均";
            dyAreaAvg.VALUE = areaAvg;
            arr.Add(dyAreaAvg);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items,
                footer = arr
            };
            return Content(data.ToJson());
            
        }
        public IActionResult GetRainDataPeriodNew(string stcds, string startDate, string endDate,double rainValue,double rainValue2)
        {
            string addvcd = ""; 
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            DataTable dt = service.GetRainDataPeriod(stcds, startDate, endDate, rainValue, rainValue2, addvcd);
            var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count+ "}";
            return Content(result);
        }
        //public IActionResult RainPeriodDetail(string stcd, string startDate, string endDate)
        //{


        //    return View();
        //}
        public IActionResult RainPeriodDetail(string stcd,string sdate, string edate)
        {

            if (String.IsNullOrEmpty(sdate))
                sdate = Convert.ToDateTime(edate).AddDays(-3).ToString("yyyy-MM-dd HH:mm");
            if (String.IsNullOrEmpty(edate))
                edate = Convert.ToDateTime(sdate).AddDays(3).ToString("yyyy-MM-dd HH:mm");
            ViewData["stcd"] = stcd;
            ViewData["sdate"] = sdate;
            ViewData["edate"] = edate;
            return View();
        }
        /// <summary>
        /// 获取某个测站起止时间内所有时段雨量，即数据库记录
        /// </summary>
        /// <param name="stcd">测站编码</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IActionResult GetRainDataPeriodDetail(string stcd, string startDate, string endDate)
        {
            var pageObj = service.GetRainDataPeriodDetail( stcd, startDate, endDate);
            var data = new
            {
                total = pageObj.Count,
                rows = pageObj
            };
            return Content(data.ToJson());
        }

        #endregion

        #region 雨情分析

        /// <summary>
        /// 返回等值面等值线分析所需要的数据
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="type">统计雨量类型，0-时段雨量，1-日雨量，默认值为1</param>
        /// <param name="addvcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GetSumRain(string startDate, string endDate, int type = 1)
        {
            int areatype = 0;
            string addvcd = "";
            areatype = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            List<dynamic> listData = service.GetSumRain(startDate,endDate,areatype,addvcd,type);

            return Content(listData.ToJson());
        }

        /// <summary>
        /// 返回雨量等值面geojson格式数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="type"></param>
        /// <param name="featureType">线面类型，0-返回等值线要素，1-返回等值面要素，默认值为1</param>
        /// <returns></returns>
        public async Task<IActionResult> GetRainAnalysisData(string startDate,string endDate,int type=1,int featureType=1)
        {
            int areatype = 0;
            string addvcd = "";
            areatype = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var result=await service.GetRainAnalysisData(startDate,endDate, areatype, addvcd, type, featureType);
            
            return Content(result);
        }

        /// <summary>
        /// 雨情分析条件页面
        /// add by SUN
        /// Date:2019-05-29 14:00
        /// </summary>
        /// <returns></returns>
        public IActionResult RainAnalysisCondition()
        {
            return View();
        }
        #endregion

        #region 旬月雨量
        /// <summary>
        /// 旬月雨量查询
        /// </summary>
        /// <returns></returns>
        public IActionResult RainQueryMonth()
        {
            ViewData["DD"] = "tt";
            return View();
        }
        /// <summary>
        /// 旬月雨情表格不分页
        /// </summary>
        /// <param name="stcds">站码以逗号分隔</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IActionResult GetRainMonthData(string stcds, string startDate, string endDate)
        {
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            DataTable dt = service.GetRainMonthData(stcds, startDate, endDate,type, addvcd);
            //获取区域面平均
            var areaAvg = service.GetAreaAvg(startDate + "-1 0:0:0",DateTime.Parse(endDate + "-1 0:0:0").AddMonths(1).ToString(), addvcd, 1);
            var arr = new ArrayList();
            dynamic dyAreaAvg = new System.Dynamic.ExpandoObject();
            dyAreaAvg.站名 = "面平均"; 
            dyAreaAvg.上旬 = areaAvg;
            arr.Add(dyAreaAvg);
            var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + ",\"footer\":"+arr.ToJson()+"}";
            return Content(result); 
           
        }
        #endregion
        
        #region 极值滑动统计

        public IActionResult Raininess()
        {
            return View();
        }

        /// <summary>
        /// 获取极值滑动统计
        /// add by SUN
        /// Date:2019-05-30 11:00
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public IActionResult GetRaininess(string startDate,string endDate)
        {
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            List<dynamic> listData = service.GetRaininess(startDate, endDate, type, addvcd);
            return Content(listData.ToJson());
        }

        #endregion

        #region 首页降雨信息
        /// <summary>
        /// 返回最新雨情数据,昨日降雨
        /// </summary>
        /// <returns></returns>
        public IActionResult GetLatestRainNewData(int stype)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetLatestRainData(type == "" ? 0 : Convert.ToInt32(type), addvcd, stype);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj

            };
            return Content(data.ToJson());
        }

        //
        public IActionResult RainChart(string stcd, string edate, string stype)
        {
            ViewData["stcd"] = stcd;
            ViewData["edate"] = edate;
            ViewData["stype"] = stype;
            return View();
        }
        /// <summary>
        /// 获取某个测站起止时间内所有时段雨量，即数据库记录
        /// </summary>
        /// <param name="stcd">测站编码</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public IActionResult GetRainChartData(string stcd, string endDate, string stype)
        {
            var pageObj = service.GetRainChartData(stcd, endDate, stype);
            var data = new
            {
                total = pageObj.Count,
                rows = pageObj
            };
            return Content(data.ToJson());
        }

        //首页降雨统计
        public IActionResult GetLatestRainStaticData()
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetLatestRainStaticData(type == "" ? 0 : Convert.ToInt32(type), addvcd);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj

            };
            return Content(data.ToJson());
        }
        //降雨统计明细
        public IActionResult RainStaticDetail(string tm, string slevel)
        {
            ViewData["tm"] = tm;
            ViewData["slevel"] = slevel;
            return View();
        }
        public IActionResult GetRainStaticDetailData(string tm, string slevel)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetRainStaticDetailData(slevel, tm, type == "" ? 0 : Convert.ToInt32(type), addvcd);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj

            };
            return Content(data.ToJson());
        }
        #endregion


    }
}
