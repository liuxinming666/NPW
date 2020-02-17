/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/6/4 14:45:11
 * 描  述：视频站点摄像头维护
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Entity;
using EWF.IServices;
using EWF.Application.Web.Controllers;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using EWF.Util.Log;
using System.Diagnostics;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class VideoManageNewController : EWFBaseController
    {
        private ISYS_VIDEOMANAGEService service;
        private LoggerHelper _logger;
        public VideoManageNewController(ISYS_VIDEOMANAGEService _videoService, LoggerHelper _loger)
        {
            service = _videoService;
            _logger = _loger;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取全部视频信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCameraData(int page, int rows, string name)
        {
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var pageObj = service.GetCameraData(page, rows, name, Convert.ToInt32(type), addvcd);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());
        }

        public JsonResult AddCameraInfo([FromServices]IHostingEnvironment env, string STCD)
        {
            SYS_VIDEOMANAGE video = new SYS_VIDEOMANAGE();
            if (string.IsNullOrWhiteSpace(Request.Form["ID"]))
            {
                video.STCD = STCD;
                video.SNAME = Request.Form["SNAME"];
                video.SIP = Request.Form["SIP"];
                video.SPORT = Request.Form["SPORT"];
                video.USERNAME = Request.Form["USERNAME"];
                video.PASSWORD = Request.Form["PASSWORD"];
                video.PASSWAY = Request.Form["PASSWAY"];
                video.LINETYPE =Request.Form["LINETYPE"];
                video.SEBLONG = Request.Form["SEBLONG"];
                video.REMARKS = Request.Form["REMARKS"];
                //if (!string.IsNullOrWhiteSpace(Request.Form["MAINPAGE"]))
                //    video.MAINPAGE = Convert.ToBoolean(Convert.ToInt32(Request.Form["MAINPAGE"]));
                //else
                //    video.MAINPAGE = false;
            }
            var videonamelist = service.GetCameraNameList(video.SNAME);
            if (videonamelist != null)
            {
                if (videonamelist.ToList().Count > 0)
                    return Json(new { errorMsg = "error", msg = "该摄像头已添加过，请勿重复添加！" });
            }
            var videolist = service.GetCameraList(STCD, video.SNAME, video.SIP, video.SPORT);
            if (videolist != null)
            {
                if (videolist.ToList().Count > 0)
                    return Json(new { errorMsg = "error", msg = "该摄像头该端口号已添加过，请勿重复添加！" });
            }
            var result = service.Insert(video);
            if (result > 0)
            {
                #region 添加到Video.xml
                //var type = HttpContext.User.Claims.First().Value.Split(',')[2];
                //var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];

                //var rootpath = env.WebRootPath;
                //var path = rootpath + "\\FastVideo\\Video.xml";
                //_logger.Info("添加视频xml的路径："+path);
                //XDocument document = XDocument.Load(path);
                ////获取到XML的根元素进行操作
                //XElement root = document.Root;
                //XElement Videos = root.Element("Videos");                
                //XElement videoele = new XElement("video");
                //videoele.SetAttributeValue("vid", result);
                //videoele.SetAttributeValue("id", STCD);
                //videoele.SetAttributeValue("type", type);
                //videoele.SetAttributeValue("addvcd", addvcd);
                ////if (Request.Form["MAINPAGE"].ToString() == "1")
                ////    videoele.SetAttributeValue("mainpage", "1");
                ////else
                ////    videoele.SetAttributeValue("mainpage", "0");

                //videoele.SetElementValue("videotype", "2");
                //videoele.SetElementValue("videoname", video.SNAME);
                //videoele.SetElementValue("connecttype", "1");
                //videoele.SetElementValue("videoaddress", video.SIP);
                //videoele.SetElementValue("videoport", video.SPORT);
                //videoele.SetElementValue("username", video.USERNAME);
                //videoele.SetElementValue("password", video.PASSWORD);
                //videoele.SetElementValue("channel", video.PASSWAY);
                //Videos.Add(videoele);
                //root.Save(path);


                #endregion

                #region 启动bat批处理文件
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe"; //命令
                    p.StartInfo.UseShellExecute = false; //不启用shell启动进程
                    p.StartInfo.RedirectStandardInput = true; // 重定向输入
                    p.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
                    p.StartInfo.RedirectStandardError = true; // 重定向错误输出 
                    p.StartInfo.CreateNoWindow = true; // 不创建新窗口
                    p.Start();
                    p.StandardInput.WriteLine(@"cd C:\Windows\system32"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc stop FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc start FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("exit"); //退出
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                #endregion
                return Json(new { result = "success", msg = "添加成功" });
            }
                
            else
                return Json(new { errorMsg = "error", msg = "添加失败" });
        }

        public JsonResult EditCameraInfo([FromServices]IHostingEnvironment env, string STCD)
        {
            var Id = Request.Form["ID"];
            SYS_VIDEOMANAGE video = service.GetCameraByID(Convert.ToInt32(Id));
            video.STCD = STCD;
            video.SNAME = Request.Form["SNAME"];
            video.SIP = Request.Form["SIP"];
            video.SPORT = Request.Form["SPORT"];
            video.USERNAME = Request.Form["USERNAME"];
            video.PASSWORD = Request.Form["PASSWORD"];
            video.PASSWAY = Request.Form["PASSWAY"];
            video.LINETYPE = Request.Form["LINETYPE"];
            video.SEBLONG = Request.Form["SEBLONG"];
            video.REMARKS = Request.Form["REMARKS"];
            //if (!string.IsNullOrWhiteSpace(Request.Form["MAINPAGE"]))
            //    video.MAINPAGE = Convert.ToBoolean(Convert.ToInt32(Request.Form["MAINPAGE"]));
            //else
            //    video.MAINPAGE = false;

            if (service.Update(video))
            {
                #region 修改Video.xml里面的节点
                //var rootpath = env.WebRootPath;
                //var path = rootpath + "\\FastVideo\\Video.xml";
                //XDocument document = XDocument.Load(path);
                ////获取到XML的根元素进行操作
                //XElement root = document.Root;
                //XElement Videos = root.Element("Videos");
                ////获取Videos元素下的所有子元素
                //IEnumerable<XElement> enumerable = Videos.Elements();
                //foreach (XElement item in enumerable)
                //{
                //    if (item.Name == "video" && !item.Attribute("vid").IsNullOrEmpty())
                //    {
                //        if (item.Attribute("vid").Value == Id)
                //        {
                //            item.Attribute("id").Value = STCD;
                //            //if (Request.Form["MAINPAGE"].ToString() == "1")
                //            //    item.Attribute("mainpage").Value = "1";
                //            //else
                //            //    item.Attribute("mainpage").Value = "0";
                //            item.SetElementValue("videoname", video.SNAME);
                //            item.SetElementValue("videoaddress", video.SIP);
                //            item.SetElementValue("videoport", video.SPORT);
                //            item.SetElementValue("username", video.USERNAME);
                //            item.SetElementValue("password", video.PASSWORD);
                //            item.SetElementValue("channel", video.PASSWAY);
                //        }
                //    }
                //}
                //document.Save(path);

                #endregion

                #region 启动bat批处理文件
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe"; //命令
                    p.StartInfo.UseShellExecute = false; //不启用shell启动进程
                    p.StartInfo.RedirectStandardInput = true; // 重定向输入
                    p.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
                    p.StartInfo.RedirectStandardError = true; // 重定向错误输出 
                    p.StartInfo.CreateNoWindow = true; // 不创建新窗口
                    p.Start();
                    p.StandardInput.WriteLine(@"cd C:\Windows\system32"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc stop FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc start FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("exit"); //退出
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);   
                }
                #endregion
                return Json(new { result = "success", msg = "修改成功" });
            }

            return Json(new { errorMsg = "error", msg = "修改失败" });
        }

        public JsonResult DeleteCameraInfo([FromServices]IHostingEnvironment env, string Id)
        {
            if (service.Delete(Convert.ToInt32(Id)))
            {
                #region 删除Video.xml里面的节点
                //var rootpath = env.WebRootPath;
                //var path = rootpath + "\\FastVideo\\Video.xml";
                //XDocument document = XDocument.Load(path);
                ////获取到XML的根元素进行操作
                //XElement root = document.Root;
                //XElement Videos = root.Element("Videos");               
                ////获取Videos元素下的所有子元素
                //IEnumerable<XElement> enumerable = Videos.Elements();
                //foreach (XElement item in enumerable)
                //{
                //    if (item.Name == "video" && !item.Attribute("vid").IsNullOrEmpty())
                //    {
                //        if(item.Attribute("vid").Value == Id)
                //            item.Remove();
                //    }                        
                //}
                //document.Save(path);
                #endregion

                #region 启动bat批处理文件
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe"; //命令
                    p.StartInfo.UseShellExecute = false; //不启用shell启动进程
                    p.StartInfo.RedirectStandardInput = true; // 重定向输入
                    p.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
                    p.StartInfo.RedirectStandardError = true; // 重定向错误输出 
                    p.StartInfo.CreateNoWindow = true; // 不创建新窗口
                    p.Start();
                    p.StandardInput.WriteLine(@"cd C:\Windows\system32"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc stop FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("sc start FastVideo"); //cmd执行的语句
                    p.StandardInput.WriteLine("exit"); //退出
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
                #endregion
                return Json(new { result = "success", msg = "删除成功" });
            }                

            return Json(new { errorMsg = "error", msg = "删除失败" });
        }
    }
}