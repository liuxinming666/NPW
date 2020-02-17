/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijuan
 * 日  期：2019/5/30 16:36:04
 * 描  述：WaterFloodRepository
 * *********************************************/
using EWF.Entity;
using EWF.IRepository;
using EWF.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Services
{
    public class WaterFloodService : IWaterFloodService
    {
        private IWaterFloodRepository repository;
        public WaterFloodService(IWaterFloodRepository _epository)
        {
            repository = _epository;
        }
        //单站多要素对比
        public IEnumerable<dynamic> GetWaterFloodData(string STCD, string sdate, string edate)
        {
            return repository.GetWaterFloodData(STCD, sdate, edate);
        }
        //多站单要素对比
        public IEnumerable<dynamic> GetWaterFloodMutiData(string STCD, string sdate, string edate, string ystype)
        {
            var result = repository.GetWaterFloodMutiData(STCD, sdate, edate);
            return ConvertTableMutiStcd(result.z, result.s, ystype);
        }
        //多站单要素对比表格转置
        private IEnumerable<dynamic> ConvertTableMutiStcd(IEnumerable<dynamic> list, IEnumerable<dynamic> lists, string ystype)
        {
            var list_result = new List<dynamic>();
            if (ystype == "水位")
            {
                foreach (var item in list)
                {

                    {
                        dynamic row = new
                        {
                            STNM = item.STNM,
                            STCD = item.STCD,
                            TM = item.TM,
                            Z = item.Z,
                        };
                        list_result.Add(row);
                    }
                }
            }
            else if (ystype == "流量")
            {
                foreach (var item in list)
                {
                    dynamic row = new
                    {
                        STNM = item.STNM,
                        STCD = item.STCD,
                        TM = item.TM,
                        Z = item.Q,
                    };
                    list_result.Add(row);
                }

            }
            else
            {
                foreach (var item in lists)
                {
                    dynamic row = new
                    {
                        STNM = item.STNM,
                        STCD = item.STCD,
                        TM = item.TM,
                        Z = item.S,
                    };
                    list_result.Add(row);
                }
            }
            return list_result;
        }
    }
}
