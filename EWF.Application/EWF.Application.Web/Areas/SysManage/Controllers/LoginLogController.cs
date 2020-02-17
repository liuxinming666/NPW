using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class LoginLogController : EWFBaseController
    {
        private ISYS_LOGINLOGService service;
        public LoginLogController(ISYS_LOGINLOGService _logService)
        {
            service = _logService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取登录日志信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetLoginLogData(int page, int rows, string logtype)
        {
            var pageObj = service.GetLoginLogData(page, rows, logtype);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items

            };
            return Content(data.ToJson());
        }
    }
}
