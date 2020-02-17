using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    public interface IST_ADDVCD_POINTService : IDependency
    {
        Page<ST_ADDVCD_POINT> GetAddvcdPointData(int pageIndex, int pageSize, string Addvcd,string Stnm);

        Page<dynamic> GetAddPointData(int pageIndex, int pageSize, string Addvcd, string Stnm);

        Page<dynamic> GetStcdData(int pageIndex, int pageSize, string Stcd, string Stnm);

        string Insert(ST_ADDVCD_POINT entity);

        string Update(ST_ADDVCD_POINT entity);

        string Delete(string Addvcd, string Stcd);

        ST_ADDVCD_POINT GetAddvcdPointByID(string ID);
    }
}
