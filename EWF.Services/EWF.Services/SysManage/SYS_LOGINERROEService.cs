/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_LOGINERROEService                                    
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
    public class SYS_LOGINERROEService: ISYS_LOGINERROEService
    {
        private readonly ISYS_LOGINERROERepository _repository;

        public SYS_LOGINERROEService(ISYS_LOGINERROERepository repository)
        {
            _repository = repository;
        }

        public DataTable GetLoginErrorCount(string userip, string username, DateTime tm)
        {
            return _repository.GetLoginErrorCount(userip, username, tm);
        }
        public string Insert(SYS_LOGINERROE entity)
        {
            var result = _repository.Insert<string>(entity);
            return "";
        }
        public DataTable GetLoginByHasSQLCount(string userip, string username, DateTime tm, string ishassql)
        {
            return _repository.GetLoginByHasSQLCount(userip, username, tm, ishassql);
        }
        public bool DeleteLoginError(string userip, string username)
        {
            return _repository.DeleteLoginError(userip, username);
        }
    }
}