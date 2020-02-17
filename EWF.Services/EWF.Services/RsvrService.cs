/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：lw
 * 日  期：2019/5/24 16:48:11
 * 描  述：RsvrService
 * *********************************************/
using EWF.IRepository;
using EWF.IServices;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EWF.Services
{
    public class RsvrService : IRsvrService
    {
        private IRsvrRepository repository;
        DataOption dataOption;
        public RsvrService(IRsvrRepository _epository, IOptionsSnapshot<DataOption> _option)
        {
            repository = _epository;
            dataOption = _option.Get("DataOption");
        }

        public Page<dynamic> GetReadData(int pageIndex, int pageSize, string STNM)
        {

            var list = repository.GetReadData(pageIndex, pageSize, STNM);
            return list;
        }

        /// <summary>
        /// 获取一天内最新水情信息
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetLatestRsvrData(string startDate, string endDate, string addvcd, string type)
        {
            string eDate = DateTime.Now.ToString();
            string sDate = DateTime.Now.AddDays(-dataOption.Interval).ToString();
            var list = repository.GetLatestRsvr(startDate, endDate, addvcd, type);
            return list.ToList<dynamic>();
        }

        public Page<dynamic> GetRsvrData(int pageIndex, int pageSize, string stcds, string startDate, string endDate, string addvcd, string type)
        {
            var list = repository.GetRsvrData(pageIndex, pageSize, stcds, startDate, endDate, addvcd, type);
            return list;
        }

        /// <summary>
        /// 获取水库水情均值
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<dynamic> GetRsvrav_avg(int sttdrcd, string stcd, string startDate, string endDate)
        {
            var list = repository.GetRsvrav_avg(sttdrcd, stcd,startDate,endDate);
            return list.ToList<dynamic>();
        }

        /// <summary>
        /// 获取水库水位过程线
        /// add by lw
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<dynamic> GetRsvr_Line(string stcd, string startDate, string endDate)
        {
            var list = repository.GetRsvr_Line(stcd,startDate,endDate);
            return list.ToList<dynamic>();
        }
        /// <summary>
        /// 首页查询8点水库水情过程线信息
        /// add by qlj
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRsvrLineEight(string stcd, string startDate, string endDate)
        {
            startDate = Convert.ToDateTime(endDate).AddDays(-dataOption.SysRsvr).ToString();
            return repository.GetRsvrLineEight(stcd, startDate, endDate);
        }
    }
}
