using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //水温气温-对比分析
    public class TmpavComparativeController : EWFBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}