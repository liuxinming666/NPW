using EWF.Entity;
using EWF.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.IServices
{
    /// <summary>
    /// 气象信息接口
    /// </summary>
    public interface IWeatherService : IDependency
    {
        /// <summary>
        /// 获取指定类型的最新一张气象图片
        /// </summary>
        /// <param name="sType">气象类型：st云图，rd雷达，yb天气预报</param>
        /// <param name="url">网络路径</param>
        /// <param name="encode">网页编码</param>
        /// <param name="strTag">位置的特殊标志</param>
        /// <returns></returns>
        string GetShowWeather(string sType, string url, string encode, string strTag);
    }
}
