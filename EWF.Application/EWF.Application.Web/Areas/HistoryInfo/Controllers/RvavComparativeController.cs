using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //水情-对比分析
    public class RvavComparativeController : EWFBaseController
    {
        private IRvavService service;
        public RvavComparativeController(IRvavService _service)
        {
            service = _service;
        }


        //URL:/HistoryInfo/RvavComparative/Index
        //水情多日均值
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData(string STCD,string avgType, string sdate, string edate, string year)
        {
            #region 参数检查
            if (STCD.IsEmpty())
            {
                return Error("测站不能为空！");
            }
            if (avgType.IsEmpty())
            {
                return Error("均值类型！");
            }
            if (sdate.IsEmpty())
            {
                return Error("开始日期不能为空！");
            }
            if (edate.IsEmpty())
            {
                return Error("截止日期不能为空！");
            }
            if (year.IsEmpty())
            {
                return Error("对比年份不能为空！");
            }
            #endregion

            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetData_Comparative(STCD,addvcd,type, avgType, sdate, edate, year);

            var data = new
            {
                total = list.Count(),
                rows = list
            };

            return Content(data.ToJson());
        }
        
    }
}