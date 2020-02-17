using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Entity;
using EWF.IServices;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class RsvrWarnController : EWFBaseController
    {
        private ISYS__RsvrWarnService service;
        public RsvrWarnController(ISYS__RsvrWarnService _rsvrService)
        {
            service = _rsvrService;
        }
        public IActionResult RsvrWarnPage()
        {
            return View();
        }
        public IActionResult GetRsvrWarnData(int page, int rows, string stnm)
        {
            var addvcd = "";
            var type = "0";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type= HttpContext.User.Claims.First().Value.Split(',')[2];
            }
            var pageObj = service.GetRsvrWarnData(page, rows, stnm, Convert.ToInt32(type), addvcd);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());
        }

        public string EditRsvr()
        {
            SYS_ST_RSVRFSR_B model = new SYS_ST_RSVRFSR_B();
            model.ACTYR = Request.Form["ACTYR"].ToInt();
            model.STCD = Request.Form["STCD"];
            model.FSTP = Request.Form["FSTP"];
            model.BGMD = Request.Form["BGMD"];
            model.EDMD = Request.Form["EDMD"];
            model.FSLTDZ = Request.Form["FSLTDZ"].ToDouble();
            string result = service.UpdateData(model);
            return result;
        }

    }
}
