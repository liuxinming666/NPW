using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using EWF.IRepository;
using EWF.IServices;
using EWF.Util;

namespace EWF.Services
{
    public class WeatherService :IWeatherService
    {
        /// <summary>
        /// 获取指定类型的最新一张气象图片
        /// </summary>
        /// <param name="sType">气象类型：st云图，rd雷达，yb天气预报</param>
        /// <param name="url">网络路径</param>
        /// <param name="encode">网页编码</param>
        /// <param name="strTag">位置的特殊标志</param>
        /// <returns></returns>
        public string GetShowWeather(string sType,string url,string encode,string strTag)
        {
            string strRen = "";
            switch (sType)
            {
                case "st":
                    break;
                case "rd":
                    break;
                case "yb":
                    strRen=GetLastImg(url, encode, strTag);
                    strRen +="|"+ GetLastImg(url.Replace("1-day","2-day"), encode, strTag);
                    strRen +="|"+ GetLastImg(url.Replace("1-day", "3-day"), encode, strTag);
                    break;
                default:
                    break;
            }
            return strRen;
        }
        private string GetLastImg(string surl, string ecode, string strTag)
        {
            HttpWebRequest hwr;
            hwr = (HttpWebRequest)WebRequest.Create(surl);
            StreamReader streamr = new StreamReader(hwr.GetResponse().GetResponseStream(), System.Text.Encoding.GetEncoding(ecode));
            string pagedata = streamr.ReadToEnd();
            streamr.Close();
            streamr.Dispose();
            int start = pagedata.IndexOf(strTag);
            start = pagedata.IndexOf("http", start);
            string imgPath = pagedata.Substring(start, pagedata.IndexOf("\"", start) - start);
            return imgPath;
        }
    }
}
