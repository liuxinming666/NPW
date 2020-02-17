using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using EWF.Util;

namespace EWF.Services
{
    public class TmpavService : ITmpavService
    {
        private ITmpavRepository repository;
        public TmpavService(ITmpavRepository _epository)
        {
            repository = _epository;
        }
        /// <summary>
        /// 查询水温气温数据-未分页
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetTempComparativeData(string stcd, string startDate, string endDate)
        {
            return repository.GetTempComparativeData(stcd, startDate, endDate);
        }
        #region 历史同期蒸发量旬月对比分析
        /// <summary>历史同期蒸发量对比分析</summary>
        /// <param name="STCD">站名</param>
        /// <param name="type">类型4表示旬，5表示月</param>
        /// <param name="sdate">开始日期</param>
        /// <param name="edate">结束日期</param>
        /// <param name="year">对比年份</param>
        /// <returns>时段内旬月均值</returns>
        public IEnumerable<dynamic> GetData_MMonthEV(string STCD, string type, string sdate, string edate, string year, ref string datasrc)
        {
            var stime = Convert.ToDateTime(sdate).ToString("yyyy-MM-dd 08:00");
            var etime = Convert.ToDateTime(edate).ToString("yyyy-MM-dd 08:00");
            var sdate_history = year + Convert.ToDateTime(sdate).ToString("-MM-dd 08:00");
            var edate_history = year + Convert.ToDateTime(edate).ToString("-MM-dd 08:00");
            var result = repository.GetData_MMonthEV(STCD, type, stime, etime, sdate_history, edate_history);
            return ConvertTableMonth_Comparative(result.real, result.history,type);
        }
        //历史同期对比表格转置--月
        private IEnumerable<dynamic> ConvertTableMonth_Comparative(IEnumerable<ST_PSTATEntity> list_real, IEnumerable<ST_PSTATEntity> list_history,string type)
        {
            var list_result = new List<dynamic>();
            foreach (var item in list_real)
            {
                var IDTM_Comparative = "";
                double ACCP_Comparative = 0;
                var row_Comparative = list_history.Where(x => x.STCD == item.STCD)
                    .Where(x => x.IDTM.Month == item.IDTM.Month && x.IDTM.Day == item.IDTM.Day);

                if (row_Comparative.Count() > 0)
                {

                    ACCP_Comparative = row_Comparative.FirstOrDefault().ACCP.ToDouble();
                   // string t = row_Comparative.FirstOrDefault().IDTM.ToString();
                    IDTM_Comparative = row_Comparative.FirstOrDefault().IDTM.ToString("yyyy-MM-dd");
                }
               
                dynamic row = new
                {
                    //STNM = item.STNM,
                    STCD = item.STCD,
                    //IDTM = item.IDTM.AddMonths(-1).ToString("yyyy-MM-dd"),
                    IDTM = item.IDTM.ToString("MM-dd"),
                    ACCP = item.ACCP,
                    IDTM_Comparative = IDTM_Comparative,
                    ACCP_Comparative = ACCP_Comparative,
                };
                list_result.Add(row);
            }
            return list_result;
        }
        #endregion
    }
}
