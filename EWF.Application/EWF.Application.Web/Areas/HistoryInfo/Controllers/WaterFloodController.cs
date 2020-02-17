using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    public class WaterFloodController : EWFBaseController
    {
        private IWaterFloodService service;
        public WaterFloodController(IWaterFloodService _service)
        {
            service = _service;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        //单站多要素对比分析
        public IActionResult GetWaterFloodData(string STCD, string sdate, string edate)
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
            #endregion

            var list = service.GetWaterFloodData(STCD, sdate, edate);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        //多站单要素对比分析
        public IActionResult GetWaterFloodMutiData(string STCD, string sdate, string edate, string ystype)
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
            if (ystype.IsEmpty())
            {
                return Error("对比要素不能为空！");
            }
            #endregion

            var list = service.GetWaterFloodMutiData(STCD, sdate, edate, ystype);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

    }
}
