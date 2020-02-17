using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.ViewModels;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Log;
using System.Dynamic;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //径流量-统计
    public class SedrfController : EWFBaseController
    {
        private ISedrfService service;
        private LoggerHelper logger;

        public SedrfController(ISedrfService _service, LoggerHelper _logger)
        {
            service = _service;
            logger = _logger;
        }

        //URL:/HistoryInfo/Sedrf/Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData_MDay(InCountConditionViewModel model)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var isShowSTW =false;
            var datasrc   = "history";
            var list      = service.GetDayData(model.STCD,addvcd,type, model.sdate, model.edate, ref datasrc);

            var nameArray = list.Select(x => x.STNM).Distinct();
            var dayArray = list.Select(x => x.IDTM.ToString("MM-dd")).Distinct().Reverse();

            var varArray = new List<dynamic>();
            var sArray = new List<dynamic>();

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
                        sdataArray.Add(null);
                    }
                    else
                    {
                        dataArray.Add(temp.FirstOrDefault().WRNF);
                        sdataArray.Add(temp.FirstOrDefault().STW);
                        if (temp.FirstOrDefault().STW != null)
                        {
                            isShowSTW = true;
                        }
                    }
                }

                varArray.Add(new { name = day, data = dataArray.ToArray() });
                sArray.Add(new { name = day, data = sdataArray.ToArray() });
            }
            
            var data = new
            {
                total     = list.Count(),
                rows      = list,
                xData     = nameArray,
                yData     = varArray,
                yDataS    = sArray,
                isShowSTW = isShowSTW
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_MMonth(InCountConditionViewModel model)
        {
            var datasrc = "history";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetMonthData(model.STCD,addvcd,type, model.smonth, model.emonth, ref datasrc);

            var nameArray = list.Select(x => x.STNM).Distinct();
            var monthArray = list.Select(x => x.Month).Distinct().Reverse();

            var varArray = new List<dynamic>();
            var sArray = new List<dynamic>();

            foreach (var month in monthArray)
            {
                var dataArray = new List<double?>();
                var sdataArray = new List<double?>();
                foreach (var name in nameArray)
                {
                    var temp = list.Where(x => x.Month == month && x.STNM == name);

                    var json = temp.ToJson();
                    logger.Debug(json);

                    if (temp == null || temp.Count() == 0)
                    {
                        dataArray.Add(null);
                        sdataArray.Add(null);
                    }
                    else
                    {
                        dataArray.Add(temp.FirstOrDefault().WMonSum);
                        sdataArray.Add(temp.FirstOrDefault().SMonSum);
                    }
                }
                
                varArray.Add(new { name = month, data = dataArray.ToArray() });
                sArray.Add(new { name = month, data = sdataArray.ToArray() });
            }

            var data = new
            {
                total = list.Count(),
                rows  = list,
                xData = nameArray,
                yData = varArray,
                yDataS= sArray
            };
            return Content(data.ToJson());
        }

        public IActionResult GetData_Year(InCountConditionViewModel model)
        {
            var datasrc = "history";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GeYearData(model.STCD,addvcd,type, model.year,ref datasrc);
            var data = new
            {
                total = list.Rows.Count,
                rows = list
            };
            return Content(data.ToJson());
        }
        
    }
}