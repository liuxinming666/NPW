/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianping
 * 日  期：2019/5/31 10:36:04
 * 描  述：VideoController视频监控
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.MapVideo.Controllers
{
    [Authorize]
    public class VideoController : EWFBaseController
    {
        private IVideoService service;

        public VideoController(IVideoService _service)
        {
            service = _service;
        }
        public IActionResult Index()
        {
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            DataTable dt = service.GetVideoList("",Convert.ToInt32(type),addvcd);
            string strTree = "<?xml version=\"1.0\" encoding=\"utf-8\"?><nodes><node id=\"1\" text=\"视频监控\" checked=\"true\">";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strTree += "<node id=\"" + dt.Rows[i]["ID"] + "\" text=\"" + dt.Rows[i]["Name"] + "\" url=\\\"javascript:flytovideo(" + dt.Rows[i]["ID"] + ");\\\" />";
            }
            strTree += "</node></nodes>";
            ViewData["strTree"] = strTree;
            return View();
        }

        /// <summary>
        /// 获取所有视频监控站点
        /// </summary>
        /// <returns></returns>
        public IActionResult GetVideoList(string ids)
        {
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var result = service.GetVideos(ids,Convert.ToInt32(type), addvcd);
            return Content(result);
        }
    }
}