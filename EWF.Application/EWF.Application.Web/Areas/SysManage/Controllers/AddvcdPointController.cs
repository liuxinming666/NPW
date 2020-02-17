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
    public class AddvcdPointController : EWFBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        private IServices.IST_ADDVCD_POINTService service;
        public AddvcdPointController(IST_ADDVCD_POINTService _Service)
        {
            service = _Service;
        }

        public IActionResult GetAddvcdPointData(int page, int rows,string Addvcd, string Stnm )
        {
            var pageObj = service.GetAddvcdPointData(page, rows, Addvcd, Stnm);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }


        public IActionResult GetAddPointData(int page, int rows, string Addvcd, string Stnm)
        {
            var pageObj = service.GetAddPointData(page, rows, Addvcd, Stnm);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }

        public IActionResult GetStcdData(int page, int rows, string Stcd, string Stnm)
        {
            var pageObj = service.GetStcdData(page, rows, Stcd, Stnm);
            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());

        }

        public string AddAddvcdPointInfo(string Addvcd,string Stcd,int Type)
        {
            ST_ADDVCD_POINT unit = new ST_ADDVCD_POINT()
            {
                ADDVCD = Addvcd,
                STCD = Stcd,
                TYPE = Type,
            };
            string result = service.Insert(unit);
            return result;
        }


        public string DeleteAddvcdPointInfo(string Addvcd, string Stcd)
        {
            string result = service.Delete(Addvcd,Stcd);
            return result;
        }
    }
}