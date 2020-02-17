/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-05-23 16:43:42                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_USERService                                    
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
    public class SYS_USERService: ISYS_USERService
    {
        private readonly ISYS_USERRepository repository;
        private readonly ISYS_UNITRepository unitRepository;
        private readonly ISYS_DEPARTMENTRepository departmentRepository;

        public SYS_USERService(ISYS_USERRepository _repository, ISYS_UNITRepository _unitRepository, ISYS_DEPARTMENTRepository _departmentRepository)
        {
            repository = _repository;
            unitRepository = _unitRepository;
            departmentRepository = _departmentRepository;
        }
        public IEnumerable<dynamic> GetUserList(string username, string addvcd, int type)
        {
            return repository.GetUserList(username, addvcd, type);
        }
        public Page<SYS_USER> GetUserData(int pageIndex, int pageSize, string username, string addvcd, int type)
        {
            return repository.GetUserData(pageIndex, pageSize, username, addvcd, type);
        }
        public string Insert(SYS_USER entity)
        {
            var result = repository.Insert<string>(entity);
            if (!result.IsEmpty())
            {
                return "添加用户成功";
            }
            return "添加用户失败";
        }
        public string Update(SYS_USER entity)
        {
            var result = repository.Update(entity);
            if (result)
            {
                return "修改用户成功";
            }
            return "修改用户失败";
        }
        public string Delete(string ID)
        {
            var result = repository.Delete(ID);
            if (result > 0)
            {
                return "删除用户成功";
            }
            return "删除用户失败";
        }
        public SYS_USER GetUserByID(string ID)
        {
            var result = repository.Get(ID);
            return result;
        }
        public IEnumerable<SYS_OBJECTUSERRELATION> GetRoleByUser(string userid)
        {
            return repository.GetRoleByUser(userid);
        }
        public IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename)
        {
            return repository.GetAllRole(roleid, addvcd, type, rolename);
        }
        public IEnumerable<SYS_ROLE> GetRoleByCategory(string category)
        {
            return repository.GetRoleByCategory(category);
        }
        public string SaveUserRole(string[] permissionIds, string userid)
        {
            return repository.SaveUserRole(permissionIds, userid);
        }
        public DataTable GetLoginCheck(string userid, string pwd)
        {
            return repository.GetLoginCheck(userid, pwd);
        }
        public string UpdateLoginUser(SYS_USER user)
        {
            //string strDate = user.FIRSTVISIT.ToDate().ToString("yyyy-MM-dd");
            //DateTime dtDate;
            //if (!DateTime.TryParse(strDate, out dtDate) || dtDate.ToDate().ToString("yyyy-MM-dd") == "0001-01-01")
            //{
            //    user.FIRSTVISIT = DateTime.Now;
            //}
            //user.LOGONCOUNT = user.LOGONCOUNT + 1;
            var result = repository.Update(user);
            return "";
        }
        //获取单位信息
        public DataTable GetUnitData()
        {
            return unitRepository.getUnitList("", "");
        }
        //获取部门信息
        public DataTable GetDepartmenttList(string unitid)
        {
            return departmentRepository.GetDepartmenttList(unitid);
        }
        public DataTable GetCityData(int type)
        {
            return repository.GetCityData(type);
        }
        public DataTable GetXianData(string cityid, int type)
        {
            return repository.GetXianData(cityid, type);
        }
        //根据单位id获取单位信息
        public SYS_UNIT GetUnitDataById(string unitid)
        {
            var list = unitRepository.Get(unitid);
            return list;
        }
        public DataTable GetAddvcdData(string addvcd, int type)
        {
            return repository.GetAddvcdData(addvcd, type);
        }
    }
}