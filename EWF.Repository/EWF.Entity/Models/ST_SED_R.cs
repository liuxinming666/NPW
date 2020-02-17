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
    [Table("ST_SED_R")]
    public class ST_SED_R
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        public DateTime TM { get; set; }
        public decimal S { get; set; }
        public string SCHRCD { get; set; }
        public string SMT { get; set; }
    }
}
