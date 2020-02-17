using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Services
{
    public class RainWarnSetService: IRainWarnSetService
    {
        private readonly IRainWarnSetRepository repository;
        public RainWarnSetService(IRainWarnSetRepository _repository)
        {
            repository = _repository;
        }
        public DataTable GetRainWarnData(int type, string addvcd)
        {
            DataTable dt = repository.GetRainWarnData(type, addvcd);
            DataTable dtRain = new DataTable();
            dtRain.Columns.Add("RTYPE", typeof(string));
            dtRain.Columns.Add("THRESHOLD_3", typeof(double));
            dtRain.Columns.Add("THRESHOLD_2", typeof(double));
            dtRain.Columns.Add("THRESHOLD_1", typeof(double));
            var typeArray = new int[] { 1, 3, 6 };
            //var djArray = new int[] { 3, 2, 1 };
            foreach (var tpItem in typeArray)
            {
                DataRow dr = dtRain.NewRow();
                dr["RTYPE"] = tpItem + "小时";
                DataRow[] drs3 = dt.Select("DURATION=" + tpItem + " and YLJB=3");
                DataRow[] drs2 = dt.Select("DURATION=" + tpItem + " and YLJB=2");
                DataRow[] drs1 = dt.Select("DURATION=" + tpItem + " and YLJB=1");
                dr["THRESHOLD_3"] = drs3.Length > 0 ? drs3[0]["THRESHOLD"] : "25.0";
                dr["THRESHOLD_2"] = drs2.Length > 0 ? drs2[0]["THRESHOLD"] : "50.0";
                dr["THRESHOLD_1"] = drs1.Length > 0 ? drs1[0]["THRESHOLD"] : "100.0";
                dtRain.Rows.Add(dr);
            }
            return dtRain;
        }

        public string UpdateData(string rtype, string threshold_3, string threshold_2, string threshold_1, int type, string addvcd)
        {
            var result = repository.UpdateData(rtype, threshold_3, threshold_2, threshold_1, type, addvcd);
            return result;
        }
    }
}
