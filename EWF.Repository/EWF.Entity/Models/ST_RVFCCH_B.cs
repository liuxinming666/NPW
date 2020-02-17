/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：qiulijuan
 * 日  期：2019/6/14 14:59:29
 * 描  述：ST_RVFCCH_B
 * *********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table("ST_RVFCCH_B")]
    public class ST_RVFCCH_B
    {        
        [Key]
        public string STCD { get; set; }
        public decimal? LDKEL { get; set; }
        public decimal? RDKEL { get; set; }
        public decimal? WRZ { get; set; }
        public decimal? WRQ { get; set; }
        public decimal? GRZ { get; set; }
        public decimal? GRQ { get; set; }
        public decimal? FLPQ { get; set; }
        public decimal? OBHTZ { get; set; }
        public DateTime? OBHTZTM { get; set; }
        public decimal? IVHZ { get; set; }
        public DateTime? IVHZTM { get; set; }
        public decimal? OBMXQ { get; set; }
        public DateTime? OBMXQTM { get; set; }
        public decimal? IVMXQ { get; set; }
        public DateTime? IVMXQTM { get; set; }
        public decimal? HMXS { get; set; }
        public DateTime? HMXSTM { get; set; }
        public decimal? HMXAVV { get; set; }
        public DateTime? HMXAVVTM { get; set; }
        public decimal? HLZ { get; set; }
        public DateTime? HLZTM { get; set; }
        public decimal? HMNQ { get; set; }
        public DateTime? HMNQTM { get; set; }
        public decimal? FRZ { get; set; }
        public decimal? FRQ { get; set; }
    }
}
