using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.ViewModels;
using EWF.IServices;
using EWF.Util;
using System.Dynamic;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //降水量-统计
    public class PstatController : EWFBaseController
    {
        private IPstatService service;

        public PstatController(IPstatService _service)
        {
            service = _service;
        }

        //URL:/HistoryInfo/Pstat/Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData_MDay(InCountConditionViewModel model)
        {
            var datasrc = "history";
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var list = service.GetDayData(model.STCD, model.sdate, model.edate, type, addvcd,ref datasrc);

            var data = new
            {
                datasrc,
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_MMonth(InCountConditionViewModel model)
        {
            var datasrc = "history";
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var list = service.GetMonthData(model.STCD, model.smonth, model.emonth, type, addvcd,ref datasrc);

            var nameArray = list.Select(x => x.STNM).Distinct();
            var monthArray = list.Select(x => x.Month).Distinct().Reverse();

            var varArray = new List<dynamic>();

            foreach (var month in monthArray)
            {
                dynamic valObj= new ExpandoObject();
                valObj.name = month;

                var dataArray = new List<double?>();
                foreach (var name in nameArray)
                {
                    var temp = list.Where(x => x.Month == month && x.STNM == name);
                    if (temp == null || temp.Count() == 0)
                    {
                        dataArray.Add(null);
                    }
                    else
                    {
                        dataArray.Add(temp.FirstOrDefault().MonSum);
                    }
                }

                valObj.data = dataArray.ToArray();
                varArray.Add(valObj);
            }
            
            var data = new
            {
                datasrc,
                total = list.Count(),
                rows = list,
                xData = nameArray,
                yData = varArray
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_Year(InCountConditionViewModel model)
        {
            var datasrc = "history";
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var list = service.GeYearData(model.STCD, model.year, type, addvcd,ref datasrc);
            var data = new
            {
                datasrc,
                total = list.Rows.Count,
                rows = list
            };
            return Content(data.ToJson());
        }

    }
}