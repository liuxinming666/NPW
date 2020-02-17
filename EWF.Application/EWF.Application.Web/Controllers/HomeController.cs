using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Application.Web.Models;
using EWF.IServices;
using EWF.Util.Log;
using EWF.Util.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EWF.Entity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using EWF.Util;

namespace EWF.Application.Web.Controllers
{
    [Authorize]
    public class HomeController : EWFBaseController
    {
        private IMenuService menuService;
        private IWatchWarnService watchService;
        private LoggerHelper _logger;
        private WeaterConfig weatherConfig;
        private IWeatherService weatherService;
        private ISysConfigService configservice;
        private IHostingEnvironment hostingEnvironmen;
        private IVideoService videoService;
        public HomeController(IMenuService _menuService,IWatchWarnService _watchService, LoggerHelper _loger,IWeatherService _weatherService, IOptionsSnapshot<WeaterConfig> _option, ISysConfigService _configservice, IHostingEnvironment _hostingEnvironmen, IVideoService _videoService)
        {
            menuService = _menuService;
            watchService = _watchService;
            _logger = _loger;
            weatherService = _weatherService;
            weatherConfig = _option.Get("YbWeather");
            configservice = _configservice;
            hostingEnvironmen = _hostingEnvironmen;
            videoService = _videoService;
        }
        /// <summary>
        /// 系统四块布局
        /// </summary>
        /// <returns></returns>
        public IActionResult SysIndex()
        {
            #region 读取配置信息
            var addvcd = "";var type = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            }
            if (configservice.GetSysConfigByAddvcd(addvcd).Count<SYS_Config>() > 0)
            {
                SYS_Config sys_Config = configservice.GetSysConfigByAddvcd(addvcd).Single<SYS_Config>();
                ViewBag.Sysid = sys_Config.SYSID;
                ViewBag.Stcd = sys_Config.STCD;
                ViewBag.video = sys_Config.VIDEONAME;
            }
            else
            {
                ViewBag.Sysid = "";
                ViewBag.Stcd = "";
                ViewBag.video = "";
            }
            ViewBag.VideoUrl = weatherConfig.VideoPreUrl;
            #endregion

            return View();
        }
        public IActionResult GetWeatherImage(string sType)
        {
            return Content(weatherService.GetShowWeather(sType,weatherConfig.url, weatherConfig.encode, weatherConfig.strTag));
        }
        public IActionResult Index()
        {
            //_logger.Debug("LogLevel.Trace");
            //_logger.Info("LogLevel.Debug");
            //_logger.Warn("LogLevel.Information");
            //_logger.Error("LogLevel.Warning");
            //_logger.Exception(new Exception("test"));
            //throw new Exception("test");

            //throw new Exception("测试异常");
            var userName = "";
            var addvcd = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.User.Claims.First().Value.Split(',')[0];
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                //var type= HttpContext.User.Claims.First().Value.Split(',')[2];
                var role = HttpContext.User.Claims.Last().Value;
            }
            else
            {
                return RedirectToAction("Login","Access");
            }
            #region 读取配置信息            
            if (configservice.GetSysConfigByAddvcd(addvcd).Count<SYS_Config>() > 0)
            {
                SYS_Config sys_Config = configservice.GetSysConfigByAddvcd(addvcd).Single<SYS_Config>();
                ViewBag.SConfig = sys_Config;
            }
            else
            {
                //SYS_Config syscon = configservice.GetSysByID(3);
                SYS_Config syscon = new SYS_Config();
                syscon.SYSCOL = "2394f2";
                syscon.SYSLOGO = "/_fileupload/sysPic/logosys-20190610110212766.png";
                syscon.SYSNAME = "上游局综合业务系统";
                syscon.LGLT = "113.65113410245, 34.76626768096,2000000";
                ViewBag.SConfig = syscon;
            }
            #endregion

            ViewBag.UserName = userName;
            return View();
        }

        /// <summary>
        /// 地图界面
        /// </summary>
        /// <returns></returns>
        public IActionResult MapsIndex()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); //"2019-05-24 18:00";
            var type = "";
            var addvcd = "";
            var html = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
                type = HttpContext.User.Claims.First().Value.Split(',')[2];
                html = watchService.GetWatchWarn(date, Convert.ToInt32(type), addvcd);
            }
            else
            {
                return RedirectToAction("Login", "Access");
            }
            //var html = watchService.GetWatchWarn(date, Convert.ToInt32(type), addvcd);
            ViewBag.html = html;
            return View();
        }

        /// <summary>
        /// ol,二维地图
        /// </summary>
        public IActionResult OLMapsIndex()
        {
            return View();
        }

        /// <summary>
        /// lf,二维地图
        /// </summary>
        /// <returns></returns>
        public IActionResult LFMapsIndex()
        {
            return View();
        }

        /// <summary>
        /// 三维地球测试
        /// </summary>
        /// <returns></returns>
        public IActionResult CesMapsIndex()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> GetTopMenu()
        {
            var role = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                role = HttpContext.User.Claims.Last().Value;
                return Content(menuService.GetUserTopMenu(role));
            }
            else
            {
                return RedirectToAction("Login", "Access");
            }
            //return menuService.GetUserTopMenu(role);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> GetUserMenuByParentCode(string parentCode)
        {
            var role = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                role = HttpContext.User.Claims.Last().Value;
                return Content(menuService.GetUserMenuByParentCode(role, parentCode));
            }
            else
            {
                return RedirectToAction("Login", "Access");
            }
            //return menuService.GetUserMenuByParentCode(role, parentCode);
        }

        public IActionResult LoadZYJBoundary()
        {
            string str = "";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\" + "910000" + ".geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }

        /// <summary>
        /// 加载区域边界
        /// add by SUN
        /// Date:2019-06-17 20:00
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadAreaBoundary()
        {
            string str = "";
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\"+ addvcd + ".geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载主要支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_ZL()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\zhiliuline.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载一级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Line1()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\line1.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载一级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Polygon1()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\polygon1.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载二级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Line2()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\line2.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载二级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Polygon2()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\polygon2.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载三级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Line3()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\line3.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载三级支流
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_Polygon3()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\polygon3.geojson"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        /// <summary>
        /// 加载流域边界
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadLine_HHLY()
        {
            string str = "";
            string contentRootPath = hostingEnvironmen.WebRootPath;
            using (StreamReader sr = new StreamReader(contentRootPath + @"\MapDate\hhlyline.json"))
            {
                str = sr.ReadToEnd();
            }
            return Content(str);
        }
        public IActionResult Download(string path)
        {
            //get the temp folder and file path in server
            
                string filePath = Path.Combine(hostingEnvironmen.WebRootPath +@"\temp", path);
                var stream = System.IO.File.OpenRead(filePath);
                return File(stream, "application/vnd.android.package-archive", Path.GetFileName(filePath));

            
        }
    }
}
