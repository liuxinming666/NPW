/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/20 20:59:29
 * 描  述：ST_RIVER_R
 * *********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table("ST_RIVER_R")]
    public class ST_RIVEREntity
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        public DateTime TM { get; set; }
        public string STNM { get; set; }
        public string FLWCHRCD { get; set; }
        public string WPTN { get; set; }
        public string MSQMT { get; set; }
        public string MSAMT { get; set; }
        public string MSVMT { get; set; }
        public double? XSMXV { get; set; }
        public double? Z { get; set; }
        public double? Q { get; set; }
        public double? XSA { get; set; }
        public double? XSAVV { get; set; }

        public double? S { get; set; }
    }
}
