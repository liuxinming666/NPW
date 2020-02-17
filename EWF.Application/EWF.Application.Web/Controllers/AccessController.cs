using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EWF.IServices;
using EWF.Util;
using System.Data;
using EWF.Entity;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace EWF.Application.Web.Controllers
{
    public class AccessController : Controller
    {
        private ILoginService loginService;
        private IHttpContextAccessor _httpContextAccessor;

        public AccessController(ILoginService _loginService, IHttpContextAccessor httpContextAccessor)
        {
            loginService = _loginService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[AllowAnonymous]
        public IActionResult Login()
        {
            var userName = "";
            var upwd = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.User.Claims.First().Value.Split(',')[0];
                upwd = HttpContext.User.Claims.First().Value.Split(',')[1];
            }
            string userip = GetIP(HttpContext);
            var item = loginService.GetBackUser(userip);
            if (item != null && !String.IsNullOrEmpty(item.USERIP))
            {
                if (item.BULAYER == "3")
                    return RedirectToAction("Error3");
                else if (item.BULAYER == "2")
                    return RedirectToAction("Error2");
                else if (item.BULAYER == "1" && item.BUDATE > DateTime.Now.AddMinutes(-30))
                    return RedirectToAction("Error1");
            }
            ViewBag.NameLen = 30;
            ViewBag.username = userName;
            ViewBag.upwd = upwd;
            return View();
        }
        
        [Route("Access/SignIn")]  
        public async Task<string> SignInAsync(string username, string userpwd)
        {
            string userIp = GetIP(HttpContext);
            string roleid = "";
            DataTable dtCheckUser = new DataTable();
            string loginMsg = loginService.LoginCheck(username, userpwd, userIp, Request.Host.ToString(), Request.Path.ToString(), out dtCheckUser, out roleid);
            if (!loginMsg.Contains("success"))
                return loginMsg;
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username + "," + userpwd + "," +dtCheckUser.Rows[0]["TYPE"].ToString()+","+ dtCheckUser.Rows[0]["ADDVCD"].ToString()),
                    new Claim("UnitID",dtCheckUser.Rows[0]["UNITID"].ToString()),
                    new Claim(ClaimTypes.Role, roleid),
                };

                var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }
            return loginMsg;
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        //注销用户登录信息        
        [Route("Access/SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
       

        protected IActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
        protected IActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }

        #region 黑名单错误页面
        public IActionResult Error1()
        {
            return View();
        }
        public IActionResult Error2()
        {
            return View();
        }
        public IActionResult Error3()
        {
            return View();
        }
        #endregion

        #region 获取客户端IP地址
        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetIP(Microsoft.AspNetCore.Http.HttpContext content)
        {
            string userHostAddress = content.Connection.RemoteIpAddress.ToString();

            if (userHostAddress == "::1")
            {
                userHostAddress = GetClientIPv4Address(userHostAddress);
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        private static string GetClientIPv4Address(string clientIP)
        {
            string ipv4 = String.Empty;

            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostAddresses(clientIP))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (System.Net.IPAddress ip in System.Net.Dns.GetHostEntry(clientIP).AddressList)
            //foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }

            return ipv4;
        }
        #endregion
    }
}