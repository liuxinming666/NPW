using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using Microsoft.AspNetCore.Mvc;
using EWF.Util;
using EWF.Entity;
using EWF.Util.Log;
using Microsoft.AspNetCore.Authorization;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    //水情-对比分析
    public class RvavManageController : EWFBaseController
    {
        private IRvavService service;
        private LoggerHelper logger;
        public RvavManageController(IRvavService _service, LoggerHelper _logger)
        {
            service = _service;
            logger = _logger;
        }


        //URL:/HistoryInfo/RvavManage/Delete

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult GetPageData(string stcd,string avgType, int rows = 20, int page = 1)
        {
            var addvcd = HttpContext.User.Claims.First().Value.Split(',')[3];
            var type = HttpContext.User.Claims.First().Value.Split(',')[2];
            var pageList = service.GetPageData(page, rows, stcd, avgType, addvcd, type);

            var data = new
            {
                total = pageList.TotalItems,
                rows = pageList.Items
            };
            return Content(data.ToJson());
        }

        public IActionResult Add(ST_RVAVEntity entity)
        {
            try
            {
                service.Insert(entity);
                return Json(new { result = "success", msg = "添加成功" });
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                return Json(new { errorMsg = "error", msg = "添加失败:不能重复添加数据" });
            }
        }

        public IActionResult Edit(ST_RVAVEntity entity)
        {
            if (service.Update(entity))
                return Json(new { result = "success", msg = "修改成功" });

            return Json(new { errorMsg = "error", msg = "修改失败" });
        }

        public IActionResult Delete(ST_RVAVEntity entity)
        {
            var result = service.Delete(entity);
            if (result)
                return Success("删除成功");

            return Error("删除失败");
        }

    }
}