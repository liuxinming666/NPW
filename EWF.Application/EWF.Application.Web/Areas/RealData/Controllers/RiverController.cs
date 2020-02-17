using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using System.Data;
using EWF.IServices;
using System.Text;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    //[Area("RealData")]
    [Authorize]
    public class RiverController : EWFBaseController
    {
        //URL:/RealData/River/GetRealData

        private IRiverService service;
        private IStationService staService;
        private IRainService rainService;
        public RiverController(IRiverService _service,IStationService _staService, IRainService _rainService)
        {
            service = _service;
            staService = _staService;
            rainService = _rainService;
        }

        public IActionResult RealData()
        {
            return View();
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="STNM"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRealData(int page, int rows, string STNM)
        {
            var pageObj = service.GetReadData(page, rows, STNM);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items

            };
            return Content(data.ToJson());
        }


        #region 最新水情

        public IActionResult LatestRiver()
        {
            return View();
        }

        /// <summary>
        /// 返回最新水情数据
        /// add by SUN
        /// Date:2019-05-23 14:00
        /// </summary>
        /// <returns></returns>
        public IActionResult GetLatestRiverData()
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetLatestRiverData(addvcd,type);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj

            };
            return Content(data.ToJson());
        }
        #endregion


        #region 河道水情查询

        public IActionResult RiverData()
        {
            return View();
        }

        public IActionResult GetRiverData(int page, int rows,string stcds,string startDate,string endDate)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetRiverData(page, rows, stcds,startDate,endDate,addvcd,type);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 水位流量过程线

        public IActionResult RiverLine()
        {
            return View();
        }

        public IActionResult StationRiverLine()
        {
            return View();
        }

        public IActionResult GetRiverLine(string stnm)
        {
            var unit = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                unit = HttpContext.User.Claims.First().Value.Split(',')[3];
                DataTable dt = new DataTable();
                var aa = dt.ToJson();
                var lineDB = service.GetRiverData(unit, stnm);
                var data = new
                {
                    total = lineDB.Count(),
                    rows = lineDB

                };
                return Content(data.ToJson());
            }
            return Content("暂无数据");
        }

        public IActionResult GetRiverLineData(string stcd, string startDate, string endDate)
        {
            var lineDB = service.GetRiverData(stcd, startDate, endDate);
            var data = new
            {
                total = lineDB.Count(),
                rows = lineDB

            };
            return Content(data.ToJson());
        }
        public IActionResult GetRiverChartData(string stcd, string endDate, int stype)
        {
            var lineDB = service.GetRiverChartData(stcd, endDate, stype);
            var data = new
            {
                total = lineDB.Count(),
                rows = lineDB

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 水情均值

        public IActionResult Rvav()
        {
            return View();
        }

        public IActionResult GetRvavData(string stcd,string startDate,string endDate)
        {
            var list = service.GetRvavData(stcd, startDate, endDate);
            var data = new
            {
                total = list.Count(),
                rows = list

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 多站水位对比

        public IActionResult ZContra()
        {
            return View();
        }

        public IActionResult GetZContraData(string stcds,string startDate,string endDate)
        {
            var result = service.GetZDataByMultiStcds(stcds, startDate, endDate);
            return Content(result);
        }
        #endregion

        #region 多站流量对比

        public IActionResult QContra()
        {
            return View();
        }

        public IActionResult GetQContraData(string stcds,string startDate,string endDate)
        {
            var result = service.GetQDataByMultiStcds(stcds, startDate, endDate);
            return Content(result);
        }
        #endregion

        #region 测站基本信息

        /// <summary>
        /// 返回所有站点
        /// add by SUN
        /// Date:2019-05-23 17:00
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAllStations()
        {
            //var list = staService.GetStationList("");
            var list = new DataTable();
            return Content(list.ToJson());
        }

        public IActionResult GetStaByKeywords(string q, string sttp = "")
        {
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var list = staService.GetSearchKeywords(q, GetStrType(q), 0, sttp, type, addvcd); 
            return Content(list.ToJson());
        }

        public string GetStrType(string sVal)
        {
            string strType = "";
            if (string.IsNullOrEmpty(sVal))
            {
                strType = "";
            }else if (sVal.IsEnglish())
                strType = "1";
            else if (sVal.IsNumberic())
                strType = "2";
            else
                strType = "3";

            return strType;
        }

        #endregion

        #region 首页河道水情图表过程线
        //public IActionResult ChartRiver(string stcd,string sdate, string edate)
        public IActionResult ChartRiver(string stcd, string edate, int stype)
        {
            ////系统要求返回数据不能超过30天
            //if (!String.IsNullOrEmpty(sdate) && !String.IsNullOrEmpty(edate))
            //{
            //    DateTime dtstm = Convert.ToDateTime(sdate);
            //    DateTime dtetm = Convert.ToDateTime(edate);
            //    int interval = new TimeSpan(dtetm.Ticks - dtstm.Ticks).Days;
            //    if (interval > 30)
            //        sdate = dtetm.AddDays(-30).ToString("yyyy-MM-dd HH:mm");
            //}
            //else
            //{//如果只给一个日期，则默认数据周期为10天
            //    if (String.IsNullOrEmpty(sdate))
            //        sdate = Convert.ToDateTime(edate).AddDays(-10).ToString("yyyy-MM-dd HH:mm");
            //    if (String.IsNullOrEmpty(edate))
            //        edate = Convert.ToDateTime(sdate).AddDays(10).ToString("yyyy-MM-dd HH:mm");
            //}
            ViewData["stcd"] = stcd;
            //ViewData["startDate"] = sdate;
            ViewData["endDate"] = DateTime.Parse(edate).ToString("yyyy-MM-dd HH:mm");
            ViewData["stype"] = stype;
            return View();
        }


        #endregion


        #region 断面冲淤变化图

        public IActionResult SectScour()
        {
            return View();
        }

        #endregion

        #region 断面水位图
        public IActionResult SectionZChart()
        {
            return View();
        }

        public string GetSectionZ(string stcd,string stnm,string tm,string sDt)
        {
            return service.GetSectionZ(stcd, stnm, tm, sDt);
        }
        #endregion
    }
}