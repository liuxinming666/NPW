/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_ACCESSTIMEService                                    
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

namespace EWF.Services
{
    public class SYS_ACCESSTIMEService: ISYS_ACCESSTIMEService
    {
        private readonly ISYS_ACCESSTIMERepository _repository;

        public SYS_ACCESSTIMEService(ISYS_ACCESSTIMERepository repository)
        {
            _repository = repository;
        }
        public bool UpdateLastAccessTime(string uid, string authority)
        {
            return _repository.UpdateLastAccessTime(uid, authority);
        }
        public string Insert(SYS_ACCESSTIME entity)
        {
            return _repository.Insert<string>(entity);
        }
    }
}