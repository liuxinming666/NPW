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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.StationInfo.Controllers
{
    [Authorize]
    public class ShowController : EWFBaseController
    {
        ISTATIONINFO_SHOWService service;
        ISTATIONINFO_SHOWIMGService imgservice;
        public ShowController(ISTATIONINFO_SHOWService _service, ISTATIONINFO_SHOWIMGService _imgservice)
        {
            service = _service;
            imgservice = _imgservice;
        }
        public IActionResult Index(string STCD)
        {
            var list = service.GetModelBySTCD(STCD).ToList();
            List<string> picname = new List<string>();
            if (list != null && list.Count > 0)
            {
                ViewBag.VideoURL = list[0].Video_URL;
                var imglist = imgservice.GetModelById(list[0].ID).ToList();                
                foreach (var item in imglist)
                {
                    picname.Add(item.file_Path);
                }
                if (imglist.Count > 0)
                    ViewData["PicURL"] = picname;
            }
            else
            {
                ViewBag.VideoURL = "";
                ViewData["PicURL"] = picname;
            }
            return View();
        }
        

        public IActionResult GetPicInfo(string stcd)
        {
            //List<string> picname = new List<string>();
            //string dir = Directory.GetCurrentDirectory() + "\\wwwroot\\StationInfo\\" + stcd + "\\StaImg";
            //if(Directory.Exists(dir))
            //{
            //    string[] strNames = Directory.GetFiles(dir);

            //    foreach (var item in strNames)
            //    {
            //        picname.Add(Path.GetFileName(item));
            //    }
            //}
            var list = service.GetModelBySTCD(stcd).ToList();
            List<string> picname = new List<string>();
            if (list != null && list.Count > 0)
            {
                var imglist = imgservice.GetModelById(list[0].ID).ToList();
                foreach (var item in imglist)
                {
                    picname.Add(item.file_Path);
                }
            }            
            return Content(picname.ToJson());
        }
        #region 测站展示的管理

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

        public IActionResult Add(STATIONINFO_SHOW entity, string ImagesPath)
        {
            var fileList = new List<STATIONINFO_SHOWIMG>();
            if (service.Count(entity.STCD) > 0)
            {
                return Json(new { errorMsg = "error", msg = "添加失败:该站已经测站展示文件，不能重复上传" });
            }
            if (!string.IsNullOrWhiteSpace(ImagesPath))
            {
                fileList = Extention.ToList<STATIONINFO_SHOWIMG>(ImagesPath);
            }
            int result = service.Insert(entity);
            if (result > 0)
            {
                foreach (var item in fileList)
                {
                    item.show_Id = result;
                    item.add_Time = DateTime.Now;
                    imgservice.Insert(item);
                }
                return Json(new { result = "success", msg = "添加成功" });
            }
            else
                return Json(new { errorMsg = "error", msg = "添加失败" });
        }

        public IActionResult Edit(STATIONINFO_SHOW entity, string ImagesPath)
        {
            var fileList = new List<STATIONINFO_SHOWIMG>();
            var model = service.Get(entity.ID);
            if (model.STCD != entity.STCD)
            {
                if (service.Count(entity.STCD) > 0)
                {
                    return Json(new { errorMsg = "error", msg = "修改失败:该站已经上传了文件，不能重复上传" });
                }
            }
            if (!string.IsNullOrWhiteSpace(ImagesPath))
            {
                fileList = Extention.ToList<STATIONINFO_SHOWIMG>(ImagesPath);
            }
            if (entity.Video_URL == null)
                entity.Video_URL = "";
            bool result = service.Update(entity);            
            if (result)
            {
                List<STATIONINFO_SHOWIMG> showImgList = imgservice.GetModelById(entity.ID).ToList();
                foreach (var item in showImgList)
                {
                    imgservice.Delete(item.id);
                }
                foreach (var item in fileList)
                {
                    item.show_Id = entity.ID;
                    item.add_Time = DateTime.Now;
                    imgservice.Insert(item);
                }
                return Json(new { result = "success", msg = "修改成功" });
            }
            else
                return Json(new { errorMsg = "error", msg = "修改失败" });
        }

        public IActionResult GetModelById(int ID)
        {
            List<STATIONINFO_SHOWIMG> data = imgservice.GetModelById(ID).ToList();
            return Content(data.ToJson());
        }
        public IActionResult Delete(int ID)
        {
            if (service.Delete(ID))
                return Json(new { result = "success", msg = "删除成功" });

            return Json(new { errorMsg = "error", msg = "删除失败" });
        }

        #region 上传多张照片        
        [HttpPost]
       
        public IActionResult UploadImg([FromServices]IHostingEnvironment env, IFormFile file)
        {
            // 如果没有上传文件
            if (file == null || string.IsNullOrEmpty(file.FileName) || file.Length == 0)
            {
                return Error("没有选择上传图片！");
            }           
            //获取用户上传文件的文件名
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = System.IO.Path.GetExtension(file.FileName);
            if (fileExtension != ".gif" && fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".bmp" && fileExtension != ".png")//笔者这儿修改了后缀的判断
            {
                return Error("文件格式不正确，请选择正确的图片格式！");
            }
            var newFileName = fileName + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + fileExtension;
            //虚拟路径
            string virtualPath = string.Format("/_fileupload/StationShow/images/{0}", newFileName);
            
            var path = env.WebRootPath + "\\_fileupload\\StationShow\\images\\" + newFileName;

            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }
            var data = new STATIONINFO_SHOWIMG
            {
                file_Name = fileName,
                file_Path = virtualPath,
                file_Ext = fileExtension
            };
            //var data = new { FilePath = virtualPath };
            return Success("上传成功", data);
        }
        #endregion

        #region 上传宣传片视频
        [HttpPost]
        public IActionResult UploadVideo([FromServices]IHostingEnvironment env, IFormFile file)
        {            
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
            string virtualPath = string.Format("/_fileupload/StationShow/video/{0}", newFileName);
            
            if (fileExtension != ".mp4" && fileExtension != ".rmvb" && fileExtension != ".avi" && fileExtension != ".flv")//笔者这儿修改了后缀的判断
            {
                return Error("文件格式不正确，请选择正确的视频格式！");
            }

            var rootpath = env.WebRootPath;
            var path = rootpath + "\\_fileupload\\StationShow\\video\\" + newFileName;

            using (var stream = new FileStream(path, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }


            var data = new { FilePath = virtualPath };
            return Success("上传成功", data);
        }
        #endregion

        [HttpGet]
        public ActionResult DeleteFile([FromServices]IHostingEnvironment env, int showId, string filePath1, string filePath2)
        {
            try
            {
                var fileList = new List<STATIONINFO_SHOWIMG>();
                fileList = imgservice.GetModelById(showId).ToList();               
                if (fileList.Count > 0)
                {
                    foreach (var item in fileList)
                    {
                        imgservice.Delete(item.id);
                        if (System.IO.File.Exists(env.WebRootPath + item.file_Path))
                        {                            
                            System.IO.File.Delete(env.WebRootPath + item.file_Path);
                        }                        
                    }
                }
                if (System.IO.File.Exists(env.WebRootPath + filePath2))
                {
                    System.IO.File.Delete(env.WebRootPath + filePath2);
                }
                return Json(new { result = "success", msg = "文件删除成功" });
            }
            catch
            {
                return Json(new { result = "error", msg = "文件删除失败" });
            }
        }
        #endregion

    }
}