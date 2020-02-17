/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_DEPARTMENTService                                    
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
    public class SYS_DEPARTMENTService: ISYS_DEPARTMENTService
    {
        private readonly ISYS_DEPARTMENTRepository repository;

        public SYS_DEPARTMENTService(ISYS_DEPARTMENTRepository _repository)
        {
            repository = _repository;
        }

        public Page<SYS_DEPARTMENT> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName)
        {
            return repository.GetDepartmentData(pageIndex, pageSize, DCode, DName);

        }

        /// <summary>
        /// 获取部门分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="DCode"></param>
        /// <param name="DName"></param>
        /// <param name="UName"></param>
        /// <returns></returns>
        public Page<dynamic> GetDepartmentData(int pageIndex, int pageSize, string DCode, string DName, string UName, string UnitId)
        {
            return repository.GetDepartmentData(pageIndex, pageSize, DCode, DName,UName, UnitId);

        }

        public string Insert(SYS_DEPARTMENT entity)
        {
            var result = repository.Insert<string>(entity);
            if (!result.IsEmpty())
            {
                return "录入成功";
            }
            return "录入失败";
        }
        public string Update(SYS_DEPARTMENT entity)
        {
            var result = repository.Update(entity);
            if (result)
            {
                return "编辑成功";
            }
            return "编辑失败";
        }
        public string Delete(string ID)
        {
            var result = repository.Delete(ID);
            if (result > 0)
            {
                return "删除成功";
            }
            return "删除失败";
        }
        public SYS_DEPARTMENT GetUnitByID(string ID)
        {
            var result = repository.Get(ID);
            return result;
        }
        
    }
}