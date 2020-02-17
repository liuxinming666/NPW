using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    public class PstatComparativeController : EWFBaseController
    {
        private IPstatService service;

        public PstatComparativeController(IPstatService _service)
        {
            service = _service;
        }
        //降水量-对比分析
        public IActionResult PstatComparative()
        {
            return View();
        }
        /// <summary>历史同期降水量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>月降水量</returns>
        public IActionResult GetMonthComparativeData(string STCD, string sdate, string edate, string year)
        {
            #region 参数检查
            if (STCD.IsEmpty())
            {
                return Error("测站不能为空！");
            }
            if (sdate.IsEmpty())
            {
                return Error("开始日期不能为空！");
            }
            if (edate.IsEmpty())
            {
                return Error("截止日期不能为空！");
            }
            if (year.IsEmpty())
            {
                return Error("对比年份不能为空！");
            }
            //判断日期不能跨年
            if (Convert.ToDateTime(sdate).Year != Convert.ToDateTime(edate).Year)
            {
                return Error("查询时间段不能跨年！");
            }
            #endregion
            var datasrc = "history";

            var list = service.GetMonthComparativeData(STCD, "5", sdate, edate, year, ref datasrc);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());


        }

        /// <summary>历史同期降水量对比分析-旬</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="sten">开始旬</param>
        /// <param name="eten">结束旬</param>
        /// <param name="year">对比年份</param>
        /// <returns>旬降水量</returns>
        public IActionResult GetTenComparativeData(string STCD, string sdate, string edate, string sten, string eten, string year)
        {
            #region 参数检查
            if (STCD.IsEmpty())
            {
                return Error("测站不能为空！");
            }
            if (sdate.IsEmpty())
            {
                return Error("开始日期不能为空！");
            }
            if (edate.IsEmpty())
            {
                return Error("截止日期不能为空！");
            }
            if (year.IsEmpty())
            {
                return Error("对比年份不能为空！");
            }
            if (sten.IsEmpty())
            {
                return Error("开始旬不能为空！");
            }
            if (eten.IsEmpty())
            {
                return Error("结束旬不能为空！");
            }            
            //判断日期不能跨年
            if (Convert.ToDateTime(sdate).Year != Convert.ToDateTime(edate).Year)
            {
                return Error("查询时间段不能跨年！");
            }
            #endregion

            var datasrc = "history";            
            
            var list = service.GetTenComparativeData(STCD, "4", sdate, edate, sten, eten, year, ref datasrc);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());


        }
    }
}