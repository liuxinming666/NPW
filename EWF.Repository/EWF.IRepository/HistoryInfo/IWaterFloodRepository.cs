/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijuan
 * 日  期：2019/5/30 16:36:04
 * 描  述：WaterFloodRepository
 * *********************************************/
using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IRepository
{
    public interface IWaterFloodRepository : IDependency
    {
        /// <summary>历史洪水过程分析单站对比</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>历史洪水单站对比数据</returns>
        IEnumerable<ST_FSPS_REntity> GetWaterFloodData(string STCD, string sdate, string edate);
        /// <summary>历史洪水过程分析多站对比</summary>
        /// <param name="STCD">站名</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <returns>历史洪水多站对比数据</returns>
        dynamic GetWaterFloodMutiData(string STCD, string sdate, string edate);
    }
}
