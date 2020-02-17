using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.Util.Options
{
    /// <summary>
    /// 对系统所需演示数据的配置
    /// </summary>
    public class DataOption
    {
        /// <summary>
        /// 时间间隔(单位：天)--获取最新数据时用
        /// </summary>
        public int Interval { set; get; }
        /// <summary>
        /// 实时库名字
        /// </summary>
        public string RDBName { set; get; }
        /// <summary>
        /// 首页-河道水情过程线时间间隔(天)
        /// </summary>
        public int SysRiver { set; get; }
        /// <summary>
        /// 首页-水库水情过程线时间间隔(天)
        /// </summary>
        public int SysRsvr { set; get; }
        /// <summary>
        /// 首页-昨日雨量柱状图时间间隔(天)
        /// </summary>
        public int SysDYP { set; get; }
        /// <summary>
        /// 首页-24小时降雨柱状图时间间隔(小时)
        /// </summary>
        public int Sys24DRP { set; get; }
        /// <summary>
        /// 首页-在线水位过程线时间间隔(小时)
        /// </summary>
        public int SysOnlineZ { get; set; }
    }
}
