/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/22 13:30:13
 * 描  述：ST_PSTAT_R
 * *********************************************/
using EWF.Util.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table(EWFConsts.RTDB_SchemaName + "ST_PSTAT_R")]
    public class ST_PSTATEntity
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        public DateTime IDTM { get; set; }
        [Key]
        public string STTDRCD { get; set; }
        public double? ACCP { get; set; }
        public string STNM { get; set; }
    }
}
