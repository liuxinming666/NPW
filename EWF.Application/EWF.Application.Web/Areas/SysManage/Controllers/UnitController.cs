using System;
using System.Collections.Generic;
using System.Data;
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
    public class UnitController : EWFBaseController
    {
        private ISYS_UNITService service;
        private ISYS_DEPARTMENTService dservice;
        public UnitController(ISYS_UNITService _unitService,ISYS_DEPARTMENTService _dservice)
        {
            service = _unitService;
            dservice = _dservice;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取单位列表信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public IActionResult GetUnitData(int page, int rows,string UCode, string UnitName)
        {
            var pageObj = service.GetUnitData(page, rows,UCode,UnitName);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }

        /// <summary>
        ///获取机构单位信息
        /// </summary>
        /// <param name="UCode"></param>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public IActionResult GetUnitList(string UCode, string UnitName)
        {
            DataTable dt = service.getUnitList(UCode,UnitName);
            var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + "}";
            return Content(result);
        }

        public string AddUnitInfo()
        {
            SYS_UNIT unit = new SYS_UNIT()
            {
                UNITID = Guid.NewGuid().ToString(),
                PARENTID = Guid.Empty.ToString(),
                CATEGORY = "",
                UCODE = Request.Form["UCODE"],
                FULLNAME = Request.Form["FULLNAME"],
                SHORTNAME = Request.Form["SHORTNAME"],
                MANAGER = Request.Form["MANAGER"],
                CONTACT = Request.Form["CONTACT"],
                PHONE = Request.Form["PHONE"],
                FAX = Request.Form["FAX"],
                EMAIL = Request.Form["EMAIL"],
                PROVINCEID = "",
                CITYID = "",
                COUNTYID = "",
                ADDRESS = Request.Form["ADDRESS"],
                POSTALCODE = Request.Form["POSTALCODE"],
                WEB = "",
                REMARK = Request.Form["REMARK"],
                ISENABLED = Request.Form["ISENABLED"].ToInt(),
                SORTCODE = Request.Form["SORTCODE"].ToInt(),
                DELETEMARK = 0,
                CREATEDATE = DateTime.Now,
                CREATEUSERID = "",
                CREATEUSERNAME = "",
                MODIFYDATE = DateTime.Now,
                MODIFYUSERID = "",
                MODIFYUSERNAME =""
            };

            string result = service.Insert(unit);
            return result;
        }

        public string AddZUnitInfo()
        {
            var Id = Request.Form["UNITID"];
            SYS_UNIT unit = new SYS_UNIT()
            {
                UNITID = Guid.NewGuid().ToString(),
                PARENTID = Id.ToString(),
                CATEGORY = "",
                UCODE = Request.Form["UCODE"],
                FULLNAME = Request.Form["FULLNAME"],
                SHORTNAME = Request.Form["SHORTNAME"],
                MANAGER = Request.Form["MANAGER"],
                CONTACT = Request.Form["CONTACT"],
                PHONE = Request.Form["PHONE"],
                FAX = Request.Form["FAX"],
                EMAIL = Request.Form["EMAIL"],
                PROVINCEID = "",
                CITYID = "",
                COUNTYID = "",
                ADDRESS = Request.Form["ADDRESS"],
                POSTALCODE = Request.Form["POSTALCODE"],
                WEB = "",
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

        public string AddDepartmentInfo()
        {
            var Id = Request.Form["DUNITID"];
            SYS_DEPARTMENT Depart = new SYS_DEPARTMENT()
            {
                UNITID = Id.ToString(),
                DEPARTMENTID =Guid.NewGuid().ToString(), 
                PARENTID = "",
                DCODE = Request.Form["DDCODE"],
                FULLNAME = Request.Form["DFULLNAME"],
                SHORTNAME = Request.Form["DSHORTNAME"],
                MANAGER = Request.Form["DMANAGER"],
                PHONE = Request.Form["DPHONE"],
                FAX = Request.Form["DFAX"],
                EMAIL = Request.Form["DEMAIL"],
                NATURE = "",
                REMARK = Request.Form["DREMARK"],
                ISENABLED = Request.Form["DISENABLED"].ToInt(),
                SORTCODE = Request.Form["DSORTCODE"].ToInt(),
                DELETEMARK = 0,
                CREATEDATE = DateTime.Now,
                CREATEUSERID = "",
                CREATEUSERNAME = "",
                MODIFYDATE = DateTime.Now,
                MODIFYUSERID = "",
                MODIFYUSERNAME = ""
            };
            string result = dservice.Insert(Depart);
            return result;
        }

        public string EditUnitInfo()
        {
            var Id = Request.Form["UNITID"];
            SYS_UNIT Unit = service.GetUnitByID(Id);
            Unit.UCODE = Request.Form["UCODE"];
            Unit.FULLNAME = Request.Form["FULLNAME"];
            Unit.SHORTNAME = Request.Form["SHORTNAME"];
            Unit.MANAGER = Request.Form["MANAGER"];
            Unit.CONTACT = Request.Form["CONTACT"];
            Unit.PHONE = Request.Form["PHONE"];
            Unit.FAX = Request.Form["FAX"];
            Unit.EMAIL = Request.Form["EMAIL"];;
            Unit.ADDRESS = Request.Form["ADDRESS"];
            Unit.POSTALCODE = Request.Form["POSTALCODE"];
            Unit.REMARK = Request.Form["REMARK"];
            Unit.ISENABLED = Request.Form["ISENABLED"].ToInt();
            Unit.SORTCODE = Request.Form["SORTCODE"].ToInt();
            Unit.MODIFYDATE = DateTime.Now;
            string result = service.Update(Unit);
            return result;
        }

        public string DeleteUnitInfo(string Id)
        {
            string result = service.Delete(Id);
            return result;
        }
    }
}