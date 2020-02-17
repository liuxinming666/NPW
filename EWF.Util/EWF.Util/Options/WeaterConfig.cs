using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Options
{
    /// <summary>
    /// 气象配置
    /// </summary>
    public class WeaterConfig
    {
        /// <summary>
        /// 网络路径
        /// </summary>
        public string url { set; get; }
        /// <summary>
        /// 所需编码
        /// </summary>
        public string encode { set; get; }
        /// <summary>
        /// 取值标志
        /// </summary>
        public string strTag { set; get; }
        /// <summary>
        /// 视频服务中的视频Web端前缀例如：http://10.4.94.131:8586
        /// </summary>
        public string VideoPreUrl { get; set; }
    }
}
