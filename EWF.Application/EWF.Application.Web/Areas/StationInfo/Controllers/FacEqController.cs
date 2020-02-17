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
    public class FacEqController : EWFBaseController
    {
        private IStationService service;
        public FacEqController(IStationService _service)
        {
            service = _service;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取设施设备分类的信息树数据
        /// </summary>
        /// <returns></returns>
        public IActionResult GetFacEqTreeData()
        {
            var list = service.GetFacEqTreeData();

            var data = new
            {
                Table = list.Table,
                Table1 = list.Table1
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据站码和表名获取相关的表信息和测站的相关设施设备信息
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IActionResult GetFacEqFieldsAndData(string stcd, string tableName)
        {
            var list = service.GetFacEqFieldsAndData(stcd, tableName);

            var data = new
            {
                Table = list.Table,
                Table1 = list.Table1
            };
            return Content(data.ToJson());
        }
    }
}