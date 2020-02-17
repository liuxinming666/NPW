using EWF.Entity;
using EWF.IRepository;
using EWF.IRepository.SysManage;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Services
{
    public  class ST_ADDVCD_DService:IST_ADDVCD_DService
    {

        private readonly IST_ADDVCD_DRepository repository;
        private readonly ISYS_USERRepository userRepository;

        public ST_ADDVCD_DService(IST_ADDVCD_DRepository _repository, ISYS_USERRepository _userRepository)
        {
            repository = _repository;
            userRepository = _userRepository;
        }

        /// <summary>
        /// 获取单位分页信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="UCode"></param>
        /// <param name="UnitName"></param>
        /// <returns></returns>
        public Page<ST_ADDVCD_D> GetAddvcdData(int pageIndex, int pageSize, string Addvcd, string Addvnm,string Type)
        {
            return repository.GetAddvcdData(pageIndex, pageSize, Addvcd, Addvnm,Type);

        }

        public string Insert(ST_ADDVCD_D entity)
        {
            var result = repository.Insert<string>(entity);
            if (!result.IsEmpty())
            {
                return "录入成功";
            }
            return "录入失败";
        }
        public string Update(ST_ADDVCD_D entity)
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
        public ST_ADDVCD_D GetAddvcdByID(string ID)
        {
            var result = repository.Get(ID);
            return result;
        }
        public IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename)
        {
            return userRepository.GetAllRole(roleid, addvcd, type, rolename);
        }
    }
}
