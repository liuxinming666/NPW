using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    [Authorize]
    public class WatchWarnController : EWFBaseController
    {
        // GET: /RealData/River/Index
        private IWatchWarnService service;
        public WatchWarnController(IWatchWarnService _watchService)
        {
            service = _watchService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 根据指定的值返回对应的预警站点列表信息
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="sname">预警类型名称</param>
        /// <param name="tname">具体预警类别</param>
        /// <param name="sttp">测站类型</param>
        /// <param name="qjfz"></param>
        /// <returns></returns>
        public string GetWatchWarnDetailData(string date, string sname, string tname, string sttp, string qjfz)
        {
            var html = service.Get_WatchWarnDetailData(sname, tname, sttp, date, qjfz);
            return html;
        }
        //首页初始化
        public IActionResult WatchWarnData()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");//"2019-05-24 18:00:00";
            var type = "";
            var addvcd = "";
            var html = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
                html = service.GetWatchWarn(date, Convert.ToInt32(type), addvcd);
            }
            //var html = service.GetWatchWarn(date, Convert.ToInt32(type), addvcd);
            return Content(html);
        }
    }
}
