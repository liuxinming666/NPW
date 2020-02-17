/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-05 12:20:06                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： STATIONINFO_SHOWIMGService                                    
*└──────────────────────────────────────────────────────────────┘
*/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using EWF.Util;
using EWF.Util.Page;

namespace EWF.Services
{
    public class STATIONINFO_SHOWIMGService : ISTATIONINFO_SHOWIMGService
    {
        private readonly ISTATIONINFO_SHOWIMGRepository _repository;

        public STATIONINFO_SHOWIMGService(ISTATIONINFO_SHOWIMGRepository repository)
        {
            _repository = repository;
        }

        
        public int Insert(STATIONINFO_SHOWIMG entity)
        {
            var result = _repository.Insert(entity);
            return result;
        }

        public bool Delete(Int32 ID)
        {
            var result = _repository.Delete(ID);
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteList(object whereConditions)
        {
            var result = _repository.DeleteList(whereConditions);
            if (result > 0)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<STATIONINFO_SHOWIMG> GetModelById(int ShowId)
        {
            var result = _repository.GetModelById(ShowId);
            return result;
        }
    }
}