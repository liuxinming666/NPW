/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_LOGINLOGService                                    
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
    public class SYS_LOGINLOGService: ISYS_LOGINLOGService
    {
        private readonly ISYS_LOGINLOGRepository _repository;

        public SYS_LOGINLOGService(ISYS_LOGINLOGRepository repository)
        {
            _repository = repository;
        }
        public Page<SYS_LOGINLOG> GetLoginLogData(int pageIndex, int pageSize, string logtype)
        {
            return _repository.GetLoginLogData(pageIndex, pageSize, logtype);
        }
        public string Insert(SYS_LOGINLOG entity)
        {
            var result = _repository.Insert<string>(entity);
            return "";
        }
    }
}