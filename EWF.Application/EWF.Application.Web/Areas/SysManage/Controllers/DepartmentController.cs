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

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class DepartmentController : EWFBaseController
    {
        private IServices.ISYS_DEPARTMENTService service;
        public DepartmentController(ISYS_DEPARTMENTService _departmentService)
        {
            service = _departmentService;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetDepartData(int page, int rows, string UCode, string DName)
        {
            //只取本单位的部门信息
            var uaccount = "";
            var addvcd = "";
            int type = 0;
            var unitid = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uaccount = HttpContext.User.Claims.First().Value.Split(',')[0];
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                unitid = HttpContext.User.Claims.ToList()[1].Value.ToString();
            }

            var pageObj = service.GetDepartmentData(page, rows, UCode, DName);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }

        /// <summary>
        /// 获取单位列表信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="unitname"></param>
        /// <returns></returns>
        public IActionResult GetDepartmentData(int page, int rows, string UCode, string DName,string UName)
        {
            //只取本单位的部门信息
            var uaccount = "";
            var addvcd = "";
            int type = 0;
            var unitid = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uaccount = HttpContext.User.Claims.First().Value.Split(',')[0];
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                unitid = HttpContext.User.Claims.ToList()[1].Value.ToString();
            }
            
            var pageObj = service.GetDepartmentData(page, rows, UCode, DName, UName, unitid);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }

        public string AddDepartmentInfo()
        {
            SYS_DEPARTMENT unit = new SYS_DEPARTMENT()
            {
                UNITID = Guid.NewGuid().ToString(),
                PARENTID = Guid.Empty.ToString(),
                DCODE = Request.Form["DCODE"],
                FULLNAME = Request.Form["FULLNAME"],
                SHORTNAME = Request.Form["SHORTNAME"],
                MANAGER = Request.Form["MANAGER"],
                PHONE = Request.Form["PHONE"],
                FAX = Request.Form["FAX"],
                EMAIL = Request.Form["EMAIL"],
                REMARK = Request.Form["REMARK"],
                ISENABLED = Request.Form["ISENABLED"].ToInt(),
                SORTCODE = Request.Form["SORTCODE"].ToInt(),
                DELETEMARK = 0,
                CREATEDATE = DateTime.Now,
                CREATEUSERID = "",
                CREATEUSERNAME = "",
                MODIFYDATE = DateTime.Now,
                MODIFYUSERID = "",
                MODIFYUSERNAME = ""
            };

            string result = service.Insert(unit);
            return result;
        }


        public string EditDepartmentInfo()
        {
            var Id = Request.Form["DepartmentID"];
            SYS_DEPARTMENT Unit = service.GetUnitByID(Id);
            Unit.DCODE = Request.Form["DCODE"];
            Unit.FULLNAME = Request.Form["FULLNAME"];
            Unit.SHORTNAME = Request.Form["SHORTNAME"];
            Unit.MANAGER = Request.Form["MANAGER"];
            Unit.PHONE = Request.Form["PHONE"];
            Unit.FAX = Request.Form["FAX"];
            Unit.EMAIL = Request.Form["EMAIL"]; 
            Unit.REMARK = Request.Form["REMARK"];
            Unit.ISENABLED = Request.Form["ISENABLED"].ToInt();
            Unit.SORTCODE = Request.Form["SORTCODE"].ToInt();
            Unit.MODIFYDATE = DateTime.Now;
            string result = service.Update(Unit);
            return result;
        }

        public string DeleteDepartmentInfo(string Id)
        {
            string result = service.Delete(Id);
            return result;
        }
    }
}