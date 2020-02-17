using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using System.Data;
using EWF.IServices;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    [Authorize]
    public class IceJamController : EWFBaseController
    {
        private IIcejamService service;
        private IStationService staService;
        private IHostingEnvironment hostingEnvironmen;
        public IceJamController(IIcejamService _service, IStationService _staService,IHostingEnvironment _hostingEnvironmen)
        {
            service = _service;
            staService = _staService;
            hostingEnvironmen = _hostingEnvironmen;
        }
        //public IActionResult RealData()
        //{
        //    return View();
        //}

        #region 单站温度对比
        public IActionResult SingleTempContra()
        {
            return View();
        }
        public IActionResult GetSingleTempContraData(string stcds, string startDate, string endDate)
        {
            
            var lineDB = service.GetSingleTempData(stcds, startDate, endDate);
            var data = new
            {
                total = lineDB.Count(),
                rows = lineDB

            };
            return Content(data.ToJson());
        }
        #endregion
        #region 多站温度对比
        public IActionResult MultiTempContra()
        {
            return View();
        }
        public IActionResult GetMultiTempContraData(string stcds,string sqnm, string startDate, string endDate)
        {
            var result = service.GetTempDataByMultiStcds(stcds,sqnm, startDate, endDate);
            return Content(result);
        }
        #endregion
        public IActionResult GetQelByKeywords(string q)
        {
            var list = service.GetSearchKeywords(q);
            return Content(list.ToJson());
        }

        #region 查询实时凌情信息
        public IActionResult IceJamDate()
        {
            return View();
        }
        /// <summary>
        /// 查询实时凌情信息
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IActionResult GetIceDate(string stcds, string startDate, string endDate)
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string type = HttpContext.User.Claims.First().Value.Split(',')[2];
            string result = service.GetIceDate(stcds, startDate, endDate, addvcd, type);
            return Content(result);
        }
        #endregion

        #region 凌情动态
        public IActionResult IceJamDynamic()
        {
            //string dt = service.GetIceJamDynamic("2018-01-01 08:00", "2019-01-01 08:00");
            return View();
        }

        public IActionResult GetDynamicJson()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath+ @"\MapDate\Dynamic.geojson"))
            {
                 str=sr.ReadToEnd();
            }
            return Content(str);
        }
       
        public IActionResult GetDynamicPoint(string state)
        {
            
            //state = "2019-06-01 08:00:00.000";
            DataTable dt = service.GetIceJam_River(state);
            //DataTable  dt = dts.Tables[0];
            //DataTable dtResult = dts.Tables[1];
            //var result = "{\"rows\":" + dt.ToJson()+ ",\"total\":" + dt.Rows.Count + ",\"Rvalue\":'"+dtResult.Rows[0]["CONTEXTSTR"].ToString()+"'}";
            var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + "}";
            return Content(result);
        }

        public string GetDynamicLQ(string state)
        {
            //state = "2016-11-24 08:00:00.000";
            DataTable dts = service.GetIceJam_LQDT(state);
            string value = "";
            //DataTable dtResult = dts.Tables[1];
            //var result = "{\"Rvalue\":'" + dtResult.Rows[0]["CONTEXTSTR"].ToString() + "'}";
            //var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + "}";
            if(dts.Rows.Count>0)
            {
                value= dts.Rows[0]["CONTEXTSTR"].ToString();
            }
            return value;
        }
        public IActionResult GetIceDynamice(string state)
        {
            //state = "2019-03-01 08:00";
            DataTable dt = service.GetIceJamDynamic(state);
            var result = "{\"rows\":" + dt.ToJson() + ",\"total\":" + dt.Rows.Count + "}";
            return Content(result);
        }
        #endregion
    }
}
