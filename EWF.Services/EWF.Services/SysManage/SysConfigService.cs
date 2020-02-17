using EWF.IRepository;
using EWF.Entity;
using EWF.IRepository.SysManage;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Util;
using EWF.Util.Page;
using System.Linq;

namespace EWF.Services
{
    public class SysConfigService: ISysConfigService
    {
        private readonly ISysConfigRepository repository;
        private readonly IVideoRepository videoRepository;

        public SysConfigService(ISysConfigRepository _repository,IVideoRepository _videoRepository)
        {
            repository = _repository;
            videoRepository = _videoRepository;
        }
        public SYS_Config GetSysByID(int ID)
        {
            var result = repository.Get(ID);
            return result;
        }
        public string Update(SYS_Config entity)
        {
            var result = repository.Update(entity);
            if (result)
            {
                return "修改成功";
            }
            return "修改失败";
        }
        public string Insert(SYS_Config entity)
        {
            var result = repository.Insert(entity);
            if (result>0)
                return "添加成功";
            else
                return "添加失败";
        }
        public IEnumerable<dynamic> GetSysConfigData(int sysId)
        {
            return repository.GetSysConfigData(sysId);
        }
        public IEnumerable<SYS_Config> GetSysConfigByAddvcd(string addvcd)
        {
            return repository.GetSysConfigByAddvcd(addvcd);
        }
        /// <summary>
        /// 根据系统配置的测站编码获取测站名称
        /// </summary>
        /// <param name="addvcd"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetStnmBySysconfig(string addvcd)
        {
            return repository.GetStnmBySysconfig(addvcd);
        }
        /// <summary>
        /// 根据测站名称获取测站编码
        /// </summary>
        /// <param name="stnm"></param>
        /// <returns></returns>
        public DataTable GetStationListByStnm(string stnm)
        {
            return repository.GetStationListByStnm(stnm);
        }

        public string getVideoManageList(string addvcd,int type)
        {
            //先从系统配置中读取摄像头信息
            string video = GetSysConfigByAddvcd(addvcd).Single<SYS_Config>().VIDEONAME;

            string videoArray = "[";
            DataTable dtVideo = videoRepository.GetVideoList("", type, addvcd);
            Dictionary<string, string> typedictionaryItem = new Dictionary<string, string>();
            string stcd = "", stnm = "";
            if (dtVideo.Rows.Count > 0)
            {
                for (int i = 0; i < dtVideo.Rows.Count; i++)
                {
                    stcd = dtVideo.Rows[i]["STCD"].ToString();
                    stnm= dtVideo.Rows[i]["Name"].ToString();
                    typedictionaryItem.Add(stcd, stnm);                    
                }
            }
            var treeList = new List<TreeViewModel>();
            foreach (var item in typedictionaryItem)
            {
                DataTable dtVideoManage = videoRepository.GetVideoManageList(item.Key);
                videoArray += "{\"id\":\"" + item.Key.ToString() + "\",\"text\":\"" + item.Value + "\"";
                if (dtVideoManage.Rows.Count > 0)
                {
                    videoArray += ",\"children\":[";
                    for (int j = 0; j < dtVideoManage.Rows.Count; j++)
                    {
                        //从系统配置表中读取是否存在此摄像头,摄像头保存的格式：测站编码|摄像头名字
                        var videoname = dtVideoManage.Rows[j]["STCD"].ToString() + "|" + dtVideoManage.Rows[j]["SNAME"].ToString();
                        if (!string.IsNullOrEmpty(video))
                        {
                            if (video.IndexOf(videoname) > -1)
                            {
                                videoArray += "{\"id\":\"" + videoname + "\",\"text\":\"" + dtVideoManage.Rows[j]["SNAME"].ToString() + "\",\"checked\":true},";
                            }
                            else
                            {
                                videoArray += "{\"id\":\"" + videoname + "\",\"text\":\"" + dtVideoManage.Rows[j]["SNAME"].ToString() + "\",\"checked\":false},";
                            }
                        }
                        else
                            videoArray += "{\"id\":\"" + videoname + "\",\"text\":\"" + dtVideoManage.Rows[j]["SNAME"].ToString() + "\",\"checked\":false},";
                    }
                    videoArray = videoArray.Substring(0, videoArray.Length - 1);
                    videoArray += "]";
                }
                videoArray += "},";
            }
            videoArray= videoArray.Substring(0, videoArray.Length - 1);
            videoArray += "]";
            return videoArray;
        }
    }
}
