using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.Entity;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class RiverWarnSetController : EWFBaseController
    {
        private IRiverWarnSetService service;

        public RiverWarnSetController(IRiverWarnSetService _service)
        {
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetRiverWarnSetData(int page, int rows, string stnm)
        {
            var addvcd = "";
            var type = "0";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
            }
            var pageObj = service.GetRiverWarnData(page, rows, stnm, Convert.ToInt32(type), addvcd);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());
        }

        public string EditRiverWarnSet()
        {
            ST_RVFCCH_B model = new ST_RVFCCH_B();
            model.STCD = Request.Form["STCD"];
            model.WRZ = Request.Form["WRZ"] == "" ? 0 : Request.Form["WRZ"].ToDecimal();
            model.WRQ = Request.Form["WRQ"] == "" ? 0 : Request.Form["WRQ"].ToDecimal();
            model.GRZ = Request.Form["GRZ"] == "" ? 0 : Request.Form["GRZ"].ToDecimal();
            model.GRQ = Request.Form["GRQ"] == "" ? 0 : Request.Form["GRQ"].ToDecimal();
            string result = service.UpdateData(model);
            return result;
        }
    }
}
