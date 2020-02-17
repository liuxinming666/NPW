using EWF.IRepository;
using EWF.IServices;
using EWF.Util;
using EWF.Util.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EWF.Services
{
    public class IcejamService : IIcejamService
    {
        private IIcejamRepository repository;
        public IcejamService(IIcejamRepository _epository)
        {
            repository = _epository;
        }
        /// <summary>
        /// 查询单站水情数据-未分页
        /// </summary>
        /// <param name="stcd"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetSingleTempData(string stcd, string startDate, string endDate)
        {
            return repository.GetSingleTempData(stcd, startDate, endDate);
        }
        /// <summary>
        /// 查询多站水情数据-未分页
        /// add by SUN
        /// Date:2019-05-24 11:00
        /// </summary>
        /// <param name="stcds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string GetTempDataByMultiStcds(string stcds, string sqnm, string startDate, string endDate)
        {
            List<dynamic> listData = repository.GetTempDataByMultiStcds(stcds, startDate, endDate).ToList();
            string[] arrStcd;
            //创建结果表结构 BEGIN
            DataTable dt = new DataTable();
            dt.Columns.Add("TM");
            if (!string.IsNullOrEmpty(stcds))
            {
                arrStcd = stcds.Split(',');
                foreach (string strStcd in arrStcd)
                {
                    if (!dt.Columns.Contains(strStcd))
                    {
                        dt.Columns.Add(strStcd.Trim('\''));
                    }

                }
            }
            //创建结果表结构 END

            //按照新表结构重新组织数据
            Dictionary<string, DataRow> dir = new Dictionary<string, DataRow>();
            foreach (dynamic dy in listData)
            {
                string key = dy.TM.ToString();
                if (!dir.Keys.Contains(key))
                {
                    DataRow drNew = dt.NewRow();
                   
                    drNew["TM"] = key;
                    if(sqnm=="最高气温")
                    {
                        if (!string.IsNullOrEmpty(dy.HTMP))
                        {
                            drNew[dy.STCD] = dy.HTMP.ToString();
                        }
                        else
                        {
                            drNew[dy.STCD] = "";
                            
                        }
                    }
                    else if(sqnm == "最低气温")
                    {
                        if (!string.IsNullOrEmpty(dy.LTMP))
                        {
                            
                            drNew[dy.STCD] = dy.LTMP.ToString();
                        }
                        else
                        {
                            drNew[dy.STCD] = "";
                        }
                    }
                    else if(sqnm == "平均气温")
                    {
                        
                        //if (!string.IsNullOrEmpty(dy.AVTP))
                        //{

                            drNew[dy.STCD] = dy.AVTP.ToString();
                        //}
                        //else
                        //{
                        //    drNew[dy.STCD] = "";
                        //}
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(dy.AVWT))
                        //{

                            drNew[dy.STCD] = dy.AVWT.ToString();
                        //}
                        //else
                        //{
                        //    drNew[dy.STCD] = "";
                        //}
                    }
                    dt.Rows.Add(drNew);
                    dir.Add(key, drNew);
                }
                else
                {
                    if (sqnm == "最高气温")
                    {
                        if (!string.IsNullOrEmpty(dy.HTMP))
                        {
                            dir[key][dy.STCD] = dy.HTMP.ToString();
                        }
                        else
                        {
                            dir[key][dy.STCD] = "";

                        }
                    }
                    else if (sqnm == "最低气温")
                    {
                        if (!string.IsNullOrEmpty(dy.LTMP))
                        {

                            dir[key][dy.STCD] = dy.LTMP.ToString();
                        }
                        else
                        {
                            dir[key][dy.STCD] = "";
                        }
                    }
                    else if (sqnm == "平均气温")
                    {

                        //if (!string.IsNullOrEmpty(dy.AVTP))
                        //{

                        dir[key][dy.STCD] = dy.AVTP.ToString();
                        //}
                        //else
                        //{
                        //    drNew[dy.STCD] = "";
                        //}
                    }
                    else
                    {
                        //if (!string.IsNullOrEmpty(dy.AVWT))
                        //{

                        dir[key][dy.STCD] = dy.AVWT.ToString();
                        //}
                        //else
                        //{
                        //    drNew[dy.STCD] = "";
                        //}
                    }
                }
            }
            return Json.ToJson(dt);
        }

        public string GetIceDate(string stcd, string startDate, string endDate, string addvcd, string type)
        {
            DataTable tables = repository.GetIceData(stcd, startDate, endDate, addvcd, type);
            return tables.ToJson();
        }
        public List<dynamic> GetSearchKeywords(string keyword)
        {
            var list = repository.GetSearchKeywords(keyword);
            return list.ToList();
        }

        /// <summary>
        /// 获取凌情动态
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable GetIceJamDynamic(string sDate)
        {
            DataTable tables = repository.GetIceJamDynamic(sDate);
            return tables;
        }

        /// <summary>
        /// 获取凌情信息中关注的站点信息
        /// </summary>
        /// <param name="Sdate"></param>
        /// <returns></returns>
        public DataTable GetIceJam_River(string Sdate)
        {
            DataTable dt = repository.GetIceJam_River(Sdate);
            return dt;
        }

        public DataTable GetIceJam_LQDT(string Sdate)
        {
            DataTable dt = repository.GetIceJam_LQDT(Sdate);
            return dt;
        }
    }
}
