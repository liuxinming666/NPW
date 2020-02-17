using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    [Authorize]
    public class RsvrController : EWFBaseController
    {
        private IRsvrService service;

        public RsvrController(IRsvrService _service)
        {
            service = _service;
        }
        
        public IActionResult RealData()
        {
            return View();
        }
        public IActionResult RsvrSearch()
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

        public IActionResult LatestRsvr()
        {
            return View();
        }

        /// <summary>
        /// 返回最新水库水情数据
        /// add by SUN
        /// Date:2019-05-23 14:00
        /// </summary>
        /// <returns></returns>
        public IActionResult GetLatestRsvrData(string startDate, string endDate)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetLatestRsvrData("", "", addvcd, type);

            var data = new
            {
                total = pageObj.Count,
                rows = pageObj

            };
            return Content(data.ToJson());
        }
        #endregion


        #region 河道水情查询

        public IActionResult RsvrData()
        {
            return View();
        }

        public IActionResult GetRsvrData(int page, int rows, string stcds, string startDate, string endDate)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageObj = service.GetRsvrData(page, rows, stcds, startDate, endDate, addvcd, type);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 水库水情过程线
        public IActionResult RsvrLine()
        {
            return View();
        }

        /// <summary>
        /// 水库水情过程线
        /// add by lw
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IActionResult GetRsvrLine(string stcd, string startDate, string endDate)
        {
            var list = service.GetRsvr_Line(stcd, startDate, endDate);
            var data = new
            {
                total = list.Count(),
                rows = list

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 水库水情均值
        public IActionResult RsvrAvg()
        {
            return View();
        }

        /// <summary>
        /// 水库水情均值
        /// add by lw
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IActionResult GetRsvrAvg(int sttdrcd, string stcd, string startDate, string endDate)
        {
            var list = service.GetRsvrav_avg(sttdrcd, stcd, startDate, endDate);
            var data = new
            {
                total = list.Count(),
                rows = list

            };
            return Content(data.ToJson());
        }
        #endregion

        #region 首页水库水情图表过程线
        //public IActionResult ChartRsvr(string stcd, string sdate, string edate)
        public IActionResult ChartRsvr(string stcd, string edate)
        {
            //if (!String.IsNullOrEmpty(sdate) && !String.IsNullOrEmpty(edate))
            //{
            //    DateTime dtstm = Convert.ToDateTime(sdate);
            //    DateTime dtetm = Convert.ToDateTime(edate);
            //    int interval = new TimeSpan(dtetm.Ticks - dtstm.Ticks).Days;
            //    if (interval > 30)
            //        sdate = dtetm.AddDays(-30).ToString("yyyy-MM-dd HH:mm");
            //}
            //else
            //{//如果只给一个日期，则默认数据周期为15天
            //    if (String.IsNullOrEmpty(sdate))
            //        sdate = Convert.ToDateTime(edate).AddDays(-14).ToString("yyyy-MM-dd HH:mm");
            //    if (String.IsNullOrEmpty(edate))
            //        edate = Convert.ToDateTime(sdate).AddDays(14).ToString("yyyy-MM-dd HH:mm");
            //}
            ViewData["stcd"] = stcd;
            //ViewData["startDate"] = sdate;
            ViewData["endDate"] = edate;
            return View();
        }
        /// <summary>
        /// 首页水库水情过程线
        /// add by qlj
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        //public IActionResult GetRsvrLineEight(string stcd, string startDate, string endDate)
        public IActionResult GetRsvrLineEight(string stcd, string endDate)
        {
            var startDate = "";
            var list = service.GetRsvrLineEight(stcd, startDate, endDate);
            var data = new
            {
                total = list.Count(),
                rows = list

            };
            return Content(data.ToJson());
        }

        #endregion
    }
}