using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.IServices.SysManage;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class CharVManage : EWFBaseController
    {
        private ISYS_CharVManageService service;
       
        public CharVManage(ISYS_CharVManageService _Service)
        {
            service = _Service;
        }
        
        
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ZQLineIndex()
        {
            return View();
        }
        #region 历史特征值
        /// <summary>
        /// 根据测站类型获取数据
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IActionResult GetData(string stcd,string type)
        {
            var list = service.GetData(stcd,type);
            var data = new
            {
                Table = list.Table,
                Table1 = list.Table1
            };

            return Content(data.ToJson());
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <param name="type">类型</param>
        /// <param name="field">字段组</param>
        /// <param name="fieldType">字段类型组</param>
        /// <param name="fieldContent">内容组</param>
        /// <returns></returns>
        public string SaveData(string stcd,string type,string field, string fieldType, string fieldContent)
        {
            var list = service.SaveData(stcd, type, field, fieldType, fieldContent);
            return list;
            //return "";
        }
        #endregion
        #region 历年水位流量关系曲线
        public IActionResult GetZQLineData(string stcd)
        {
            var list = service.GetZQLineData(stcd);

            var data = new
            {
                total = list.Count(),
                rows = list
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 保存水位流量关系曲线数据
        /// </summary>
        /// <param name="stcd">站码</param>
        /// <param name="field">字段</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="fieldContent">字段内容</param>
        /// <returns></returns>
        public string SaveZQLineData(string stcd, string field, string fieldType, string fieldContent)
        {
            var list = service.SaveZQLineData(stcd, field, fieldType, fieldContent);
            return list;
            //return "";
        }
        #endregion
    }
}
