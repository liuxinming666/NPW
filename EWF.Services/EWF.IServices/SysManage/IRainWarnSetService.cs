using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.IServices
{
    public interface IRainWarnSetService : IDependency
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        DataTable GetRainWarnData(int type, string addvcd);
        string UpdateData(string rtype, string threshold_3, string threshold_2, string threshold_1, int type, string addvcd);
    }
}
