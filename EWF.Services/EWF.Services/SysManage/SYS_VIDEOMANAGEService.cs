/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_VIDEOService                                    
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.IRepository.SysManage;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.Services
{
    public class SYS_VIDEOMANAGEService : ISYS_VIDEOMANAGEService
    {
        private readonly ISYS_VIDEOMANAGERepository repository;

        public SYS_VIDEOMANAGEService(ISYS_VIDEOMANAGERepository _repository)
        {
            repository = _repository;
        }
        public Page<SYS_VIDEOMANAGE> GetCameraData(int pageIndex, int pageSize, string name, int type, string addvcd)
        {
            return repository.GetCameraData(pageIndex, pageSize, name,type,addvcd);
        }
        public int Insert(SYS_VIDEOMANAGE entity)
        {
            var result = repository.Insert(entity);
            return result;
        }
        public bool Update(SYS_VIDEOMANAGE entity)
        {
            var result = repository.UpdateIgnoreNull(entity);
            return result;
        }
        public bool Delete(int ID)
        {
            var result = repository.Delete(ID);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraNameList(string SNAME)
        {
            var result = repository.GetCameraNameList(SNAME);
            return result;
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraList(string STCD, string SNAME, string SIP, string SPORT)
        {
            var result = repository.GetCameraList(STCD,SNAME,SIP,SPORT);
            return result;
        }
        public IEnumerable<SYS_VIDEOMANAGE> GetCameraListBySTCD(string STCD)
        {
            var result = repository.GetCameraListBySTCD(STCD);
            return result;
        }
        public SYS_VIDEOMANAGE GetCameraByID(int ID)
        {
            var result = repository.GetCameraByID(ID);
            return result;
        }
    }
}