using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IST_ADDVCD_DService : IDependency
    {
        Page<ST_ADDVCD_D> GetAddvcdData(int pageIndex, int pageSize, string Addvcd, string Addvnm,string Type);

        string Insert(ST_ADDVCD_D entity);

        string Update(ST_ADDVCD_D entity);

        string Delete(string ID);

        ST_ADDVCD_D GetAddvcdByID(string ID);
        IEnumerable<SYS_ROLE> GetAllRole(string roleid, string addvcd, string type, string rolename);
    }
}
