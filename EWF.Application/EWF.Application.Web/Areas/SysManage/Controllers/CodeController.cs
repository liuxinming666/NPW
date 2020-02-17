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
    public class CodeController : EWFBaseController
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GeneratorByTable()
        {
            return View();
        }

        public IActionResult GeneratorNoData()
        {
            return View();
        }

    }
}