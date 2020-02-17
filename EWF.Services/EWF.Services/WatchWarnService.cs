/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijua
 * 日  期：2019/5/27 12:36:04
 * 描  述：WatchWarnRepository
 * *********************************************/
using EWF.IRepository;
using EWF.IServices;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EWF.Services
{
    public class WatchWarnService: IWatchWarnService
    {
        private IWatchWarnRepository repository;
        DataOption dataOption;
        public WatchWarnService(IWatchWarnRepository _watchRepository, IOptionsSnapshot<DataOption> _option)
        {
            repository = _watchRepository;
            dataOption = _option.Get("DataOption");
        }
        /// <summary>
        /// 获取预警列表统计数据
        /// </summary>
        /// <param name="sdate">开始时间</param>
        /// <param name="type">类型：1行政区划2流域分区</param>
        /// <param name="addvcd">行政区划</param>
        /// <returns></returns>
        public string GetWatchWarn(string sdate, int type, string addvcd)
        {
            string strdate = null, stredate = null;
            //CompareDateTime(sdate, ref strdate, ref stredate);
            stredate = DateTime.Now.ToString();
            strdate = DateTime.Now.AddDays(-1).ToString();
            DataSet ds = ManageOneData(repository.GetWarnWarnData(strdate, stredate, type, addvcd));
            DataTable dt = ds.Tables[0];
            DataTable dtDetail = ds.Tables[1];
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strStation = new StringBuilder();
            strHtml.AppendLine("<table border=\"0\" align=\"center\"  width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
            string sn = "", tn = "", typename = "", thname = "", strList = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                strStation.Append("var objlists=[");
                foreach (DataRow dr in dtDetail.Rows)
                {
                    strStation.Append("{stcd:'" + dr["stcd"].ToString() + "',stnm:'" + dr["stnm"].ToString() + "',sttp:'" + dr["sttp"].ToString() + "',lgtd:" + dr["lgtd"].ToString() + ",lttd:" + dr["lttd"].ToString() + ",legend:'" + dr["legend"].ToString() + "',qjfz:" + (dr["qjfz"].ToString() == "" ? "0" : dr["qjfz"].ToString()) + ",qj:'" + dr["rid"].ToString() + "'");
                    if (dr["rid"].ToString() != "")
                    {
                        if (dr["sjdj"].ToString() == "1")
                            strStation.Append(",qjcolor:'#FF0000'},");
                        else if (dr["sjdj"].ToString() == "2")
                            strStation.Append(",qjcolor:'#F90BFF'},");
                        else
                            strStation.Append(",qjcolor:'#0000FF'},");
                    }
                    else
                        strStation.Append(",qjcolor:''},");
                }
                strList = strStation.ToString();
                strList = strList.Substring(0, strList.Length - 1) + "];";
                strStation.Remove(0, strStation.Length);
                int i = 0, j = 0, k = 0, index = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (index == 0)
                    {
                        sn = dr["sumname"].ToString();
                        tn = dr["thname"].ToString();
                    }
                    if (dr["sjdj"].ToString() == "1")
                    {
                        if (dr["sttp"].ToString() == "PP")
                        {
                            typename = "特大暴雨";
                            thname = dr["thname"].ToString().Replace("降雨超过", ">=").Replace("mm", "");
                            if (i == 0)
                            {
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/red.gif\" /></td><td><table id='onelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");    
                        }
                        else if (dr["sumname"].ToString().IndexOf("洪峰") != -1)
                        {
                            typename = "特大洪峰";
                            if (i == 0)
                            {
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/red.gif\" /></td><td><table id='onelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>

                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>

                        }
                        else
                        {
                            typename = dr["sumname"].ToString().Replace("含沙量", "河道水情");
                            if (i == 0)
                            {
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/red.gif\" /></td><td><table id='onelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" align=\"left\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>

                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|1" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" align=\"left\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>

                        }

                        i++;
                    }
                    else if (dr["sjdj"].ToString() == "2")
                    {

                        if (dr["sttp"].ToString() == "PP")
                        {
                            typename = "大暴雨";
                            thname = dr["thname"].ToString().Replace("降雨超过", ">=").Replace("mm", "");
                            if (j == 0)
                            {
                                if (i > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/yellow.gif\" /></td><td><table id='twolist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");
                        }
                        else if (dr["sumname"].ToString().IndexOf("洪峰") != -1)
                        {
                            typename = "较大洪峰";
                            if (j == 0)
                            {
                                if (i > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/yellow.gif\" /></td><td><table id='twolist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>
                        }
                        else
                        {
                            typename = dr["sumname"].ToString();
                            if (j == 0)
                            {
                                if (i > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/yellow.gif\" /></td><td><table id='twolist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|2" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>
                        }

                        j++;
                    }
                    else if (dr["sjdj"].ToString() == "3")
                    {
                        if (dr["sttp"].ToString() == "PP")
                        {
                            typename = "暴雨";
                            thname = dr["thname"].ToString().Replace("降雨超过", ">=").Replace("mm", "");
                            if (k == 0)
                            {
                                if (i > 0 || j > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/blue.gif\" /></td><td><table id='threelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td nowrap=nowrap valign=\"top\" style=\"width:50px;\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "'," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><td nowrap=nowrap valign=\"top\" style=\"width:50px;\" align=\"left\">" + typename + "</td><td nowrap=nowrap style=\"width:25px;\" valign=\"top\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td>" + thname + "</td></tr>");
                        }
                        else if (dr["sumname"].ToString().IndexOf("洪峰") != -1)
                        {
                            typename = "一般洪峰";
                            if (k == 0)
                            {
                                if (i > 0 || j > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/blue.gif\" /></td><td><table id='threelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "',null);\"><td nowrap=nowrap valign=\"top\" style=\"width:50px;\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "',null);\"><td nowrap=nowrap valign=\"top\" style=\"width:50px;\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" align=\"left\" colspan=\"3\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td></tr>");//<td style=\"display:none;\" nowrap=nowrap valign=\"top\">" + dr["sumname"].ToString() + "</td><td style=\"display:none;\">" + dr["thname"].ToString() + "</td>
                        }
                        else
                        {
                            typename = dr["sumname"].ToString();
                            if (k == 0)
                            {
                                if (i > 0 || j > 0)
                                    strHtml.AppendLine("</table></td></tr>");
                                else
                                {

                                }
                                strHtml.AppendLine("<tr><td valign='top' align='center' width=\"24px;\"><img src=\"/images/warn/blue.gif\" /></td><td><table id='threelist' border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"0\">");

                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" style=\"width:25px;\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>
                            }
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["sttp"].ToString() + "\" style=\"cursor:hand;\" onclick=\"showDetail('" + sdate + "','" + dr["sumname"].ToString() + "','" + dr["thname"].ToString() + "','" + dr["sttp"].ToString() + "|3" + "',null);\"><td nowrap=nowrap valign=\"top\" width=\"10%\" align=\"left\">" + typename + "</td><td nowrap=nowrap valign=\"top\" style=\"width:25px;\"><a href=\"javascript:void(0);\">" + dr["znum"].ToString() + "站</a></td><td valign=\"top\" colspan=\"2\">" + dr["thname"].ToString() + "</td></tr>");//" + dr["sumname"].ToString() + "</td><td>
                        }
                        k++;
                    }
                    index++;
                }
                if (i > 0 || j > 0 || k > 0)
                    strHtml.AppendLine("</table></td></tr>");
                
                if (strList != "")
                    strHtml.AppendLine("</table><script defer='true'>if(document.getElementById('divdetail')!=undefined){document.getElementById('divdetail').innerHTML='';};" + strList + "</script>");//window.setTimeout(\"showlistall();\",3000);<script>window.setTimeout(\"showDetail('" + sn + "','" + tn + "');\",2000);</script>
                else
                    strHtml.AppendLine("</table><script>if(document.getElementById('divdetail')!=undefined){document.getElementById('divdetail').innerHTML='';}</script>");//<script>window.setTimeout(\"showDetail('" + sn + "','" + tn + "');\",2000);</script>
                
            }
            else
            {
                strHtml.AppendLine("</table>");
            }
            string str = strHtml.ToString();
            strHtml.Remove(0, strHtml.Length);
            return str;
        }
        /// <summary>
        /// 时间比较
        /// </summary>
        /// <param name="sourceDate">原时间</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public void CompareDateTime(string sourceDate, ref string startDate, ref string endDate)
        {
            if (String.Compare(sourceDate.Substring(0, sourceDate.IndexOf(" ")), DateTime.Now.ToString("yyyy-MM-dd")) < 0)
            {
                startDate = DateTime.Parse(sourceDate).AddHours(-24).ToString("yyyy-MM-dd HH:mm");
                endDate = DateTime.Parse(sourceDate).ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                startDate = DateTime.Parse(sourceDate).ToString("yyyy-MM-dd HH:mm");
                endDate = DateTime.Parse(sourceDate).AddHours(24).ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="strData">时间字符串</param>
        /// <param name="stype">类型</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        private string DateFormat(string strData, string stype)
        {
            string newstr = "";
            if (stype == "PP")
            {
                newstr = strData.Split('-')[1].Replace(" ", "日");
                newstr = newstr.Split(':')[0] + "时";
            }
            else if (stype == "ZZ")
            {
                newstr = strData.Split('-')[1].Replace(" ", "/");
            }
            else if (stype == "RR")
            {
                newstr = strData.Split('-')[1].Replace(" ", "/");
            }
            else if (stype == "FF")
            {
                newstr = strData.Split('-')[1].Replace(" ", "/");
            }
            else
                newstr = strData;
            return newstr;
        }
        /// <summary>
        /// 获取预警信息,内部数据使用
        /// </summary>
        /// <param name="source">数据表</param>
        /// <returns></returns>
        public DataSet ManageOneData(DataTable source)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("sumname");
            dt.Columns.Add("thname");
            dt.Columns.Add("znum");
            dt.Columns.Add("sjdj");
            dt.Columns.Add("sttp");
            dt.Columns.Add("qjfz");

            //站点数据，只存唯一站
            DataTable dtDetail = new DataTable();
            dtDetail.Columns.Add("stcd");
            dtDetail.Columns.Add("stnm");
            dtDetail.Columns.Add("sttp");
            dtDetail.Columns.Add("lgtd");
            dtDetail.Columns.Add("lttd");
            dtDetail.Columns.Add("legend");
            dtDetail.Columns.Add("qjfz");
            dtDetail.Columns.Add("rid");
            dtDetail.Columns.Add("sjdj");
            Dictionary<string, string> dicZD = new Dictionary<string, string>();
            string stcd = null, stnm = null, tm = null, sname = null, tname = null, sttp = null;//
            string qjfz = null;
            double drp = 0.0, z = 0.0, q = 0.0, drp_old = 0.0;//, qjfz = 0.0
            int sjdj = 0, zs = 0;
            foreach (DataRow dr in source.Rows)
            {
                if (stcd == null)
                {
                    stcd = dr["stcd"].ToString();
                    //stnm = dr["stnm"].ToString();
                    //tm = dr["tm"].ToString();
                    sname = dr["sumname"].ToString();
                    tname = dr["thname"].ToString();
                    sttp = dr["sttp"].ToString();
                    qjfz = dr["qjfz"].ToString();

                    sjdj = int.Parse(dr["sjdj"].ToString());
                    drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                    z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                    q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                    zs++;
                    dicZD.Add(dr["stcd"].ToString(), dr["stnm"].ToString());
                    DataRow detail_Row = dtDetail.NewRow();
                    detail_Row["stcd"] = stcd;
                    detail_Row["stnm"] = dr["stnm"];
                    detail_Row["sttp"] = dr["sttp"];
                    detail_Row["lgtd"] = dr["lggd"];
                    detail_Row["lttd"] = dr["lttd"];
                    detail_Row["legend"] = dr["legend"];
                    detail_Row["qjfz"] = dr["qjfz"];
                    detail_Row["sjdj"] = dr["sjdj"];
                    if (sname.Contains("前期"))
                        detail_Row["rid"] = dr["stnm"];
                    else if (dr["qjfz"].ToString() != "")
                        detail_Row["rid"] = "";
                    else
                        detail_Row["rid"] = "";
                    dtDetail.Rows.Add(detail_Row);
                }
                else if (stcd == dr["stcd"].ToString())//(sjdj == int.Parse(dr["sjdj"].ToString()))
                {
                    if (sname == dr["sumname"].ToString() && tname == dr["thname"].ToString())
                    {
                        if (sname.Contains("洪峰"))
                        {
                            //if (drp > double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString())))
                            if (drp < double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString())))
                            {
                                drp_old = drp;
                                drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                            }
                        }
                        else if (sname.Contains("水位"))
                        {
                            if (z < double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString())))
                            {
                                drp_old = drp;
                                drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                            }
                        }
                        else if (sname.Contains("流量"))
                        {
                            if (q < double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString())))
                            {
                                drp_old = drp;
                                drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                            }
                        }
                        else if (sname == "河道水情")
                        {
                            if (drp < double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString())))
                            {
                                drp_old = drp;
                                drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                            }
                        }
                        else if (sname == "水库水情")
                        {
                            if (z < double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString())))
                            {
                                drp_old = 1;
                                drp = 1;
                            }
                        }
                    }
                    else
                    {
                        DataRow new_dr = dt.NewRow();
                        new_dr["sumname"] = sname;
                        new_dr["thname"] = tname;
                        new_dr["znum"] = zs.ToString();
                        new_dr["sjdj"] = sjdj.ToString();
                        new_dr["sttp"] = sttp;
                        new_dr["qjfz"] = qjfz;

                        zs = 0;
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        //stnm = dr["stnm"].ToString();
                        //tm = dr["tm"].ToString();
                        sname = dr["sumname"].ToString();
                        tname = dr["thname"].ToString();
                        sttp = dr["sttp"].ToString();
                        qjfz = dr["qjfz"].ToString();

                        sjdj = int.Parse(dr["sjdj"].ToString());
                        drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                        z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                        q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                        zs++;
                    }
                }
                else
                {
                    if (drp_old > 0.0)
                    {
                        zs++;
                        drp_old = 0.0;
                    }
                    if (sname == dr["sumname"].ToString() && tname == dr["thname"].ToString())
                    {
                        zs++;
                        z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                        q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                        stcd = dr["stcd"].ToString();
                        drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                    }
                    else
                    {
                        DataRow new_dr = dt.NewRow();
                        new_dr["sumname"] = sname;
                        new_dr["thname"] = tname;
                        new_dr["znum"] = zs.ToString();
                        new_dr["sjdj"] = sjdj.ToString();
                        new_dr["sttp"] = sttp;
                        new_dr["qjfz"] = qjfz;

                        zs = 0;
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        //stnm = dr["stnm"].ToString();
                        //tm = dr["tm"].ToString();
                        sname = dr["sumname"].ToString();
                        tname = dr["thname"].ToString();
                        sttp = dr["sttp"].ToString();
                        qjfz = dr["qjfz"].ToString();

                        sjdj = int.Parse(dr["sjdj"].ToString());
                        drp = double.Parse((dr["drp"].ToString() == "" ? "0" : dr["drp"].ToString()));
                        z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                        q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                        zs++;
                    }
                    if (!dicZD.TryGetValue(dr["stcd"].ToString(), out stnm))
                    {
                        dicZD.Add(dr["stcd"].ToString(), dr["stnm"].ToString());
                        DataRow detail_Row = dtDetail.NewRow();
                        detail_Row["stcd"] = stcd;
                        detail_Row["stnm"] = dr["stnm"];
                        detail_Row["sttp"] = dr["sttp"];
                        detail_Row["lgtd"] = dr["lggd"];
                        detail_Row["lttd"] = dr["lttd"];
                        detail_Row["legend"] = dr["legend"];
                        detail_Row["qjfz"] = dr["qjfz"];
                        detail_Row["sjdj"] = dr["sjdj"];
                        if (dr["sumname"].ToString().Contains("前期"))
                            detail_Row["rid"] = dr["stnm"];
                        else if (dr["qjfz"].ToString() != "")
                            detail_Row["rid"] = "";
                        else
                            detail_Row["rid"] = "";
                        dtDetail.Rows.Add(detail_Row);
                    }
                    else
                    {
                        foreach (DataRow tdr in dtDetail.Rows)
                        {
                            if (tdr["stnm"].ToString() == stnm)
                            {
                                if (dr["sumname"].ToString().Contains("前期"))
                                {
                                    tdr["rid"] = dr["stnm"];
                                    tdr["sjdj"] = dr["sjdj"];
                                }
                                else if (dr["qjfz"].ToString() != "" && tdr["rid"].ToString() == "")
                                {
                                    tdr["rid"] = "";
                                    tdr["qjfz"] = dr["qjfz"];
                                    tdr["sjdj"] = dr["sjdj"];
                                }
                                break;
                            }
                        }
                    }
                }
            }
            if (zs > 0)
            {
                DataRow new_dr = dt.NewRow();
                new_dr["sumname"] = sname;
                new_dr["thname"] = tname;
                new_dr["znum"] = zs.ToString();
                new_dr["sjdj"] = sjdj.ToString();
                new_dr["sttp"] = sttp;
                new_dr["qjfz"] = qjfz;

                zs = 0;
                dt.Rows.Add(new_dr);
            }
            ds.Tables.Add(dt);
            ds.Tables.Add(dtDetail);
            return ds;
        }
        /// <summary>
        /// 河道水库水情,内部数据使用
        /// </summary>
        /// <param name="source">数据表</param>
        /// <param name="sname">名字</param>
        /// <returns></returns>
        private DataTable ManageHDSK(DataTable source, string sname)
        {
            DataTable dt = source.Clone();
            string stcd = null, tm = null, stnm = null, sttp = null, legend = null, sjjj = null, wptn = null, qjfz = null, rvnm = null;
            string lggd = null, lttd = null; double z = 0.0, q = 0.0, drp = 0;
            object objz = null, objq = null, objdrp = null, objqjfz = null;
            if (sname.Contains("洪峰"))
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        drp = double.Parse(dr["drp"].ToString());
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        if (drp > double.Parse(dr["drp"].ToString()))
                        {
                            stnm = dr["stnm"].ToString();
                            tm = dr["tm"].ToString();
                            sttp = dr["sttp"].ToString();
                            legend = dr["legend"].ToString();
                            sjjj = dr["sjjj"].ToString();
                            wptn = dr["wptn"].ToString();// == "" ? "0" : dr["wptn"].ToString());
                            lggd = (dr["lggd"].ToString() == "" ? "0" : dr["lggd"].ToString());
                            lttd = (dr["lttd"].ToString() == "" ? "0" : dr["lttd"].ToString());
                            objqjfz = dr["qjfz"];//.ToString();
                            objz = dr["z"];//.ToString();
                            objq = dr["q"];//.ToString();
                            drp = double.Parse(dr["drp"].ToString());
                            //rvnm = dr["rvnm"].ToString();
                        }
                    }
                    else
                    {
                        if (tm != null)
                        {
                            new_dr = dt.NewRow();
                            new_dr["stcd"] = stcd;
                            new_dr["stnm"] = stnm;
                            new_dr["tm"] = tm;
                            new_dr["sttp"] = sttp;
                            new_dr["legend"] = legend;
                            new_dr["sjjj"] = sjjj;
                            new_dr["wptn"] = wptn;
                            new_dr["lggd"] = lggd;
                            new_dr["lttd"] = lttd;
                            new_dr["z"] = objz;
                            new_dr["q"] = objq;
                            new_dr["drp"] = drp;
                            new_dr["qjfz"] = objqjfz;
                            //new_dr["rvnm"] = rvnm;
                            dt.Rows.Add(new_dr);
                            tm = null;
                        }
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["drp"] = dr["drp"];
                        new_dr["qjfz"] = dr["qjfz"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        drp = double.Parse(dr["drp"].ToString());
                    }
                }
                if (tm != null)
                {
                    new_dr = dt.NewRow();
                    new_dr["stcd"] = stcd;
                    new_dr["stnm"] = stnm;
                    new_dr["tm"] = tm;
                    new_dr["sttp"] = sttp;
                    new_dr["legend"] = legend;
                    new_dr["sjjj"] = sjjj;
                    new_dr["wptn"] = wptn;
                    new_dr["lggd"] = lggd;
                    new_dr["lttd"] = lttd;
                    new_dr["z"] = objz;
                    new_dr["q"] = objq;
                    new_dr["drp"] = drp;
                    new_dr["qjfz"] = objqjfz;
                    //new_dr["rvnm"] = rvnm;
                    dt.Rows.Add(new_dr);
                    tm = null;
                }
            }
            else if (sname.Contains("水位"))
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        z = double.Parse(dr["z"].ToString());
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        if (z < double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString())))
                        {
                            stnm = dr["stnm"].ToString();
                            tm = dr["tm"].ToString();
                            sttp = dr["sttp"].ToString();
                            legend = dr["legend"].ToString();
                            sjjj = dr["sjjj"].ToString();
                            wptn = dr["wptn"].ToString();
                            lggd = (dr["lggd"].ToString() == "" ? "0" : dr["lggd"].ToString());
                            lttd = (dr["lttd"].ToString() == "" ? "0" : dr["lttd"].ToString());
                            objqjfz = dr["qjfz"];//.ToString() == "" ? "0" : dr["qjfz"].ToString());
                            z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                            objq = dr["q"];//.ToString();
                            objdrp = dr["drp"];//.ToString();
                            //rvnm = dr["rvnm"].ToString();
                        }
                    }
                    else
                    {
                        if (tm != null)
                        {
                            new_dr = dt.NewRow();
                            new_dr["stcd"] = stcd;
                            new_dr["stnm"] = stnm;
                            new_dr["tm"] = tm;
                            new_dr["sttp"] = sttp;
                            new_dr["legend"] = legend;
                            new_dr["sjjj"] = sjjj;
                            new_dr["wptn"] = wptn;
                            new_dr["lggd"] = lggd;
                            new_dr["lttd"] = lttd;
                            new_dr["z"] = z;
                            new_dr["q"] = objq;
                            new_dr["drp"] = objdrp;
                            new_dr["qjfz"] = objqjfz;
                            //new_dr["rvnm"] = rvnm;
                            dt.Rows.Add(new_dr);
                            tm = null;
                        }
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        z = double.Parse((dr["z"].ToString() == "" ? "0" : dr["z"].ToString()));
                    }
                }
                if (tm != null)
                {
                    new_dr = dt.NewRow();
                    new_dr["stcd"] = stcd;
                    new_dr["stnm"] = stnm;
                    new_dr["tm"] = tm;
                    new_dr["sttp"] = sttp;
                    new_dr["legend"] = legend;
                    new_dr["sjjj"] = sjjj;
                    new_dr["wptn"] = wptn;
                    new_dr["lggd"] = lggd;
                    new_dr["lttd"] = lttd;
                    new_dr["z"] = z;
                    new_dr["q"] = objq;
                    new_dr["drp"] = objdrp;
                    new_dr["qjfz"] = objqjfz;
                    //new_dr["rvnm"] = rvnm;
                    dt.Rows.Add(new_dr);
                    tm = null;
                }
            }
            else if (sname.Contains("流量"))
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        if (q < double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString())))
                        {
                            stnm = dr["stnm"].ToString();
                            tm = dr["tm"].ToString();
                            sttp = dr["sttp"].ToString();
                            legend = dr["legend"].ToString();
                            sjjj = dr["sjjj"].ToString();
                            wptn = dr["wptn"].ToString();
                            lggd = (dr["lggd"].ToString() == "" ? "0" : dr["lggd"].ToString());
                            lttd = (dr["lttd"].ToString() == "" ? "0" : dr["lttd"].ToString());
                            objqjfz = dr["qjfz"];//.ToString() == "" ? "0" : dr["qjfz"].ToString());
                            objz = dr["z"];//.ToString();
                            q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                            objdrp = dr["drp"];//.ToString();
                            //rvnm = dr["rvnm"].ToString();
                        }
                    }
                    else
                    {
                        if (tm != null)
                        {
                            new_dr = dt.NewRow();
                            new_dr["stcd"] = stcd;
                            new_dr["stnm"] = stnm;
                            new_dr["tm"] = tm;
                            new_dr["sttp"] = sttp;
                            new_dr["legend"] = legend;
                            new_dr["sjjj"] = sjjj;
                            new_dr["wptn"] = wptn;
                            new_dr["lggd"] = lggd;
                            new_dr["lttd"] = lttd;
                            new_dr["z"] = objz;
                            new_dr["q"] = q;
                            new_dr["drp"] = objdrp;
                            new_dr["qjfz"] = objqjfz;
                            //new_dr["rvnm"] = rvnm;
                            dt.Rows.Add(new_dr);
                            tm = null;
                        }
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["drp"] = dr["drp"];
                        new_dr["qjfz"] = dr["qjfz"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        q = double.Parse((dr["q"].ToString() == "" ? "0" : dr["q"].ToString()));
                    }
                }
                if (tm != null)
                {
                    new_dr = dt.NewRow();
                    new_dr["stcd"] = stcd;
                    new_dr["stnm"] = stnm;
                    new_dr["tm"] = tm;
                    new_dr["sttp"] = sttp;
                    new_dr["legend"] = legend;
                    new_dr["sjjj"] = sjjj;
                    new_dr["wptn"] = wptn;
                    new_dr["lggd"] = lggd;
                    new_dr["lttd"] = lttd;
                    new_dr["z"] = objz;
                    new_dr["q"] = q;
                    new_dr["drp"] = objdrp;
                    new_dr["qjfz"] = objqjfz;
                    //new_dr["rvnm"] = rvnm;
                    dt.Rows.Add(new_dr);
                    tm = null;
                }
            }
            else if (sname == "河道水情")
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        drp = double.Parse(dr["drp"].ToString());
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        if (drp < double.Parse(dr["drp"].ToString()))
                        {
                            stnm = dr["stnm"].ToString();
                            tm = dr["tm"].ToString();
                            sttp = dr["sttp"].ToString();
                            legend = dr["legend"].ToString();
                            sjjj = dr["sjjj"].ToString();
                            wptn = dr["wptn"].ToString();
                            lggd = (dr["lggd"].ToString() == "" ? "0" : dr["lggd"].ToString());
                            lttd = (dr["lttd"].ToString() == "" ? "0" : dr["lttd"].ToString());
                            objqjfz = dr["qjfz"];//.ToString() == "" ? "0" : dr["qjfz"].ToString());
                            objz = dr["z"];//.ToString();
                            objq = dr["q"];//.ToString();
                            drp = double.Parse(dr["drp"].ToString());
                            //rvnm = dr["rvnm"].ToString();
                        }
                    }
                    else
                    {
                        if (tm != null)
                        {
                            new_dr = dt.NewRow();
                            new_dr["stcd"] = stcd;
                            new_dr["stnm"] = stnm;
                            new_dr["tm"] = tm;
                            new_dr["sttp"] = sttp;
                            new_dr["legend"] = legend;
                            new_dr["sjjj"] = sjjj;
                            new_dr["wptn"] = wptn;
                            new_dr["lggd"] = lggd;
                            new_dr["lttd"] = lttd;
                            new_dr["z"] = objz;
                            new_dr["q"] = objq;
                            new_dr["drp"] = drp;
                            new_dr["qjfz"] = objqjfz;
                            //new_dr["rvnm"] = rvnm;
                            dt.Rows.Add(new_dr);
                            tm = null;
                        }
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["drp"] = dr["drp"];
                        new_dr["qjfz"] = dr["qjfz"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        drp = double.Parse(dr["drp"].ToString());
                    }
                }
                if (tm != null)
                {
                    new_dr = dt.NewRow();
                    new_dr["stcd"] = stcd;
                    new_dr["stnm"] = stnm;
                    new_dr["tm"] = tm;
                    new_dr["sttp"] = sttp;
                    new_dr["legend"] = legend;
                    new_dr["sjjj"] = sjjj;
                    new_dr["wptn"] = wptn;
                    new_dr["lggd"] = lggd;
                    new_dr["lttd"] = lttd;
                    new_dr["z"] = objz;
                    new_dr["q"] = objq;
                    new_dr["drp"] = drp;
                    new_dr["qjfz"] = objqjfz;
                    //new_dr["rvnm"] = rvnm;
                    dt.Rows.Add(new_dr);
                    tm = null;
                }
            }
            else if (sname == "水库水情")
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["rwptn"] = dr["rwptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["rz"] = dr["rz"];
                        new_dr["w"] = dr["w"];
                        new_dr["qjfz"] = dr["qjfz"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        z = double.Parse(dr["rz"].ToString());
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        if (z < double.Parse(dr["rz"].ToString()))
                        {
                            stnm = dr["stnm"].ToString();
                            tm = dr["tm"].ToString();
                            sttp = dr["sttp"].ToString();
                            legend = dr["legend"].ToString();
                            sjjj = dr["sjjj"].ToString();
                            wptn = dr["rwptn"].ToString();
                            lggd = (dr["lggd"].ToString() == "" ? "0" : dr["lggd"].ToString());
                            lttd = (dr["lttd"].ToString() == "" ? "0" : dr["lttd"].ToString());
                            objqjfz = dr["qjfz"];//.ToString() == "" ? "0" : dr["qjfz"].ToString());
                            z = double.Parse(dr["rz"].ToString());
                            objq = dr["w"];//.ToString();
                        }
                    }
                    else
                    {
                        if (tm != null)
                        {
                            new_dr = dt.NewRow();
                            new_dr["stcd"] = stcd;
                            new_dr["stnm"] = stnm;
                            new_dr["tm"] = tm;
                            new_dr["sttp"] = sttp;
                            new_dr["legend"] = legend;
                            new_dr["sjjj"] = sjjj;
                            new_dr["rwptn"] = wptn;
                            new_dr["lggd"] = lggd;
                            new_dr["lttd"] = lttd;
                            new_dr["rz"] = z;
                            new_dr["w"] = objq;
                            new_dr["qjfz"] = objqjfz;
                            dt.Rows.Add(new_dr);
                            tm = null;
                        }
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["rwptn"] = dr["rwptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["rz"] = dr["rz"];
                        new_dr["w"] = dr["w"];
                        new_dr["qjfz"] = dr["qjfz"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                        z = double.Parse(dr["rz"].ToString());
                    }
                }
                if (tm != null)
                {
                    new_dr = dt.NewRow();
                    new_dr["stcd"] = stcd;
                    new_dr["stnm"] = stnm;
                    new_dr["tm"] = tm;
                    new_dr["sttp"] = sttp;
                    new_dr["legend"] = legend;
                    new_dr["sjjj"] = sjjj;
                    new_dr["rwptn"] = wptn;
                    new_dr["lggd"] = lggd;
                    new_dr["lttd"] = lttd;
                    new_dr["rz"] = z;
                    new_dr["w"] = objq;
                    new_dr["qjfz"] = objqjfz;
                    dt.Rows.Add(new_dr);
                    tm = null;
                }
            }
            else
            {
                DataRow new_dr = null;
                foreach (DataRow dr in source.Rows)
                {
                    if (stcd == null)
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["qjfz"] = dr["qjfz"];
                        new_dr["drp"] = dr["drp"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                    }
                    else if (stcd == dr["stcd"].ToString())
                    {
                        continue;
                    }
                    else
                    {
                        new_dr = dt.NewRow();
                        new_dr["stcd"] = dr["stcd"];
                        new_dr["stnm"] = dr["stnm"];
                        new_dr["tm"] = dr["tm"];
                        new_dr["sttp"] = dr["sttp"];
                        new_dr["legend"] = dr["legend"];
                        new_dr["sjjj"] = dr["sjjj"];
                        new_dr["wptn"] = dr["wptn"];
                        new_dr["lggd"] = dr["lggd"];
                        new_dr["lttd"] = dr["lttd"];
                        new_dr["z"] = dr["z"];
                        new_dr["q"] = dr["q"];
                        new_dr["drp"] = dr["drp"];
                        new_dr["qjfz"] = dr["qjfz"];
                        //new_dr["rvnm"] = dr["rvnm"];
                        dt.Rows.Add(new_dr);
                        stcd = dr["stcd"].ToString();
                    }
                }
            }
            return (dt.Rows.Count == 0 ? source : dt);
        }
        /// <summary>
        /// 根据指定的sname和tname返回对应的预警站点列表
        /// </summary>
        /// <param name="sname">预警类型名称</param>
        /// <param name="tname">具体预警类别</param>
        /// <param name="sttp">测站类型</param>
        /// <param name="sdate">时间</param>
        /// <param name="qjfz"></param>
        /// <returns></returns>
        public string Get_WatchWarnDetailData(string sname, string tname, string sttp, string sdate, string qjfz)
        {
            StringBuilder strHtml = new StringBuilder();
            string sd = null, ed = null;
            CompareDateTime(sdate, ref sd, ref ed);
            DataTable dt = repository.GetWarnWarnDetailData(sname, tname, sd, ed, sttp.Split('|')[0], qjfz);
            string script = "";
            string sj = sttp.Split('|')[1];
            string fcolor = "";
            if (sj == "1")
                fcolor = "#FF0000";
            else if (sj == "2")
                fcolor = "#F90BFF";
            else
                fcolor = "#0000FF";
            strHtml.AppendLine("<table id=\"tblDetail\" border=\"0\" align=\"center\" width=\"100%\" cellpadding=\"2\" cellspacing=\"0\">");
            sttp = sttp.Split('|')[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (sttp == "ZZ" || sttp == "ZQ")
                {
                    dt = ManageHDSK(dt, sname);
                    if (sname.IndexOf("洪峰") > -1)
                    {
                        string tem = null, temjj = null, stnm = "";
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">流量</th><th align=\"center\">排位</th></tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            temjj = dr["sjjj"].ToString().Substring(dr["sjjj"].ToString().LastIndexOf("】") + 1);
                            temjj = temjj.Substring(temjj.IndexOf("第") + 1, temjj.IndexOf("名") - (temjj.IndexOf("第") + 1));
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr title=\"" + dr["stnm"].ToString() + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + stnm + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</fon></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + temjj + "</font></td></tr>");//dr["wptn"].ToString() + 
                            else
                                strHtml.AppendLine("<tr title=\"" + dr["stnm"].ToString() + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + dr["stnm"].ToString() + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\" valign=\"top\"><font color=\"" + fcolor + "\">" + temjj + "</font></td></tr>");
                        }
                    }
                    else if (sname == "河道水情" || sname == "河道水位" || sname == "河道流量")
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">流量</th><th align=\"center\">超警值</th></tr>");//
                        string ls = "", tem = "", stnm = "";
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[\u4e00-\u9fa5]+");
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = dr["sjjj"].ToString().Substring(dr["sjjj"].ToString().LastIndexOf("】") + 1);
                            ls = reg.Replace(ls, "").Replace("(m3/s)", "").Replace("m", "");
                            if (ls.IndexOf(" ") > 0)
                                ls = double.Parse(ls.Split(' ')[0]).ToString() + "/" + double.Parse(ls.Split(' ')[1]).ToString();
                            else
                                ls = double.Parse(ls.Replace("/", "")).ToString();
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                            {
                                strHtml.AppendLine("<tr title=\"" + stnm + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + dr["stnm"].ToString() + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            }
                            else
                            {
                                strHtml.AppendLine("<tr title=\"" + stnm + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            }
                        }
                    }
                    else if (sname == "含沙量")
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">流量</th><th align=\"center\">含沙量</th></tr>");
                        string ls = "", tem = "", stnm = "";
                        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[\u4e00-\u9fa5]+");
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = double.Parse(dr["drp"].ToString()).ToString();
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                            {
                                strHtml.AppendLine("<tr title=\"" + stnm + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + dr["stnm"].ToString() + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + dr["stnm"].ToString() + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            }
                            else
                            {
                                strHtml.AppendLine("<tr title=\"" + stnm + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            }
                        }
                    }
                    else
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">流量</th><th align=\"center\">说明</th></tr>");
                        string ls = "", tem = "", stnm = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr title=\"" + stnm + "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + stnm + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td><font color=\"" + fcolor + "\">" + dr["sjjj"].ToString().Substring(dr["sjjj"].ToString().LastIndexOf("】") + 1) + "</font></td></tr>");
                            else
                                strHtml.AppendLine("<tr title=\"" + stnm+ "\"><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["z"].ToString() + "</font>" + dr["wptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["q"].ToString() + "</font></td><td><font color=\"" + fcolor + "\">" + dr["sjjj"].ToString().Substring(dr["sjjj"].ToString().LastIndexOf("】") + 1) + "</font></td></tr>");
                        }
                    }
                }
                else if (sttp == "PP")
                {
                    if (qjfz == "null")
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" nowrap=nowrap valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">历时</th><th align=\"center\" nowrap=nowrap valign=\"top\">雨量</th><th align=\"center\" nowrap=nowrap valign=\"top\">天气</th></tr>");
                        string ls = "", tem = "", stnm = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = dr["sjjj"].ToString();
                            ls = ls.Substring(ls.LastIndexOf("】"));
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + dr["stnm"].ToString() + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"center\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"center\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + ls.Substring(1, ls.IndexOf("小时") + 1).Replace("小时", "") + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + ls.Substring(ls.IndexOf("降雨") + 2).Replace("毫米", "") + "</font></td><td align=\"center\" valign=\"top\"><font color=\"" + fcolor + "\">" + dr["wth"].ToString() + "</font></td></tr>");
                            else
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"center\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"center\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + ls.Substring(1, ls.IndexOf("小时") + 1).Replace("小时", "") + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + ls.Substring(ls.IndexOf("降雨") + 2).Replace("毫米", "") + "</font></td><td align=\"center\" valign=\"top\"><font color=\"" + fcolor + "\">" + dr["wth"].ToString() + "</font></td></tr>");
                        }
                    }
                    else
                    {
                        string ls = tname.Substring(tname.IndexOf("超"));
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">区间</th><th align=\"center\" nowrap=nowrap valign=\"top\">总站数</th><th align=\"center\" nowrap=nowrap valign=\"top\" colspan=\"3\">" + ls + "站数</th></tr>");//<th align=\"center\" nowrap=nowrap valign=\"top\"></th><th align=\"center\" nowrap=nowrap valign=\"top\"></th>
                        string xylist = "";
                        int count = 0;
                        string tem = "";//, xylist = "";
                        StringBuilder str = new StringBuilder();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (count == 0)
                            {
                                str.AppendLine("<script type=\"text/javascript\"> var sec_list=[");
                                tem = dr["zs"].ToString();
                                str.Append("[{sn:'" + sname + "',tn:'" + ls + "站数',zs:" + tem + ",qj:'" + dr["rid"].ToString() + "',xmax:" + dr["xmax"].ToString() + ",ymax:" + dr["ymax"].ToString() + ",xmin:" + dr["xmin"].ToString() + ",ymin:" + dr["ymin"].ToString() + ",fc:'" + fcolor + "'},");
                            }
                            str.Append("{stcd:\"" + dr["stcd"].ToString() + "\",stnm:\"" + dr["stnm"].ToString() + "\",legend:\"" + dr["legend"].ToString() + "\",sttp:\"" + dr["sttp"].ToString() + "\",lgtd:" + dr["lggd"].ToString() + ",lttd:" + dr["lttd"].ToString() + "},");
                            str.AppendLine("{stcd:'" + dr["stcd"].ToString() + "',stnm:'" + dr["stnm"].ToString() + "',legend:'" + dr["legend"].ToString() + "',sttp:'" + dr["sttp"].ToString() + "',lgtd:" + dr["lggd"].ToString() + ",lttd:" + dr["lttd"].ToString() + "},");
                            xylist += dr["lggd"].ToString() + "," + dr["lttd"].ToString() + ";"; 
                            count++;
                        }
                        if (count > 0)
                        {
                            script = str.ToString();
                            script = script.Substring(0, script.LastIndexOf(','));//showSec_qy('" + xylist + "')
                            script += "]";
                            script += "];</script>";
                        }
                        strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showSection(eval('('+" + script + "+')'));\">" + sname + "</a></td><td align=\"center\" nowrap=nowrap valign=\"top\">" + tem + "</td><td align=\"center\" nowrap=nowrap valign=\"top\" colspan=\"3\">" + count.ToString() + "</td></tr>");

                    }
                }
                else
                {
                    dt = ManageHDSK(dt, sname);
                    if (tname.Contains("汛限"))
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" nowrap=nowrap valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">蓄量</th><th nowrap=nowrap valign=\"top\">超汛限</th></tr>");
                        string ls = "", tem = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            string stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = dr["sjjj"].ToString();
                            ls = ls.Substring(ls.LastIndexOf("】") + 1);
                            ls = ls.Substring(5, ls.IndexOf("米") - 5);
                            ls = Convert.ToDouble(ls).ToString();
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + stnm + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["w"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            else
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["w"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                        }
                    }
                    else if (tname.Contains("正常高"))
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" nowrap=nowrap valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th align=\"center\" nowrap=nowrap valign=\"top\">蓄量</th><th nowrap=nowrap valign=\"top\">超正常高</th></tr>");
                        string ls = "", tem = "", stnm = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            stnm = dr["stnm"].ToString();
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = dr["sjjj"].ToString();
                            ls = ls.Substring(ls.LastIndexOf("】") + 1);
                            ls = ls.Substring(6, ls.IndexOf("米") - 6);
                            ls = Convert.ToDouble(ls).ToString();
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + stnm + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["w"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            else
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["w"].ToString() + "</font></td><td align=\"right\"><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                        }
                    }
                    else
                    {
                        strHtml.AppendLine("<tr><th align=\"center\" nowrap=nowrap valign=\"top\">站名</th><th align=\"center\" nowrap=nowrap valign=\"top\">时间</th><th align=\"center\" nowrap=nowrap valign=\"top\">水位</th><th nowrap=nowrap valign=\"top\">说明</th></tr>");
                        string ls = "", tem = "", stnm = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            tem = DateFormat(dr["tm"].ToString(), sttp);
                            ls = dr["sjjj"].ToString();
                            ls = ls.Substring(ls.LastIndexOf("】") + 1);
                            if (double.Parse(dr["lggd"].ToString()) > 0)
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:showStation('" + dr["stcd"].ToString() + "','" + dr["stnm"].ToString() + "','" + dr["legend"].ToString() + "','" + dr["sttp"].ToString() + "'," + dr["lggd"].ToString() + "," + dr["lttd"].ToString() + "," + (dr["qjfz"].ToString() == "" ? "null" : dr["qjfz"].ToString()) + ");\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                            else
                                strHtml.AppendLine("<tr><td align=\"left\" nowrap=nowrap valign=\"top\"><a href=\"javascript:ShowReport('" + dr["stcd"].ToString() + "','" + dr["sttp"].ToString() + "');\"><font color=\"" + fcolor + "\">" + stnm + "</font></a></td><td align=\"left\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + tem + "</font></td><td align=\"right\" nowrap=nowrap valign=\"top\"><font color=\"" + fcolor + "\">" + dr["rz"].ToString() + "</font>" + dr["rwptn"].ToString() + "</td><td><font color=\"" + fcolor + "\">" + ls + "</font></td></tr>");
                        }
                    }
                }
            }
            else
            {
                strHtml.AppendLine("<tr><td>该信息已过有效期<td></tr>");
            }
            strHtml.AppendLine("</table>");
            if (script.Length > 0)
            {
                strHtml.Remove(0, strHtml.Length);
                strHtml.Insert(0, script);
            }
            string strTem = strHtml.ToString();
            strHtml.Remove(0, strHtml.Length);
            return strTem;
        }
    }
}
