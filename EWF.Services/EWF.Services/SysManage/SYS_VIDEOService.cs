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
    public class SYS_VIDEOService : ISYS_VIDEOService
    {
        private readonly ISYS_VIDEORepository repository;

        public SYS_VIDEOService(ISYS_VIDEORepository _repository)
        {
            repository = _repository;
        }
        public Page<SYS_VIDEO> GetVideoData(int pageIndex, int pageSize, string name,int type, string addvcd)
        {
            return repository.GetVideoData(pageIndex, pageSize, name, type, addvcd);
        }
        public bool Insert(SYS_VIDEO entity)
        {
            var result = repository.Insert(entity);
            if (result > 0)
            {
                return true; ;
            }
            return false;
        }
        public bool Update(SYS_VIDEO entity)
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
        public IEnumerable<SYS_VIDEO> GetVideoBySTCD(string STCD)
        {
            var result = repository.GetVideoList(STCD);
            return result;
        }
        public SYS_VIDEO GetVideoByID(int ID)
        {
            var result = repository.GetVideoByID(ID);
            return result;
        }
    }
}