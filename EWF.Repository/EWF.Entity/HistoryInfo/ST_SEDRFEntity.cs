/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 16:54:51
 * 描  述：ST_SEDRF
 * *********************************************/
using EWF.Util.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table(EWFConsts.RTDB_SchemaName + "ST_SEDRF_R")]
    public class ST_SEDRFEntity
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        public DateTime IDTM { get; set; }
        [Key]
        public string STTDRCD { get; set; }
        public string STNM { get; set; }
        public double? WRNF { get; set; }
        public double? STW { get; set; }
    }
}
