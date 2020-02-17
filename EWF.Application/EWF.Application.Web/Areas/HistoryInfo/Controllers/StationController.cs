using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using System.Data;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    public class StationController : EWFBaseController
    {
        private  IStationService service;

        public StationController(IStationService _service)
        {
            service = _service;
        }
        
        public IActionResult GetList()
        {

            //'40101600','40101200'
            string stcds = "'40104700','40104730','40105150','40105453'";
            //var list = service.GetStationList(stcds);
            var list = new DataTable();
            return Content(list.ToJson());
        }
        
    }
}