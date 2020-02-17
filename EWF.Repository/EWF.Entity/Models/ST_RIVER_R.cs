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

namespace EWF.Entity.Models
{
    [Table("ST_RIVER_R")]
    public class ST_RIVER_R
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        public DateTime TM { get; set; }
        public string FLWCHRCD { get; set; }
        public string WPTN { get; set; }
        public string MSQMT { get; set; }
        public string MSAMT { get; set; }
        public string MSVMT { get; set; }
        public decimal XSMXV { get; set; }
        public decimal Z { get; set; }
        public decimal Q { get; set; }
        public decimal XSA { get; set; }
        public decimal XSAVV { get; set; }
    }
}
