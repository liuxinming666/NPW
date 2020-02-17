/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_SYSLOGDETAILService                                    
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
    public class SYS_SYSLOGDETAILService: ISYS_SYSLOGDETAILService
    {
        private readonly ISYS_SYSLOGDETAILRepository _repository;

        public SYS_SYSLOGDETAILService(ISYS_SYSLOGDETAILRepository repository)
        {
            _repository = repository;
        }
    }
}