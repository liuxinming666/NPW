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
    public class AddvcdController : EWFBaseController
    {
        private IServices.IST_ADDVCD_DService service;
        public AddvcdController(IST_ADDVCD_DService _Service)
        {
            service = _Service;
        }
        public IActionResult Index()
        {
            //系统管理员可以看到所有的，配置所有的，普通只能看到自己的分区
            //判断是不是超级管理员权限，超级管理员可以添加任何区域，其他管理员和普通人员只能添加自己所在区域
            var role = "";
            var addvcd = "";
            int atype = 0;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                atype = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                role = HttpContext.User.Claims.Last().Value;
            }
            var rolelist = service.GetAllRole(role, addvcd, atype.ToString(), "").ToList<SYS_ROLE>();
            if (rolelist.Count > 0)
            {
                ViewBag.addvcd = "";
                ViewBag.type = "";
            }
            else
            {
                ViewBag.addvcd = addvcd;
                ViewBag.type = atype;
            }
            //ViewBag.addvcd = addvcd;
            //ViewBag.type = atype;
            return View();
        }


        public IActionResult GetAddvcdData(int page, int rows, string Addvcd, string Addvnm,string Type)
        {            
            var pageObj = service.GetAddvcdData(page, rows, Addvcd, Addvnm,Type);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }


        public string AddAddvcdInfo()
        {
            ST_ADDVCD_D  unit = new ST_ADDVCD_D()
            {
                ADDVCD = Request.Form["ADDVCD"],
                ADDVNM = Request.Form["ADDVNM"],
                COMMENTS = Request.Form["COMMENTS"],
                TYPE = Request.Form["TYPE"].ToInt(),
                MODITIME = DateTime.Now,
            };
            string result = service.Insert(unit);
            return result;
        }


        public string EditAddvcdInfo()
        {
            var Id = Request.Form["UAddvcd"];
            ST_ADDVCD_D Unit = service.GetAddvcdByID(Id);
            //Unit.ADDVCD = Request.Form["UAddvcd"];
            Unit.ADDVNM = Request.Form["ADDVNM"];
            Unit.COMMENTS = Request.Form["COMMENTS"];
            Unit.TYPE = Request.Form["TYPE"].ToInt();
            Unit.MODITIME = DateTime.Now;
            string result = service.Update(Unit);
            return result;
        }

        public string DeleteAddvcdInfo(string Id)
        {
            string result = service.Delete(Id);
            return result;
        }
    }
}