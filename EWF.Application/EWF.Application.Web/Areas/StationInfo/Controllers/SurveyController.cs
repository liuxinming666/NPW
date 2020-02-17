using System;
using System.Collections.Generic;
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

namespace EWF.Application.Web.Areas.StationInfo.Controllers
{
    [Authorize]
    public class SurveyController : EWFBaseController
    {
        //URL:/StationInfo/Survey/Add
        ISTATIONINFO_SURVEYService service;
        public SurveyController(ISTATIONINFO_SURVEYService _service)
        {
            service = _service;
        }

        public IActionResult Index(string STCD)
        {
            var list = service.GetModelBySTCD(STCD).ToList();
            if (list != null && list.Count > 0)
            {
                ViewBag.file_url = list[0].FILE_URL;
            }
            else
            {
                ViewBag.file_url = "";
            }
            return View(nameof(PDFView));
        }

        public IActionResult PDFView(string file_url)
        {
            ViewBag.file_url = file_url;
            return View();
        }


        public IActionResult List()
        {
            return View();
        }

        public IActionResult GetListData(string stnm, int rows = 20, int page = 1)
        {
            var pageList = service.GetPageData(page, rows, stnm);

            var data = new
            {
                total = pageList.TotalItems,
                rows = pageList.Items
            };

            return Content(data.ToJson());
        }

        public IActionResult Add(STATIONINFO_SURVEY entity)
        {
            if (service.Count(entity.STCD) > 0)
            {
                return Json(new { errorMsg = "error", msg = "添加失败:该站已经上传了测站概况文档，不能重复上传" });
            }

            if (service.Insert(entity))
                return Json(new { result = "success", msg = "添加成功" });

            return Json(new { errorMsg = "error", msg = "添加失败" });
        }

        public IActionResult Edit(STATIONINFO_SURVEY entity)
        {
            var model = service.Get(entity.ID);
            if (model.STCD != entity.STCD)
            {
                if (service.Count(entity.STCD) > 0)
                {
                    return Json(new { errorMsg = "error", msg = "修改失败:该站已经上传了测站概况文档，不能重复上传" });
                }
            }

            if (service.Update(entity))
                return Json(new { result = "success", msg = "修改成功" });

            return Json(new { errorMsg = "error", msg = "修改失败" });
        }

        public IActionResult Delete(int ID)
        {
            if (service.Delete(ID))
                return Json(new { result = "success", msg = "删除成功" });

            return Json(new { errorMsg = "error", msg = "删除失败" });
        }

        [HttpPost]
        public IActionResult Upload([FromServices]IHostingEnvironment env, IFormFile file)
        {
            var _extensions = "pdf";
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
            string virtualPath = string.Format("/_fileupload/Survey/{0}", newFileName);

            if (!_extensions.Contains(fileExtension.Substring(1, fileExtension.Length - 1)))
            {
                return Error("请选择PFD文档");
            }


            var rootpath = env.WebRootPath;
            var path = rootpath + "\\_fileupload\\Survey\\" + newFileName;

            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }


            var data = new { FilePath = virtualPath };
            return Success("上传成功", data);
        }

        [HttpGet]
        public ActionResult DeleteFile([FromServices]IHostingEnvironment env, string filePath)
        {
            try
            {
                if (System.IO.File.Exists(env.WebRootPath + filePath))
                {
                    System.IO.File.Delete(env.WebRootPath + filePath);
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
    }
}