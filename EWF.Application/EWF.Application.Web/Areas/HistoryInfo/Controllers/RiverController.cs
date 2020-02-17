/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianPing
 * 日  期：2019/5/29 19:48:11
 * 描  述：水情
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EWF.Application.Web.Controllers;
using EWF.IServices;
using EWF.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWF.Application.Web.Areas.HistoryInfo.Controllers
{
    [Authorize]
    public class RiverController : EWFBaseController
    {
        private IRiverService service;

        public RiverController(IRiverService _service)
        {
            service = _service;
        }

        //URL:/HistoryInfo/River/GetMultiZQSData

        #region 多站历史水情对比
        /// <summary>
        /// By JinJianping
        /// </summary>
        /// <returns></returns>
        public IActionResult MultiRiver()
        {
            return View();
        }
        public IActionResult GetHistoryZContraData(string stcds, string startDate, string endDate,int year, int type)
        {
            #region 参数检查
            if ( stcds.IsEmpty())
            {
                return Error("测站不能为空！");
            }
            if (string.IsNullOrWhiteSpace(startDate))
            {
                return Error("开始日期不能为空！");
            }
            if (string.IsNullOrWhiteSpace(endDate))
            {
                return Error("截止日期不能为空！");
            }
            if (string.IsNullOrWhiteSpace(year.ToString()))
            {
                return Error("对比年份不能为空！");
            }
            if (string.IsNullOrWhiteSpace(type.ToString()))
            {
                return Error("对比要素不能为空！");
            }
            string[] stcdlist = stcds.Split(",");
            if (stcdlist.Length > 3)
            {
                return Error("查询站点不能超过3个");
            }
            #endregion

            var list = service.GetHistoryRiverByMultiStcds(stcds, startDate, endDate, year, type);

            var data = new
            {
                total = list.Count(),
                rows = list
            };

            return Content(data.ToJson());
        }
        #endregion

        #region 单站水情对比
        public IActionResult SingleRiver()
        {
            return View();
        }

        public IActionResult GetDataForSingleRiverCompare(string stcd, string sdate, string edate, string compareYear)
        {
            var list= service.GetDataForSingleRiverCompare(stcd,sdate,edate,compareYear);

            var data = new
            {
                total = list.Count,
                rows = list
            };
            return Content(data.ToJson()); 
        }
        #endregion

        #region 水流沙过程对照
        /// <summary>
        /// 多站多要素对比
        /// </summary>
        /// <returns></returns>
        public IActionResult MultiZQS()
        {
            return View();
        }

        public IActionResult GetMultiZQSData(string stcds, string stime, string etime)
        {
            #region 参数检查
            if (stime.IsEmpty())
            {
                return Error("开始日期不能为空！");
            }
            if (etime.IsEmpty())
            {
                return Error("截止日期不能为空！");
            }
            #endregion


            var data = service.GetMutliStationZQS(stcds, stime, etime);

            var result = new
            {
                zqlist = data.zqlist,
                slist = data.slist,
                rows = data.countlist,
                total = data.countlist.Count
            };

            return Content(result.ToJson());
        }
        #endregion
    }
}