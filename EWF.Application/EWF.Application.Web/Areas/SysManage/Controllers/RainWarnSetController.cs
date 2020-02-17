using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.Entity;
using EWF.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class RainWarnSetController : EWFBaseController
    {
        private IRainWarnSetService service;
        public RainWarnSetController(IRainWarnSetService _service)
        {
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string GetRainWarnSetData()
        {
            var addvcd = "";
            var type = "0";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
            }
            DataTable dt = service.GetRainWarnData(Convert.ToInt32(type), addvcd);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        public string EditRainWarnSet()
        {
            var addvcd = "";
            var type = "0";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
            }
            var djArray = new int[] { 3, 2, 1 };
            string rtype = Request.Form["RTYPE"];
            string THRESHOLD_3= Request.Form["THRESHOLD_3"];//对应的等级是3
            string THRESHOLD_2 = Request.Form["THRESHOLD_2"];//对应的等级是2
            string THRESHOLD_1 = Request.Form["THRESHOLD_1"];//对应的等级是1
            TBL_EVENT_YLMODAL model = new TBL_EVENT_YLMODAL ();
            model.TYPE = Convert.ToInt32(type);
            model.ADDVCD = addvcd;
            string result = service.UpdateData(rtype, THRESHOLD_3, THRESHOLD_2, THRESHOLD_1, Convert.ToInt32(type), addvcd);
            return result;
        }

    }
}
