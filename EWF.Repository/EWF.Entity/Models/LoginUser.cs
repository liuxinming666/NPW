using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Entity
{
    public class LoginUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string USERID { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string UACCOUNT { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string REALNAME { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UPWD { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        public string UNITID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DEPARTMENTID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public string ADDVCD { get; set; }
    }
}
