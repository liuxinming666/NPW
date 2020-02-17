using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using EWF.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //蒸发量-统计
    public class EstatController : EWFBaseController
    {
        private IEstatService service;

        public EstatController(IEstatService _service)
        {
            service = _service;
        }

        //URL:/HistoryInfo/Estat/Index
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

            var nameArray = list.Select(x => x.STNM).Distinct();
            var dayArray = list.Select(x => x.IDTM.ToString("MM-dd")).Distinct().Reverse();

            var varArray = new List<dynamic>();

            foreach (var day in dayArray)
            {
                var dataArray = new List<double?>();
                var sdataArray = new List<double?>();
                foreach (var name in nameArray)
                {
                    var temp = list.Where(x => x.IDTM.ToString("MM-dd") == day && x.STNM == name);
                    if (temp == null || temp.Count() == 0)
                    {
                        dataArray.Add(null);
                    }
                    else
                    {
                        dataArray.Add(temp.FirstOrDefault().ACCE);
                    }
                }

                varArray.Add(new { name = day, data = dataArray.ToArray() });
            }
            

            var data = new
            {
                datasrc,
                total = list.Count(),
                rows = list,
                xData     = nameArray,
                yData     = varArray
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
        public IActionResult Evaporation()
        {
            return View();
        }
        
        /// <summary>历史同期蒸发量对比分析-月</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>月蒸发量</returns>
        public IActionResult GetData_MMonthEV(string STCD,string type, string sdate, string edate, string year)
        {
           
            var datasrc = "history";

            var list = service.GetData_MMonthEV(STCD, type, sdate, edate, year, ref datasrc);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());


        }
        /// <summary>历史同期降水量对比分析-旬</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="sten">开始旬</param>
        /// <param name="eten">结束旬</param>
        /// <param name="year">对比年份</param>
        /// <returns>旬降水量</returns>
        public IActionResult GetData_MXunEV(string STCD, string sdate, string edate, string sten, string eten, string year)
        {
            
            var datasrc = "history";

            var list = service.GetData_MXunEV(STCD, "4", sdate, edate, sten, eten, year, ref datasrc);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());


        }
    }
}