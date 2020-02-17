using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //径流量-对比分析
    public class SedrfComparativeController : EWFBaseController
    {
        private ISedrfService service;

        public SedrfComparativeController(ISedrfService _service)
        {
            service = _service;
        }

        //URL:/HistoryInfo/Sedrf/Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetDayData(string STCD, string sdate, string edate, string year)
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
            #endregion

            var datasrc = "history";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetDayData_Comparative(STCD,addvcd,type, sdate, edate, year, ref datasrc);

            var data = new
            {
                total = list.Count(),
                rows  = list
            };

            return Content(data.ToJson());
        }

    }
}