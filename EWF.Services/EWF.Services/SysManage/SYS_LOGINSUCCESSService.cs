/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_LOGINSUCCESSService                                    
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
    public class SYS_LOGINSUCCESSService: ISYS_LOGINSUCCESSService
    {
        private readonly ISYS_LOGINSUCCESSRepository _repository;

        public SYS_LOGINSUCCESSService(ISYS_LOGINSUCCESSRepository repository)
        {
            _repository = repository;
        }
        public string Insert(SYS_LOGINSUCCESS entity)
        {
            return _repository.Insert<string>(entity);
        }
        public bool DeleteLoginSuccess(string userip, string username, DateTime tm)
        {
            return _repository.DeleteLoginSuccess(userip, username, tm);
        }
        public DataTable GetLoginSuccessCount(string userip, string username, DateTime tm)
        {
            return _repository.GetLoginSuccessCount(userip, username, tm);
        }
    }
}