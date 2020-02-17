using EWF.Data.Repository;
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IRepository
{
    public interface IRainWarnSetRepository : IRepositoryBase<TBL_EVENT_YLMODAL>, IDependency
    {
        DataTable GetRainWarnData(int type, string addvcd);
        string UpdateData(string rtype, string threshold_3, string threshold_2, string threshold_1, int type, string addvcd);
    }
}
