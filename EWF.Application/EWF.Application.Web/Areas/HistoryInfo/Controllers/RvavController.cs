using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using EWF.Entity;
using EWF.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //水情-统计
    public class RvavController : EWFBaseController
    {
        private IRvavService service;
        public RvavController(IRvavService _service)
        {
            service = _service;
        }
        

        //URL:/HistoryInfo/Rvav/Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData_MDay(InCountConditionViewModel model)
        {
            if (model.sdate.IsEmpty())
            {
                return Error("开始日期不能为空！");
            }
            if (model.edate.IsEmpty())
            {
                return Error("截止日期不能为空！");
            }

            var datasrc = "history";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetDayData(model.STCD, addvcd, type, model.sdate, model.edate,ref datasrc);

            var data = new
            {
                datasrc,
                total = list.Count(),
                rows = list,
                chartData = list.OrderBy(x => x.IDTM),
                columnList = GetIntvColumns(model.sdate, model.edate)
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_MMonth(InCountConditionViewModel model)
        {
            var datasrc = "history";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetMonthData(model.STCD, addvcd, type, model.smonth, model.emonth,ref datasrc);
            
            var data = new
            {
                datasrc,
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_Year(InCountConditionViewModel model)
        {
            var datasrc = "history";
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GeYearData(model.STCD, addvcd, type, model.year,ref datasrc);
            var data = new
            {
                datasrc,
                total = list.Rows.Count,
                rows = list
            };
            return Content(data.ToJson());
        }


        private List<string> GetIntvColumns(string sdate, string edate)
        {
            List<string> list = new List<string>();

            DateTime dt1 = DateTime.Parse(sdate);
            DateTime dt2 = DateTime.Parse(edate);

            #region 动态添加列
            while (DateTime.Compare(dt1, dt2) <= 0)
            {
                list.Add(dt1.Month + "月" + dt1.Day + "日");
                dt1 = dt1.AddDays(1);
            }
            #endregion
            return list;
        }
        
    }
}