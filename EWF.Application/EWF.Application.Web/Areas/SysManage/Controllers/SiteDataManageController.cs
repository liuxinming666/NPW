/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/5/29 14:45:11
 * 描  述：数据维护-测站
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class SiteDataManageController : EWFBaseController
    {
        #region 测站概况
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}