/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/5/31 10:36:04
 * 描  述：CameraController
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using Newtonsoft.Json;
using System.Data;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using EWF.Util.Options;
using Microsoft.Extensions.Options;

namespace EWF.Application.Web.Areas.MapVideo.Controllers
{
    [Authorize]
    public class CameraController : EWFBaseController
    {
        private IVideoService service;
        private IHostingEnvironment hostingEnvironmen;
        private ISYS_VIDEOMANAGEService videoService;
        private IWeatherService weatherService;
        private WeaterConfig weatherConfig;
        public CameraController(IVideoService _service, IHostingEnvironment _hostingEnvironme,ISYS_VIDEOMANAGEService _videoservice, IWeatherService _weatherService, IOptionsSnapshot<WeaterConfig> _option)
        {
            service = _service;
            hostingEnvironmen = _hostingEnvironme;
            videoService = _videoservice;
            weatherService = _weatherService;
            weatherConfig = _option.Get("YbWeather");
        }
        #region 废弃
        /// <summary>
        /// 海康平台
        /// </summary>
        /// <param name="vname"></param>
        /// <param name="vpwd"></param>
        /// <param name="vip"></param>
        /// <param name="vport"></param>
        /// <returns></returns>
        public IActionResult Index(string vname,string vpwd,string vip,string vport)
        {
            string name="", pwd="", ip="",port = "";
            if (vname != "undefined")
                name = vname;
            if (vpwd != "undefined")
                pwd = vpwd;
            if (vip != "undefined")
                ip = vip;
            if (vport != "undefined")
                port = vport;
            ViewData["vname"] = name;
            ViewData["vpwd"] = pwd;
            ViewData["vip"] = ip;
            ViewData["vport"] = port;
            return View();
        }
        #endregion
        /// <summary>
        /// 视频主页面
        /// </summary>
        /// <param name="stcd"></param>
        /// <returns></returns>
        public IActionResult VideoInfo(string stcd)
        {
            ViewData["stcd"] = stcd;            
            return View();
        }
       
        /// <summary>
        /// 获取视频监控站点配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetVideoManageList(string stcd)
        {
            DataTable dt = service.GetVideoManageList(stcd);
            var result = JsonConvert.SerializeObject(dt);
            return Content(result);
        }
        #region 各个视频平台页面
        /// <summary>
        /// 海康平台
        /// </summary>
        /// <returns></returns>
        public IActionResult videoHKPT()
        {
            return View();
        }
        public IActionResult videoHKPTNew(string stcd)
        {
            #region 读取首页视频
            //var addvcd = ""; var type = "";
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    type = HttpContext.User.Claims.First().Value.Split(',')[2];
            //    addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            //}
            //var rootpath = hostingEnvironmen.WebRootPath;
            //var path = rootpath + "\\FastVideo\\Video.xml";
            //XDocument document = XDocument.Load(path);
            ////获取到XML的根元素进行操作
            //XElement root = document.Root;
            //XElement Videos = root.Element("Videos");
            ////获取Videos元素下的所有子元素
            //IEnumerable<XElement> enumerable = Videos.Elements();
            //List<string> videolist = new List<string>();
            //foreach (XElement item in enumerable)
            //{
            //    if (item.Name == "video" && item.Attribute("type").Value == type && item.Attribute("addvcd").Value == addvcd)
            //    {
            //        if (item.Attribute("id").Value == stcd)
            //        {
            //            videolist.Add(item.Element("videoname").Value);
            //        }
            //    }
            //}
            //ViewBag.VideoList = videolist;
            //ViewBag.VideoUrl = "http://10.4.94.131:8586";
            #endregion

            #region 读取数据库版本
            List<string> videolist = new List<string>();
            var videonamelist = videoService.GetCameraListBySTCD(stcd).ToList();

            foreach (var item in videonamelist)
            {
                videolist.Add(item.SNAME);
            }
            ViewBag.VideoList = videolist;
            ViewBag.VideoUrl = weatherConfig.VideoPreUrl;
            #endregion
            return View();
        }
        /// <summary>
        /// 大华平台
        /// </summary>
        /// <returns></returns>
        public IActionResult videoDHPTNew()
        {
            //DataTable dt = service.GetVideoManageList(stcd);
            //var result = dt.ToJson();
            //ViewData["data"] = result;
            return View();
        }
        #endregion
    }
}