/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-05 12:20:06                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： STATIONINFO_SURVEYService                                    
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
    public class STATIONINFO_SURVEYService: ISTATIONINFO_SURVEYService
    {
        private readonly ISTATIONINFO_SURVEYRepository _repository;

        public STATIONINFO_SURVEYService(ISTATIONINFO_SURVEYRepository repository)
        {
            _repository = repository;
        }


        public Page<STATIONINFO_SURVEY> GetPageData(int pageIndex, int pageSize, string name)
        {
            return _repository.GetPageData(pageIndex, pageSize, name);
        }

        public int Count(string  STCD)
        {
            var result = _repository.Count(new { STCD = STCD });
            return result;
        }

        public STATIONINFO_SURVEY Get(Int32 ID)
        {
            var result = _repository.Get(ID);
            return result;
        }

        public bool Insert(STATIONINFO_SURVEY entity)
        {
            var result = _repository.Insert(entity);
            if (result > 0)
            {
                return true; ;
            }
            return false;
        }
        
        public bool Update(STATIONINFO_SURVEY entity)
        {

            var result = _repository.UpdateIgnoreNull(entity);
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
        public IEnumerable<STATIONINFO_SURVEY> GetModelBySTCD(string STCD)
        {
            var result = _repository.GetModelBySTCD(STCD);
            return result;
        }
    }
}