using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using System.Data;
using EWF.IServices;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.StationInfo.Controllers
{
    [Authorize]
    public class DetailsController : EWFBaseController
    {
        private IStationService service;

        public DetailsController(IStationService _service)
        {
            service = _service;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 水文图表页面
        /// </summary>
        /// <returns></returns>
        public IActionResult WaterChart()
        {
            return View();
        }

        public IActionResult StationsTree()
        {
            return View();
        }

        public IActionResult rainStationInfo()
        {
            return View();
        }

        public IActionResult ZQRLSet()
        {
            return View();
        }

        public IActionResult productImage()
        {
            return View();
        }

        public string GetStrType(string sVal)
        {
            string strType = "";
            if (sVal.IsEnglish())
                strType = "1";
            else if (sVal.IsNumberic())
                strType = "2";
            else
                strType = "3";

            return strType;
        }

        public IActionResult GetSearchKeywords(string q,string sttp)
        {
            int type = 0;
            string addvcd = "";
            type = Convert.ToInt32(HttpContext.User.Claims.First().Value.Split(',')[2]);
            addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var list = service.GetSearchKeywords(q, GetStrType(q), 10, sttp, type, addvcd);

            return Content(list.ToJson());
        }

        public IActionResult GetStationList(string stcds)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var list = service.GetStationList(stcds,addvcd,type);

            return Content(list.ToJson());
        }

        public IActionResult GetRainStationInfo(string stcd)
        {
            var list = service.GetRainStationInfo(stcd);

            return Content(list.ToJson());
        }

        public IActionResult GetGLDMInfo(string stcd, string stnm, string year, string sDt)
        {
            var list = service.GetGLDMInfo(stcd, stnm, year, sDt);

            return Content(list.ToJson());
        }

        public IActionResult GetStationSectionYears(string stcd)
        {
            var list = service.GetStationSectionYears(stcd);

            return Content(list.ToJson());
        }

        /// <summary>
        /// 根据站码，年份和月份获取水位流量关系曲线数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public IActionResult GetZQRLYearMonthData(string stcd, string year)
        {
            var list = service.GetZQRLYearMonthData(stcd,year);

            return Content(list.ToJson());
        }

        /// <summary>
        /// 获取测站水位流量关系曲线有数据的年份和月份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetStationZQRLYearMonth(string stcd)
        {
            var list = service.GetStationZQRLYearMonth(stcd);

            return Content(list.ToJson());
        }

        public IActionResult GetStationSectionNames(string stcd)
        {
            var list = service.GetStationSectionNames(stcd);

            return Content(list.ToJson());
        }
        /// <summary>
        /// 获取水流沙过程FLASH数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public IActionResult GetCalLineData(string stcd, string sDate, string eDate)
        {
            var list = service.GetCalLineData(stcd, sDate, eDate);

            var data = new
            {
                dt1 = list.list1,
                dt2 = list.list2,
                dt3 = list.list3
            };

            return Content(data.ToJson());
        }

        public IActionResult GetThreeLineData(string stcd,string sDate,string eDate)
        {
            var list = service.GetThreeLineData(stcd, sDate, eDate);

            var data = new
            {
                total = list.Count(),
                rows = list
            };

            return Content(data.ToJson());
        }
    }
}