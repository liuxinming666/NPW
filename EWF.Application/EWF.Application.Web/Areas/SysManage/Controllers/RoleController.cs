using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Mvc;
using EWF.Entity;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class RoleController : EWFBaseController
    {
        private ISYS_ROLEService service;
        public RoleController(ISYS_ROLEService _menuService)
        {
            service = _menuService;
        }


        //URL:/SysManage/Role/Index
        public IActionResult Index()
        {
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var role = HttpContext.User.Claims.Last().Value;
            //超级管理员获取所有角色信息，其他不能获取超级管理员角色
            var rolelist = service.GetAllRole(role, addvcd, type, "").ToList<SYS_ROLE>();
            if (rolelist.Count > 0)
            {
                ViewBag.type = "";
                ViewBag.addvcd = "";
            }
            else
            {
                ViewBag.type = type;
                ViewBag.addvcd = addvcd;
            }
            return View();
        }
        //[MvcMenuFilter(false)]
        //查询角色信息
        public IActionResult getRoleInfoData(string RoleName, string addvcd, string type, int rows = 20, int page = 1)
        {
            //var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            //int type = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
            var pageList = service.GetUserData(page, rows, RoleName, addvcd, Convert.ToInt32(type != "" ? type : "0"));
            var data = new
            {
                total = pageList.TotalItems,
                rows = pageList.Items
            };
            return Content(data.ToJson());

        }
        //[MvcMenuFilter(false)]
        //增加角色信息
        public JsonResult addRoleInfo(SYS_ROLE entity)
        {
            //先判断角色编码是否重名
            //var CreatePerson = "";// FormsAuth.GetUserData().UserName;
            entity.CreatePerson = HttpContext.User.Claims.First().Value.Split(',')[0];
            entity.CreateDate = DateTime.Now;
            entity.ADDVCD = HttpContext.User.Claims.First().Value.Split(',')[3];
            entity.TYPE = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
            if (service.IsExists(entity.RoleCode, entity.ADDVCD, entity.TYPE.ToString()) == 0)
            {
                if (service.Insert(entity))
                    return Json(new { result = "success", msg = "添加成功" });
                else
                    return Json(new { errorMsg = "error", msg = "添加失败" });
            }
            else
                return Json(new { errorMsg = "error", msg = "角色编码不能重复" });
        }
        //[MvcMenuFilter(false)]
        //修改角色信息
        public JsonResult editRoleInfo(SYS_ROLE entity)
        {
            var id = Request.Form["ID"].ToInt();
            SYS_ROLE model = service.Get(id);
            model.RoleCode = entity.RoleCode;
            model.RoleName = entity.RoleName;
            model.Description = entity.Description;

            model.ADDVCD = Request.Form["XIAN"].ToString() == "" ? Request.Form["CITY"].ToString() : Request.Form["XIAN"].ToString();
            model.TYPE = entity.TYPE;

            model.UpdatePerson = HttpContext.User.Claims.First().Value.Split(',')[0];
            model.UpdateDate = DateTime.Now;
            //var CreatePerson = "";// FormsAuth.GetUserData().UserName;

            //entity.UpdatePerson = CreatePerson;
            //entity.UpdateDate = DateTime.Now;

            if (service.IsExists(model.RoleCode, model.ADDVCD, model.TYPE.ToString()) == 0){
                if (service.Update(model))
                    return Json(new { result = "success", msg = "修改成功" });

                return Json(new { errorMsg = "error", msg = "修改失败" });
            }
            else
                return Json(new { errorMsg = "error", msg = "角色编码不能重复" });

        }
        //[MvcMenuFilter(false)]
        //删除角色信息
        public ActionResult deleteRoleInfo(int ID)
        {
            if (service.Delete(ID))
                return Json(new { result = "success", msg = "删除成功" });

            return Json(new { errorMsg = "error", msg = "删除失败" });
        }
        ////[MvcMenuFilter(false)]
        ////获取用户角色信息
        //public string GetRoleCode(string UserCode)
        //{
        //    var aa = att.GetUserRoleCode(UserCode);
        //    string json = "{";
        //    json += "\"data\":\"" + aa + "\"}";
        //    return json;
        //}
        ////[MvcMenuFilter(false)]
        ////保存用户角色
        //public JsonResult SaveSetRole(string UserCode1, string checkedcodes)
        //{
        //    if (checkedcodes.Length > 0)
        //    {
        //        bool flag = att.insertUserRole(UserCode1, checkedcodes);
        //        if (flag)
        //            return Json(new { result = "success", msg = "添加成功" });
        //        else
        //            return Json(new { errorMsg = "error", msg = "添加失败" });
        //    }
        //    else
        //        return Json(new { errorMsg = "error", msg = "请选择角色名" });
        //}
        //[MvcMenuFilter(false)]
        //获取角色菜单信息
        public string GetRoleMenuCode(string RoleCode)
        {
            var  dtMenu = service.GetRoleMenuCode(RoleCode);

            return dtMenu.ToJson();
        }
        //[MvcMenuFilter(false)]
        //保存角色菜单
        public JsonResult SaveSetMenu(string RoleCode1, string MenuCode)
        {
            if (MenuCode.IsEmpty())
            {
                return Json(new { errorMsg = "error", msg = "请选择菜单" });
            }
            var flag = service.InsertRoleMenu(RoleCode1, MenuCode);

            if (flag)
                return Json(new { result = "success", msg = "添加成功" });
            return Json(new { errorMsg = "error", msg = "添加失败" });

        }
    }
}