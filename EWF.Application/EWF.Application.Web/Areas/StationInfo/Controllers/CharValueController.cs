using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.StationInfo.Controllers
{
    [Authorize]
    public class CharValueController : EWFBaseController
    {
        private IStationService service;
        public CharValueController(IStationService _service)
        {
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region 历史特佂值
        /// <summary>
        /// 根据站码获取测站的年水位特征值数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <returns></returns>
        public IActionResult GetYearsZ(string stcd)
        {
            var list = service.GetYearsZ(stcd);
            var data = new
            {
                total = list.Count(),
                rows = list
            };

            return Content(data.ToJson());
        }
        
        /// <summary>
        /// 根据站码获取测站的年流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsQ(string stcd)
        {
            var list = service.GetYearsQ(stcd);
            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 根据站码获取测站的年含沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsSand(string stcd)
        {
            var list = service.GetYearsSand(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 根据站码获取测站的年径流量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsJL(string stcd)
        {
            var list = service.GetYearsJL(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 根据站码获取测站的年输沙量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsSSL(string stcd)
        {
            var list = service.GetYearsSSL(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 根据站码获取测站的年降水量特征值数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsRain(string stcd)
        {
            var list = service.GetYearsRain(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据站码获取输沙量径流量
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetYearsSSLJLL(string stcd)
        {
            var list = service.GetYearsSSL(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());

        }

        /// <summary>
        /// 根据条件获取测站的所有特征值数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public IActionResult GetAllCVData(string stcd)
        {
            var list = service.GetAllCVData(stcd);

            var data = new
            {
                Table = list.Table,
                Table1 = list.Table1,
                Table2 = list.Table2,
                Table3 = list.Table3,
                Table4 = list.Table4,
                Table5 = list.Table5
            };
            return Content(data.ToJson());
        }
        #endregion


         #region 历年水位流量关系曲线
        /// <summary>
        /// 根据站码获取测站的历年水位流量数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetZQRLData(string stcd)
        {
            var list = service.GetZQRLData(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取测站有数据的年份
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult GetStationZQRLYear(string stcd)
        {
            var list = service.GetStationZQRLYear(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }
        /// <summary>
        ///获取测站选择年份的数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="years"></param>
        /// <returns></returns>
        public IActionResult GetZQRLYearsData(string stcd,string years)
        {
            var list = service.GetZQRLYearsData(stcd, years);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }
        #endregion

    }
}