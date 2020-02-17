/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-06-03 12:07:55                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： EWF.Services                                  
*│　类    名： SYS_ROLEService                                    
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
    public class SYS_ROLEService: ISYS_ROLEService
    {
        private readonly ISYS_ROLERepository _repository;
        private readonly ISYS_USERRepository _userRepository;

        public SYS_ROLEService(ISYS_ROLERepository repository, ISYS_USERRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }


        public Page<SYS_ROLE> GetUserData(int pageIndex, int pageSize, string rname, string addvcd, int type)
        {
            return _repository.GetUserData(pageIndex, pageSize, rname, addvcd, type);
        }

        public IEnumerable<SYS_ROLEMENUMAP> GetRoleMenuCode(string roleCode)
        {
            return _repository.GetRoleMenuCode(roleCode);
        }

        public bool InsertRoleMenu(string roleCode,string menuCode)
        {
            var MenuCode = menuCode.Split(',');
            var list = new List<SYS_ROLEMENUMAP>();

            foreach (var item in MenuCode)
            {
                if (item.IsEmpty())
                    continue;
                list.Add(new SYS_ROLEMENUMAP
                {
                    RoleCode = roleCode,
                    MenuCode = item
                });
            }
            
            _repository.InsertRoleMenu(list);
            return true;
        }


        public SYS_ROLE Get(int ID)=> _repository.Get(ID);
        public bool Insert(SYS_ROLE entity)=> _repository.Insert(entity) > 0 ? true : false;
        public bool Update(SYS_ROLE entity) => _repository.UpdateIgnoreNull(entity);
        public bool Delete(int ID) => _repository.Delete(ID) > 0 ? true : false;
        public bool DeleteList(object whereConditions)=>_repository.DeleteList(whereConditions)> 0 ? true : false;

        public int IsExists(string rolecode, string addvcd, string type)
        {
            //int count = _repository.Count("where rolecode='" + rolecode + "' and addvcd='" + addvcd + "' and type=" + type);
            int count = _repository.Count("where rolecode='" + rolecode + "'");
            return count;
        }
        public IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename)
        {
            return _userRepository.GetAllRole(roleid, addvcd, type, rolename);
        }

    }
}