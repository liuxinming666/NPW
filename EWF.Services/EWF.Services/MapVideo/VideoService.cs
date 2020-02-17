/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 16:48:11
 * 描  述：RiverService
 * *********************************************/
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWF.Services
{
    public class VideoService: IVideoService
    {
        private IVideoRepository repository;
        
        public VideoService(IVideoRepository _epository)
        {
            repository = _epository;
        }
        public string GetVideos(string STCD, int type, string addvcd)
        {
            DataTable dt = GetVideoList(STCD, type, addvcd);
            return Json.ToJson(dt);
        }
        public DataTable GetVideoList(string STCD, int type, string addvcd)
        {
            DataTable dt = repository.GetVideoList(STCD, type, addvcd);
            return dt;
        }
        public DataTable GetVideoManageList(string STCD)
        {
            DataTable dt = repository.GetVideoManageList(STCD);
            return dt;
        }

    }
}
