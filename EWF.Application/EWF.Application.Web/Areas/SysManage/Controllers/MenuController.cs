using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EWF.Entity;
using EWF.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class MenuController : EWFBaseController
    {
        private IMenuService service;
        public MenuController(IMenuService _menuService)
        {
            service = _menuService;
        }
        

        // GET: SysManage/Menu
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取全部菜单
        /// </summary>
        /// <returns></returns>
        public string GetMenuAll()
        {
            DataTable dtMenu =  service.GetMenuAll();

            return Newtonsoft.Json.JsonConvert.SerializeObject(dtMenu);
        }

        /// <summary>
        /// 增加菜单
        /// </summary>
        /// <returns></returns>
        public string AddMenu()
        {
            //var userName = FormsAuth.GetUserData().UserName;
            var userName = "admin";
            var menu = new sys_menuEntity
            {
                MenuCode = Request.Form["txtMenuCode"].ToString().Trim(),
                MenuName = Request.Form["txtMenuName"],
                ParentCode = Request.Form["selParentMenu"],
                IconClass = Request.Form["selIcon"],
                URL = Request.Form["txtLink"],
                Description = Request.Form["Description"],
                MenuSeq = Request.Form["txtSort"].ToInt(),
                CreatePerson = userName,
                CreateDate = DateTime.Now,
                IsEnable = (Request.Form["ckEnable"] == "on" ? true : false),
                IsVisible = (Request.Form["ckVisible"] == "on" ? true : false)
            };

            var result = service.Insert(menu);
            return result;
        }

        public string EditMenu()
        {
            //var userName = FormsAuth.GetUserData().UserName;
            var userName = "";
            var Id = Request.Form["txtId"].ToInt();
            sys_menuEntity menu = service.Get(Id);
            menu.MenuCode = Request.Form["txtMenuCode"];
            menu.MenuName = Request.Form["txtMenuName"];
            menu.ParentCode = Request.Form["selParentMenu"];
            menu.IconClass = Request.Form["selIcon"];
            menu.URL = Request.Form["txtLink"];
            menu.Description = Request.Form["Description"];
            menu.MenuSeq = Request.Form["txtSort"].ToInt();
            menu.CreatePerson = userName;
            menu.CreateDate = DateTime.Now;
            menu.IsEnable = (Request.Form["ckEnable"] == "on" ? true : false);
            menu.IsVisible = (Request.Form["ckVisible"] == "on" ? true : false);
            
            string result = service.Update(menu);
            return result;
        }
        public string DeleteMenu(int Id)
        {
            var result = service.Delete(Id);
            return result;
        }
    }
}