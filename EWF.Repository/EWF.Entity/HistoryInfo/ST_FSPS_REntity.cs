/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijuan
 * 日  期：2019/5/30 18:30:13
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
    [Table(EWFConsts.RTDB_SchemaName + "ST_FSPS_R")]
    public class ST_FSPS_REntity
    {
        [Key]
        public int ID { get; set; }
        public string STCD { get; set; }
        public DateTime TM { get; set; }
        [Key]
        public decimal? Z { get; set; }
        public decimal? Q { get; set; }
        public decimal? S { get; set; }
    }
}
