using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository
{
    public  interface IST_ADDVCD_POINTRepository:IRepositoryBase<ST_ADDVCD_POINT>, IDependency
    {
        Page<ST_ADDVCD_POINT> GetAddvcdPointData(int pageIndex, int pageSize, string Addvcd, string Stnm);


        Page<dynamic> GetAddPointData(int pageIndex, int pageSize, string Addvcd, string Stnm);

        Page<dynamic> GetStcdData(int pageIndex, int pageSize, string Stcd, string Stnm);

        int DeletePoint(string Addvcd, string Stcd);
    }
}

