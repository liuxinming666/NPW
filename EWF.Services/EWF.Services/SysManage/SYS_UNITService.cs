/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：lw                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_UNITService                                    
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
    public class SYS_UNITService: ISYS_UNITService
    {
        private readonly ISYS_UNITRepository repository;

        public SYS_UNITService(ISYS_UNITRepository _repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// 获取单位分页信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="UCode"></param>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public Page<SYS_UNIT> GetUnitData(int pageIndex, int pageSize, string UCode,string UnitName)
        {
            return repository.GetUnitData(pageIndex, pageSize, UCode,UnitName);

        }

        public DataTable getUnitList(string UCode, string UnitName)
        {
            DataTable dt = repository.getUnitList(UCode, UnitName);
            dt.Columns.Add("_parentId");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["PARENTID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    dr["_parentId"] = DBNull.Value;
                }
                else
                {
                    dr["_parentId"] = dr["PARENTID"]; //假设PCode列中存储了父标识
                }
            }
            return dt;
        }

        public string Insert(SYS_UNIT entity)
        {
            var result = repository.Insert<string>(entity);
            if (!result.IsEmpty())
            {
                return "录入成功";
            }
            return "录入失败";
        }
        public string Update(SYS_UNIT entity)
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
        public SYS_UNIT GetUnitByID(string ID)
        {
            var result = repository.Get(ID);
            return result;
        }

    }
}