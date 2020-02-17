/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/6/4 14:45:11
 * 描  述：视频站点维护
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Entity;
using EWF.Application.Web.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.SysManage.Controllers
{
    [Authorize]
    public class VideoSiteController : EWFBaseController
    {
        private ISYS_VIDEOService service;
        public VideoSiteController(ISYS_VIDEOService _userService)
        {
            service = _userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取全部视频信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetVideoData(int page, int rows, string name)
        {
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var pageObj = service.GetVideoData(page, rows, name,Convert.ToInt32(type),addvcd);

            var data = new
            {
                total = pageObj.TotalItems,
                rows = pageObj.Items
            };
            return Content(data.ToJson());
        }

        public JsonResult AddVideoInfo(string STCD,string NAME)
        {
            SYS_VIDEO video = new SYS_VIDEO();
            if (string.IsNullOrWhiteSpace(Request.Form["ID"]))
            {
                video.STCD = STCD;
                video.NAME = NAME;
                video.LGTD = Convert.ToDecimal(Request.Form["LGTD"]);
                video.LTTD = Convert.ToDecimal(Request.Form["LTTD"]);
            }
            var videolist = service.GetVideoBySTCD(STCD);
            if (videolist != null)
            {
                if(videolist.ToList().Count > 0)
                    return Json(new { errorMsg = "error", msg = "该站点已添加过，请勿重复添加！" });
            }
            
            if (service.Insert(video))
                return Json(new { result = "success", msg = "添加成功" });
            else
                return Json(new { errorMsg = "error", msg = "添加失败" });
        }

        public JsonResult EditVideoInfo(string STCD,string NAME)
        {
            var Id = Request.Form["ID"];
            SYS_VIDEO video = service.GetVideoByID(Convert.ToInt32(Id));
            video.STCD = STCD;
            video.NAME = NAME;
            video.LGTD =Convert.ToDecimal(Request.Form["LGTD"]);
            video.LTTD = Convert.ToDecimal(Request.Form["LTTD"]);

            if (service.Update(video))
                return Json(new { result = "success", msg = "修改成功" });

            return Json(new { errorMsg = "error", msg = "修改失败" });
        }

        public JsonResult DeleteVideoInfo(string Id)
        {
            if (service.Delete(Convert.ToInt32(Id)))
                return Json(new { result = "success", msg = "删除成功" });

            return Json(new { errorMsg = "error", msg = "删除失败" });
        }
    }
}