using Dapper;
using EWF.Data.Repository;
using EWF.Entity;
using EWF.IRepository;
using EWF.Util.Options;
using EWF.Util.Page;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Repository
{
    public class RainWarnSetRepository : DefaultRepository<TBL_EVENT_YLMODAL>, IRainWarnSetRepository
    {
        public RainWarnSetRepository(IOptionsSnapshot<DbOption> options):base(options)
        {
        }

        public DataTable GetRainWarnData(int type, string addvcd)
        {
            string sql = "select * from TBL_EVENT_YLMODAL where type=" + type + " and addvcd='" + addvcd + "' order by duration ,yljb desc";
            return database.FindTable(sql);
        }

        public string UpdateData(string duration, string threshold_3, string threshold_2, string threshold_1, int type, string addvcd)
        {
            TBL_EVENT_YLMODAL model = new TBL_EVENT_YLMODAL();
            int result = 0;
            var sql = "";           
            var sqlParams = new DynamicParameters();
            var djArray = new int[] { 3, 2, 1 };
            foreach (var item in djArray)
            {
                model.YLJB = item;
                model.JBMC = item.ToString().Replace("3", "暴雨").Replace("2", "大暴雨").Replace("1", "特大暴雨");
                var thresholdi = "threshold_" + item;
                var threshold = thresholdi.ToString().Replace("threshold_3", threshold_3).Replace("threshold_2", threshold_2).Replace("threshold_1", threshold_1);
                model.THRESHOLD = Convert.ToDecimal(threshold);
                model.LEGEND = "/newStyle/warn/storm" + item + ".gif";
                model.YLFLAG = "1";
                model.DURATION = Convert.ToDecimal(duration.Replace("1小时", "1.00").Replace("3小时", "3.00").Replace("6小时", "6.00"));
                model.YXQ = 24;
                model.TYPE = type;
                model.ADDVCD = addvcd;
                model.MOTYPE = "1";

                sqlParams.Add("type", type);
                sqlParams.Add("addvcd", addvcd);
                sqlParams.Add("yljb", item);
                sqlParams.Add("duration", model.DURATION);
                var condition = " where yljb=@yljb and duration=@duration and type=@type and addvcd=@addvcd";
                int count = database.Count<TBL_EVENT_YLMODAL>(condition, sqlParams);
                if (count > 0)
                {
                    sql = "update TBL_EVENT_YLMODAL set THRESHOLD=" + model.THRESHOLD + " where yljb=" + item + " and duration=" + model.DURATION + " and type=" + type + " and addvcd='" + addvcd + "'";
                    result = database.ExecuteBySql(sql);
                }
                else
                {
                    result = database.Insert(model);
                }
            }        
            
            if (result > 0)
                return "修改成功";
            else
                return "修改失败";
        }
    }
}
