/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/5/28 16:24:11
 * 描  述：WeatherCloudyController 卫星云图
 * *********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.RealData.Controllers
{
    [Authorize]
    public class WeatherCloudyController : EWFBaseController
    {       
        #region 卫星云图
        public IActionResult Index()
        {
            /******************* 抓取网页源代码数据start **********************/
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create("http://www.nmc.cn/publish/satellite/FY4A-true-color.htm");
            StreamReader streamr = new StreamReader(hwr.GetResponse().GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string pageData = streamr.ReadToEnd();
            /******************* 抓取网页源代码数据end **********************/
            string newData = GetStr(pageData, "var data_index = new Array()", "</script>");
            ViewBag.message = newData;
            return View();
        }
        //字符串截取，从网页源码中截取 两字符串中间信息
        private string GetStr(string TxtStr, string FirstStr, string SecondStr)
        {
            if (FirstStr.IndexOf(SecondStr, 0) != -1)
                return "";
            int FirstSite = TxtStr.IndexOf(FirstStr, 0);
            int SecondSite = TxtStr.IndexOf(SecondStr, FirstSite + 1);
            if (FirstSite == -1 || SecondSite == -1)
                return "";
            return TxtStr.Substring(FirstSite, SecondSite - FirstSite - FirstStr.Length +28).TrimEnd();
        }
        #endregion

        #region 天气预报
        public IActionResult Weather()
        {
            /******************* 抓取网页源代码数据start **********************/
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create("http://www.nmc.cn/publish/precipitation/1-day.html");
            StreamReader streamr = new StreamReader(hwr.GetResponse().GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string pageData = streamr.ReadToEnd();
            /******************* 抓取网页源代码数据end **********************/
            string newData = GetStr(pageData, "var data_index = new Array();", "</script>");
            ViewBag.message = newData;
            return View();
        }

        #endregion
        //首页第二板块点击全屏展示
        public IActionResult WeatherImage(string src)
        {
            ViewData["src"] = src;
            return View();
        }
    }
}