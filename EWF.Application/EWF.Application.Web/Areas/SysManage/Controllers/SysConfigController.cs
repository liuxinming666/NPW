using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.Entity;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class SysConfigController : EWFBaseController
    {
        private ISysConfigService service;
        public SysConfigController(ISysConfigService _sysService)
        {
            service = _sysService;
        }

        public IActionResult SysConfigPage()
        {
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            if (service.GetSysConfigByAddvcd(addvcd).Count<SYS_Config>() > 0)
            {
                SYS_Config sys_Config = service.GetSysConfigByAddvcd(addvcd).Single<SYS_Config>();
                ViewBag.SConfig = sys_Config;
            }
            else {
                SYS_Config syscon = service.GetSysByID(3);
                //SYS_Config syscon = new SYS_Config();
                syscon.SYSID = 0;
                ViewBag.SConfig = syscon;
            }
            return View();
        }
        public string EditSysConfigInfo(SYS_Config sconfig)
        {
            //SYS_Config syscon = service.GetSysByID("1");
            //syscon.SYSNAME = Request.Form["SYSNAME"];
            //syscon.SYSLOGO = Request.Form["FILE_URL"];

            //syscon.SYSBGPIC = Request.Form["FILE_URLPIC"];

            //syscon.SYSCONTENT = Request.Form["SYSCONTENT"];
            //syscon.SYSCOL = pic;
            sconfig.STCD = Request.Form["stcds"];
            sconfig.VIDEONAME = Request.Form["VIDEONAME1"];
            sconfig.ADDVCD= HttpContext.User.Claims.First().Value.Split(',')[3];
            string result = "";
            if (sconfig.SYSID > 0)
                result = service.Update(sconfig);
            else
                result = service.Insert(sconfig);
            return result;
        }
        //获取
        public IActionResult GetStnmBySysconfig()
        {
            var unit = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                unit = HttpContext.User.Claims.First().Value.Split(',')[3];
                var list = service.GetStnmBySysconfig(unit);
                var data = new
                {
                    total = list.Count(),
                    rows = list

                };
                return Content(data.ToJson());
            }
            return Content("暂无数据");
        }
        public string updateSysConfigStcd(string stcds, string sysid)
        {
            SYS_Config sconfig = new SYS_Config();
            sconfig = service.GetSysByID(sysid.ToInt());
            sconfig.STCD = Request.Form["stcds"];// stcds;
            string result = "";
            result = service.Update(sconfig);
            return result;
        }
        //更新系统配置的视频站点摄像头
        public string updateSysConfigVideo(int stype, string videoname, string sysid)
        {
            SYS_Config sconfig = new SYS_Config();
            sconfig = service.GetSysByID(sysid.ToInt());
            var video = sconfig.VIDEONAME;
            //1表示选择保存的视频，0表示单个关闭的
            if (stype == 1)
            {
                sconfig.VIDEONAME = Request.Form["VIDEONAME1"];
            }
            else
            {
                if (video.IndexOf(videoname) > -1)
                {
                    video = video.Replace(videoname, "");
                    video = video.Replace(",,", ",");
                    if (video.StartsWith(","))
                        video = video.Substring(1, video.Length - 1);
                    if (video.EndsWith(","))
                        video = video.Substring(0, video.Length - 1);
                    sconfig.VIDEONAME = video;
                }
            }
            string result = "";
            result = service.Update(sconfig);
            return result;
        }
        //移除选项卡
        public string updateSysConfigStcdByStnm(int stype, string stnm, string sysid)
        {
            //1表示选择保存的水文站，0表示单个关闭的
            SYS_Config sconfig = new SYS_Config();
            sconfig = service.GetSysByID(sysid.ToInt());
            if (stype == 1)
            {
                sconfig.STCD = Request.Form["stcds"];
            }
            else
            {
                var stcds = sconfig.STCD;
                var stcd = "";
                DataTable dt = service.GetStationListByStnm(stnm);
                if (dt.Rows.Count > 0)
                {
                    stcd = dt.Rows[0]["STCD"].ToString();
                    stcds = stcds.Replace(stcd, "");
                    stcds = stcds.Replace(",,", ",");
                    if (stcds.StartsWith(","))
                        stcds = stcds.Substring(1, stcds.Length - 1);
                    if (stcds.EndsWith(","))
                        stcds = stcds.Substring(0, stcds.Length - 1);
                }
                sconfig.STCD = stcds;
            }
            string result = "";
            result = service.Update(sconfig);
            return result;
        }
        public string getConfigStcd()
        {
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            SYS_Config sys_Config = service.GetSysConfigByAddvcd(addvcd).Single<SYS_Config>();
            string stcd = sys_Config.STCD;
            return stcd;
        }
        public IActionResult GetSysConfigData(int stcd)
        {

            var lineDB = service.GetSysConfigData(stcd);
            var data = new
            {
                total = lineDB.Count(),
                rows = lineDB

            };
            return Content(data.ToJson());
        }


        [HttpPost]
        public IActionResult Upload([FromServices]IHostingEnvironment env, IFormFile file)
        {
            var _extensions = "png||jpg";
            // 如果没有上传文件
            if (file == null || string.IsNullOrEmpty(file.FileName) || file.Length == 0)
            {
                return Error("没有选择上传文件！");
            }

            //获取用户上传文件的文件名
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = System.IO.Path.GetExtension(file.FileName);

            var newFileName = fileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + fileExtension;
            //虚拟路径
            string virtualPath = string.Format("/_fileupload/sysPic/{0}", newFileName);
            string t= fileExtension.Substring(1, fileExtension.Length - 1);
            if (!_extensions.Contains(fileExtension.Substring(1, fileExtension.Length - 1)))
            {
                return Error("请选择png格式的图片");
            }


            var rootpath = env.WebRootPath;
            var path = rootpath + "\\_fileupload\\sysPic\\" + newFileName;

            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }


            var data = new { FilePath = virtualPath };
            return Success("上传成功", data);
        }
        [HttpGet]
        public ActionResult DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                else
                {
                    return Json(new { result = "success", msg = "文件不存在" });
                }

                return Json(new { result = "success", msg = "文件删除成功" });
            }
            catch
            {
                return Json(new { result = "error", msg = "文件删除失败" });
            }
        }

        //获取视频站点摄像头树状信息
        public IActionResult GetVideoManageData()
        {
            string addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            int type = HttpContext.User.Claims.First().Value.Split(',')[2] != "" ? HttpContext.User.Claims.First().Value.Split(',')[2].ToInt() : 0;
            string list = service.getVideoManageList(addvcd, type);
            return Content(list);
        }
    }
}
