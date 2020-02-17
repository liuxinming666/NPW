using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.Entity;
using EWF.IServices;
using EWF.Util;
using EWF.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class UserController : EWFBaseController
    {
        private ISYS_USERService service;
        public UserController(ISYS_USERService _userService)
        {
            service = _userService;
        }
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
        /// <summary>
        /// 获取全部用户信息，没有用
        /// </summary>
        /// <returns></returns>
        public IActionResult GetUserData(int page, int rows, string username)
        {
            var role = "";
            var addvcd = "";
            int type = 0;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type= HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                role = HttpContext.User.Claims.Last().Value;
            }
            var pageObj = service.GetUserData(page, rows, username, addvcd, type);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items

            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetUserInfoData(string username, string addvcd, string type)
        {
            //var role = "";
            //var addvcd = "";
            //int type = 0;
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            //    type = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
            //    role = HttpContext.User.Claims.Last().Value;
            //}
            var data = service.GetUserList(username, addvcd, Convert.ToInt32(type != "" ? type : "0"));
            return Content(data.ToJson());
        }
        public string AddUserInfo()
        {
            SYS_USER user = new SYS_USER()
            {
                USERID = Guid.NewGuid().ToString("N").ToUpper(),
                UACCOUNT = Request.Form["UACCOUNT"].ToString().Trim(),
                REALNAME = Request.Form["REALNAME"],
                UPWD = ZEncypt.MD5(Request.Form["UPWD"]),//需用md5加密
                GENDER = Request.Form["GENDER"],
                UNITID = Request.Form["UNITID"],
                DEPARTMENTID = Request.Form["DEPARTMENTID"],
                SPELL = Request.Form["SPELL"],
                TELEPHONE = Request.Form["TELEPHONE"],
                MOBILE = Request.Form["MOBILE"],
                EMAIL = Request.Form["EMAIL"],
                ISENABLED = Request.Form["ISENABLED"].ToInt(),
                ISDUTY = Request.Form["ISDUTY"].ToInt(),
                TYPE = Request.Form["TYPE"].ToInt(),
                ADDVCD = Request.Form["XIAN"].ToString() == "" ? Request.Form["CITY"].ToString() : Request.Form["XIAN"].ToString(),
                CREATEDATE = DateTime.Now
            };           

            string result = service.Insert(user);
            return result;
        }        

        public string EditUserInfo()
        {
            var Id = Request.Form["USERID"];
            SYS_USER user = service.GetUserByID(Id);
            user.UACCOUNT = Request.Form["UACCOUNT"];
            user.REALNAME = Request.Form["REALNAME"];
            //user.UPWD = ZEncypt.MD5(Request.Form["UPWD"]);//需用md5加密
            user.GENDER = Request.Form["GENDER"];
            user.UNITID = Request.Form["UNITID"];
            user.DEPARTMENTID = Request.Form["DEPARTMENTID"];
            user.SPELL = Request.Form["SPELL"];
            user.TELEPHONE = Request.Form["TELEPHONE"];
            user.MOBILE = Request.Form["MOBILE"];
            user.EMAIL = Request.Form["EMAIL"];
            user.ISENABLED= Request.Form["ISENABLED"].ToInt();
            user.ISDUTY = Request.Form["ISDUTY"].ToInt();
            user.TYPE = Request.Form["TYPE"].ToInt();
            user.ADDVCD = Request.Form["XIAN"].ToString() == "" ? Request.Form["CITY"].ToString() : Request.Form["XIAN"].ToString();
            user.MODIFYDATE = DateTime.Now;
            string result = service.Update(user);
            return result;
        }

        public string DeleteUserInfo(string Id)
        {
            string result = service.Delete(Id);
            return result;
        }

        public string ResetUserPwInfo(string userid)
        {
            SYS_USER user = service.GetUserByID(userid);
            user.UPWD = ZEncypt.MD5("123456");//需用md5加密
            user.CHANGEPASSWORDDATE = DateTime.Now;
            string result = service.Update(user);
            return result;
        }
        //获取所有角色信息
        public string GetRoleAll()
        {
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3]; 
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var role = HttpContext.User.Claims.Last().Value;
            //超级管理员获取所有角色信息，其他不能获取超级管理员角色
            var rolelist = service.GetAllRole(role, addvcd, type.ToString(),"").ToList<SYS_ROLE>();
            if (rolelist.Count > 0)
            {
                var data = service.GetAllRole("", addvcd, type.ToString(), "").ToList<SYS_ROLE>();
                return data.ToJson();
            }
            else
            {
                var rolename = "不是超级管理员";
                var data = service.GetAllRole("", addvcd, type.ToString(), rolename).ToList<SYS_ROLE>();
                return data.ToJson();
            }
            
        }
        //获取用户角色信息
        public string GetUserRoleCode(string userid)
        {
            var userrolelist = service.GetRoleByUser(userid);
            return userrolelist.ToJson();
        }        

        public string SaveSetRole(string userid, string roleids)
        {
            if (roleids.IsEmpty())
            {
                return "请选择角色";
            }
            service.SaveUserRole(roleids.Split(','), userid);
            return "授权成功";
        }
        //获取单位信息
        public string GetUnitData()
        {
            //判断是不是超级管理员权限，超级管理员可以添加任何单位，其他管理员和普通人员只能添加自己所在单位
            var role = "";
            var uaccount = "";
            var addvcd = "";
            int type = 0;
            var unitid = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                uaccount = HttpContext.User.Claims.First().Value.Split(',')[0];
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                role = HttpContext.User.Claims.Last().Value;
                unitid= HttpContext.User.Claims.ToList()[1].Value.ToString();
            }
            var rolelist = service.GetAllRole(role, addvcd, type.ToString(),"").ToList<SYS_ROLE>();
            if (rolelist.Count > 0)
            {
                var unitData = service.GetUnitData().ToJson();
                return unitData;
            }
            else
            {
                //根据用户所在单位取出自己单位
                var unitData = service.GetUnitDataById(unitid).ToJson();
                unitData = "[" + unitData + "]";
                return unitData;
            }
        }
        //获取部门信息
        public string GetDepartmentData(string unitid)
        {
            var deptData = service.GetDepartmenttList(unitid).ToJson();
            return deptData;
        }

        //获取行政区划市,如果是超级管理员显示全部，不是只显示所在流域分区
        public string GetCity(int type=1)
        {
            //var cityData = service.GetCityData(type).ToJson();
            //return cityData;

            //判断是不是超级管理员权限，超级管理员可以添加任何区域，其他管理员和普通人员只能添加自己所在区域
            var role = "";
            var addvcd = "";
            int atype = 0;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                atype = HttpContext.User.Claims.First().Value.Split(',')[2].ToInt();
                role = HttpContext.User.Claims.Last().Value;
            }
            var rolelist = service.GetAllRole(role, addvcd, atype.ToString(),"").ToList<SYS_ROLE>();
            if (rolelist.Count > 0)
            {
                var cityData = service.GetCityData(type).ToJson();
                return cityData;
            }
            else
            {
                var cityData = service.GetAddvcdData(addvcd, atype).ToJson();

                return cityData;
            }
        }
        public string GetXian(string cityid, int type=1)
        {
            var xianData = service.GetXianData(cityid, type).ToJson();
            return xianData;
        }

    }
}