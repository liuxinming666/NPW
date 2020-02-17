/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/1/25 16:43:55
 * 描  述：DbOption
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Options
{
    public class DbOption
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }
    }
}
