using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices.SysManage;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class FACEQManage : EWFBaseController
    {
        private ISYS_FACEQService service;
        public FACEQManage(ISYS_FACEQService _Service)
        {
            service = _Service;
        }
        #region 设施设备维护
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 保存编辑后的数据到相应表格
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="tableName"></param>
        /// <param name="newdata"></param>
        /// <returns></returns>
        public string SaveFacEqData(string stcd, string tableName, string fieldName,string  fieldType,string fieldContent)
        {
            var list = service.SaveFacEqData(stcd, tableName, fieldName, fieldType, fieldContent);
            return list;
            //return "";
        }
        #endregion 
        
    }
}
