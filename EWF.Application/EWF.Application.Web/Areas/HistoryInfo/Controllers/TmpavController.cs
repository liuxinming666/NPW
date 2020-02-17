using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using EWF.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //水温气温-统计
    public class TmpavController : EWFBaseController
    {
        private ITmpavService service;
        private IStationService staService;
        public TmpavController(ITmpavService _service, IStationService _staService)
        {
            service = _service;
            staService = _staService;
        }
        //URL:/HistoryInfo/Tmpav/Index
        public IActionResult Index()
        {
            return View();
        }


        #region 水温气温统计
        public IActionResult TempComparative()
        {
            return View();
        }
        public IActionResult GetTempComparativeData(string stcds, string startDate, string endDate)
        {

            var lineDB = service.GetTempComparativeData(stcds, startDate, endDate);
            var data = new
            {
                total = lineDB.Count(),
                rows = lineDB

            };
            return Content(data.ToJson());
        }
        #endregion


        #region 水温气温历史对比分析
        public IActionResult TempCompfx()
        {
            return View();
        }

        /// <summary>历史同期水温气温对比分析</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>水温气温数据</returns>
        public IActionResult GetData_MMonthEV(string stcd, string type, string sdate, string edate, string year)
        {

            var datasrc = "history";

            var list = service.GetData_MMonthEV(stcd, type, sdate, edate, year, ref datasrc);

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